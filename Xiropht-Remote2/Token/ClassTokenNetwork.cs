using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Xiropht_Connector_All.RPC;
using Xiropht_Connector_All.Setting;
using Xiropht_RemoteNode.Utils;

namespace Xiropht_RemoteNode.Token
{
    public class ClassTokenNetwork
    {
        public const string PacketNotExist = "not_exist";
        public const string PacketResult = "result";

        public static async Task<bool> CheckWalletAddressExistAsync(string walletAddress)
        {
            try
            {
                string randomSeedNode = ClassConnectorSetting.SeedNodeIp.ElementAt(ClassUtilsNode.GetRandomBetween(0, ClassConnectorSetting.SeedNodeIp.Count - 1)).Key;
                string request = ClassConnectorSettingEnumeration.WalletTokenType + "|" + ClassRpcWalletCommand.TokenCheckWalletAddressExist + "|" + walletAddress;
                string result = await ProceedHttpRequest("http://" + randomSeedNode + ":" + ClassConnectorSetting.SeedNodeTokenPort + "/", request);
                if (result == string.Empty || result == PacketNotExist)
                {
                    return false;
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
                                return false;
                            }
                            else if (splitResultCheckWalletAddress[0] == ClassRpcWalletCommand.SendTokenCheckWalletAddressValid)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
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
