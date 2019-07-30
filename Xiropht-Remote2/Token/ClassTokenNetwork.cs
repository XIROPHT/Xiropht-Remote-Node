using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Xiropht_Connector_All.RPC;
using Xiropht_Connector_All.Setting;
using Xiropht_Connector_All.Utils;
using Xiropht_RemoteNode.Utils;

namespace Xiropht_RemoteNode.Token
{
    public class ClassTokenNetwork
    {
        public const string PacketNotExist = "not_exist";
        public const string PacketResult = "result";

        public static async Task<bool> CheckWalletAddressExistAsync(string walletAddress)
        {


            Dictionary<string, int> ListOfSeedNodesSpeed = new Dictionary<string, int>();
            foreach (var seedNode in ClassConnectorSetting.SeedNodeIp)
            {

                try
                {
                    int seedNodeResponseTime = -1;
                    Task taskCheckSeedNode = Task.Run(() => seedNodeResponseTime = CheckPing.CheckPingHost(seedNode.Key, true));
                    taskCheckSeedNode.Wait(ClassConnectorSetting.MaxPingDelay);
                    if (seedNodeResponseTime == -1)
                    {
                        seedNodeResponseTime = ClassConnectorSetting.MaxSeedNodeTimeoutConnect;
                    }
                    ListOfSeedNodesSpeed.Add(seedNode.Key, seedNodeResponseTime);

                }
                catch
                {
                    ListOfSeedNodesSpeed.Add(seedNode.Key, ClassConnectorSetting.MaxSeedNodeTimeoutConnect); // Max delay.
                }

            }

            ListOfSeedNodesSpeed = ListOfSeedNodesSpeed.OrderBy(u => u.Value).ToDictionary(z => z.Key, y => y.Value);


            bool success = false;

            foreach (var seedNode in ListOfSeedNodesSpeed)
            {
                if (!success)
                {
                    try
                    {
                        string randomSeedNode = seedNode.Key;
                        string request = ClassConnectorSettingEnumeration.WalletTokenType + "|" + ClassRpcWalletCommand.TokenCheckWalletAddressExist + "|" + walletAddress;
                        string result = await ProceedHttpRequest("http://" + randomSeedNode + ":" + ClassConnectorSetting.SeedNodeTokenPort + "/", request);
                        if (result == string.Empty || result == PacketNotExist)
                        {
                            success = false;
                        }
                        else
                        {
                            JObject resultJson = JObject.Parse(result);
                            if (resultJson.ContainsKey(PacketResult))
                            {
                                string resultCheckWalletAddress = resultJson[PacketResult].ToString();
                                if (resultCheckWalletAddress.Contains("|"))
                                {
                                    var splitResultCheckWalletAddress = resultCheckWalletAddress.Split(new[] { "|" }, StringSplitOptions.None);
                                    if (splitResultCheckWalletAddress[0] == ClassRpcWalletCommand.SendTokenCheckWalletAddressInvalid)
                                    {
                                        success = false;
                                    }
                                    else if (splitResultCheckWalletAddress[0] == ClassRpcWalletCommand.SendTokenCheckWalletAddressValid)
                                    {
                                        return true;
                                    }
                                    else
                                    {
                                        success = false;
                                    }
                                }
                                else
                                {
                                    success = false;
                                }
                            }
                            else
                            {
                                success = false;
                            }
                        }
                    }
                    catch
                    {
                        success = false;
                    }
                }
            }
            return success;

        }

        private static async Task<string> ProceedHttpRequest(string url, string requestString)
        {
            string result = string.Empty;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + requestString);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.ServicePoint.Expect100Continue = false;
            request.KeepAlive = false;
            request.Timeout = 10000;
            request.UserAgent = ClassConnectorSetting.CoinName + "Solo Miner - " + Assembly.GetExecutingAssembly().GetName().Version + "R";
            string responseContent = string.Empty;
            using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
            {
                result = await reader.ReadToEndAsync();
            }

            return result;
        }
    }
}
