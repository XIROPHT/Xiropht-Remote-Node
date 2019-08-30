using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xiropht_Connector_All.RPC;
using Xiropht_Connector_All.RPC.Token;
using Xiropht_Connector_All.Setting;
using Xiropht_Connector_All.Utils;
using Xiropht_RemoteNode.Data;

namespace Xiropht_RemoteNode.Token
{
    public class ClassTokenNetwork
    {
        public const string PacketNotExist = "not_exist";
        public const string PacketResult = "result";

        private static Dictionary<string, int> _listOfSeedNodesSpeed;

        /// <summary>
        /// Check if the wallet address exist on the network.
        /// </summary>
        /// <param name="walletAddress"></param>
        /// <returns></returns>
        public static async Task<bool> CheckWalletAddressExistAsync(string walletAddress)
        {
            if (ClassRemoteNodeSync.DictionaryCacheValidWalletAddress.ContainsKey(walletAddress))
            {
                return true;
            }

            foreach (var seedNode in GetListOfSeedNodeSpeed())
            {
                try
                {
                    string request = ClassConnectorSettingEnumeration.WalletTokenType + ClassConnectorSetting.PacketContentSeperator + ClassRpcWalletCommand.TokenCheckWalletAddressExist + ClassConnectorSetting.PacketContentSeperator + walletAddress;
                    string result = await ProceedHttpRequest("http://" + seedNode.Key + ":" + ClassConnectorSetting.SeedNodeTokenPort + "/", request);
                    if (result != string.Empty && result != PacketNotExist)
                    {
                        JObject resultJson = JObject.Parse(result);
                        if (resultJson.ContainsKey(PacketResult))
                        {
                            string resultCheckWalletAddress = resultJson[PacketResult].ToString();
                            if (resultCheckWalletAddress.Contains(ClassConnectorSetting.PacketContentSeperator))
                            {
                                var splitResultCheckWalletAddress = resultCheckWalletAddress.Split(new[] { ClassConnectorSetting.PacketContentSeperator }, StringSplitOptions.None);

                                if (splitResultCheckWalletAddress[0] == ClassRpcWalletCommand.SendTokenCheckWalletAddressValid)
                                {
                                    try
                                    {
                                        if (!ClassRemoteNodeSync.DictionaryCacheValidWalletAddress.ContainsKey(
                                            walletAddress))
                                        {
                                            ClassRemoteNodeSync.DictionaryCacheValidWalletAddress.Add(walletAddress, string.Empty);
                                        }
                                    }
                                    catch
                                    {

                                    }
                                    return true;
                                }
                            }
                        }
                    }
                }
                catch
                {
                    // ignored
                }
            }
            return false;

        }

        /// <summary>
        /// Get a question encrypted in advance linked to a wallet address.
        /// </summary>
        /// <param name="walletAddress"></param>
        /// <returns></returns>
        public static async Task<string> GetWalletQuestionConfirmation(string walletAddress)
        {
            foreach (var seedNode in GetListOfSeedNodeSpeed())
            {
                try
                {
                    string request = ClassConnectorSettingEnumeration.WalletTokenType + ClassConnectorSetting.PacketContentSeperator + ClassRpcWalletCommand.TokenAskWalletQuestion + ClassConnectorSetting.PacketContentSeperator + walletAddress;
                    string result = await ProceedHttpRequest("http://" + seedNode.Key + ":" + ClassConnectorSetting.SeedNodeTokenPort + "/", request);
                    if (!string.IsNullOrEmpty(result))
                    {
                        if (result != PacketNotExist)
                        {
                            JObject resultJson = JObject.Parse(result);
                            if (resultJson.ContainsKey(PacketResult))
                            {
                                if (resultJson[PacketResult].ToString() != PacketNotExist)
                                {
                                    string resultCheckWalletAddress = resultJson[PacketResult].ToString();
                                    if (resultCheckWalletAddress.Contains(ClassConnectorSetting.PacketContentSeperator))
                                    {
                                        var splitResultCheckWalletAddress =
                                            resultCheckWalletAddress.Split(
                                                new[] {ClassConnectorSetting.PacketContentSeperator},
                                                StringSplitOptions.None);

                                        if (splitResultCheckWalletAddress[0] ==
                                            ClassRpcWalletCommand.SendTokenWalletQuestion)
                                        {
                                            return splitResultCheckWalletAddress[1].Replace(ClassConnectorSetting.PacketSplitSeperator, "");
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
                catch
                {
                    // ignored
                }
            }

            return string.Empty;
        }


        /// <summary>
        /// Get the last blocktemplate.
        /// </summary>
        /// <returns></returns>
        public static async Task<ClassTokenBlocktemplate> GetLastBlocktemplate()
        {
            foreach (var seedNode in GetListOfSeedNodeSpeed())
            {
                try
                {
                    string request = ClassRpcWalletCommand.TokenAskBlocktemplate;
                    string result = await ProceedHttpRequest("http://" + seedNode.Key + ":" + ClassConnectorSetting.SeedNodeTokenPort + "/", request);
                    if (!string.IsNullOrEmpty(result))
                    {
                        if (result != PacketNotExist)
                        {
                           return JsonConvert.DeserializeObject<ClassTokenBlocktemplate>(result);
                        }
                    }
                }
                catch
                {
                    // ignored
                }
            }

            return null;
        }

        /// <summary>
        /// Generate the list of seed nodes sorted by their ping time and return it.
        /// </summary>
        /// <returns></returns>
        private static Dictionary<string, int> GetListOfSeedNodeSpeed()
        {
            if (_listOfSeedNodesSpeed == null)
            {
                _listOfSeedNodesSpeed = new Dictionary<string, int>();
            }

            if (_listOfSeedNodesSpeed.Count != ClassConnectorSetting.SeedNodeIp.Count)
            {
                _listOfSeedNodesSpeed.Clear();
            }

            if (_listOfSeedNodesSpeed.Count == 0)
            {
                foreach (var seedNode in ClassConnectorSetting.SeedNodeIp.ToArray())
                {

                    try
                    {
                        int seedNodeResponseTime = -1;
                        Task taskCheckSeedNode = Task.Run(() =>
                            seedNodeResponseTime = CheckPing.CheckPingHost(seedNode.Key, true));
                        taskCheckSeedNode.Wait(ClassConnectorSetting.MaxPingDelay);
                        if (seedNodeResponseTime == -1)
                        {
                            seedNodeResponseTime = ClassConnectorSetting.MaxSeedNodeTimeoutConnect;
                        }

                        if (!_listOfSeedNodesSpeed.ContainsKey(seedNode.Key))
                        {
                            _listOfSeedNodesSpeed.Add(seedNode.Key, seedNodeResponseTime);
                        }

                    }
                    catch
                    {
                        if (!_listOfSeedNodesSpeed.ContainsKey(seedNode.Key))
                        {
                            _listOfSeedNodesSpeed.Add(seedNode.Key,
                                ClassConnectorSetting.MaxSeedNodeTimeoutConnect); // Max delay.
                        }
                    }

                }
            }

            return _listOfSeedNodesSpeed.ToArray().OrderBy(u => u.Value).ToDictionary(z => z.Key, y => y.Value);
        }

        /// <summary>
        /// Proceed an http request to the token network.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="requestString"></param>
        /// <returns></returns>
        private static async Task<string> ProceedHttpRequest(string url, string requestString)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest) WebRequest.Create(url + requestString);
                request.AutomaticDecompression = DecompressionMethods.GZip;
                request.ServicePoint.Expect100Continue = false;
                request.KeepAlive = false;
                request.Timeout = ClassConnectorSetting.MaxTimeoutConnect;
                request.UserAgent = ClassConnectorSetting.CoinName + "Remote Node - " +
                                    Assembly.GetExecutingAssembly().GetName().Version + "R";
                using (HttpWebResponse response = (HttpWebResponse) await request.GetResponseAsync())
                using (Stream stream = response.GetResponseStream())
                    if (stream != null)
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            return await reader.ReadToEndAsync();
                        }
            }
            catch
            {

            }

            return string.Empty;
        }
    }
}
