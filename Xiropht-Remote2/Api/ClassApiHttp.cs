using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Reflection;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xiropht_Connector_All.Setting;
using Xiropht_RemoteNode.Api.Object;
using Xiropht_RemoteNode.Data;
using Xiropht_RemoteNode.Log;
using Xiropht_RemoteNode.Object;
using Xiropht_RemoteNode.Utils;

namespace Xiropht_RemoteNode.Api
{
    public class ClassApiHttpRequestEnumeration
    {
        public const string GetCoinName = "get_coin_name";
        public const string GetCoinMinName = "get_coin_min_name";
        public const string GetCoinMaxSupply = "get_coin_max_supply";
        public const string GetCoinCirculating = "get_coin_circulating";
        public const string GetCoinTotalFee = "get_coin_total_fee";
        public const string GetCoinTotalMined = "get_coin_total_mined";
        public const string GetCoinBlockchainHeight = "get_coin_blockchain_height";
        public const string GetCoinTotalBlockMined = "get_coin_total_block_mined";
        public const string GetCoinTotalBlockLeft = "get_coin_total_block_left";
        public const string GetCoinNetworkDifficulty = "get_coin_network_difficulty";
        public const string GetCoinNetworkHashrate = "get_coin_network_hashrate";
        public const string GetCoinNetworkFullStats = "get_coin_network_full_stats";
        public const string GetCoinBlockPerId = "get_coin_block_per_id";
        public const string GetCoinBlockPerHash = "get_coin_block_per_hash";
        public const string GetCoinTransactionPerId = "get_coin_transaction_per_id";
        public const string GetCoinTransactionPerHash = "get_coin_transaction_per_hash";
        public const string PacketFavicon = "favicon.ico";
        public const string PacketNotExist = "not_exist";
        public const string PacketError = "error";
    }

    public class ClassApiHttp
    {
        public static int PersonalRemoteNodeHttpPort;
        public static bool IsBehindProxy;
        private static Thread ThreadListenApiHttpConnection;
        private static TcpListener ListenerApiHttpConnection;
        private static bool ListenApiHttpConnectionStatus;

        /// <summary>
        /// Enable http/https api of the remote node, listen incoming connection throught web client.
        /// </summary>
        public static void StartApiHttpServer()
        {
            ListenApiHttpConnectionStatus = true;
            if (PersonalRemoteNodeHttpPort <= 0) // Not selected or invalid
            {
                ListenerApiHttpConnection = new TcpListener(IPAddress.Any, ClassConnectorSetting.RemoteNodeHttpPort);
            }
            else
            {
                ListenerApiHttpConnection = new TcpListener(IPAddress.Any, PersonalRemoteNodeHttpPort);
            }
            ListenerApiHttpConnection.Start();
            ThreadListenApiHttpConnection = new Thread(async delegate ()
            {
                while (ListenApiHttpConnectionStatus && !Program.Closed)
                {
                    try
                    {
                       await ListenerApiHttpConnection.AcceptTcpClientAsync().ContinueWith(async taskClient =>
                       {
                           var client = await taskClient;
                           CancellationTokenSource cancellationTokenApiHttp = new CancellationTokenSource();
                           try
                           {
                               await Task.Factory.StartNew(async () =>
                               {
                                   using (var clientApiHttpObject = new ClassClientApiHttpObject(client, cancellationTokenApiHttp))
                                   {
                                       await clientApiHttpObject.StartHandleClientHttpAsync();
                                   }
                               }, cancellationTokenApiHttp.Token, TaskCreationOptions.RunContinuationsAsynchronously, TaskScheduler.Current).ConfigureAwait(false);
                           }
                           catch
                           {

                           }
                       });
                    }
                    catch
                    {

                    }
                }
            });
            ThreadListenApiHttpConnection.Start();
        }

        /// <summary>
        /// Stop http server
        /// </summary>
        public static void StopApiHttpServer()
        {
            ListenApiHttpConnectionStatus = false;
            if (ThreadListenApiHttpConnection != null && (ThreadListenApiHttpConnection.IsAlive || ThreadListenApiHttpConnection != null))
            {
                ThreadListenApiHttpConnection.Abort();
                GC.SuppressFinalize(ThreadListenApiHttpConnection);
            }
            ListenerApiHttpConnection.Stop();
        }
    }


    public class ClassClientApiHttpObject : IDisposable
    {
        #region Disposing Part Implementation 

        private bool _disposed;

        ~ClassClientApiHttpObject()
        {
            Dispose(false);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                }
            }

            _disposed = true;
        }

        #endregion

        private bool _clientStatus;
        private TcpClient _client;
        private string _ip;
        private SslStream _clientSslStream;
        private CancellationTokenSource CancellationTokenSourceApi;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ip"></param>
        public ClassClientApiHttpObject(TcpClient client, CancellationTokenSource cancellationTokenSourceApi)
        {
            _clientStatus = true;
            _client = client;
            CancellationTokenSourceApi = cancellationTokenSourceApi;
        }

        /// <summary>
        /// Start to listen incoming client.
        /// </summary>
        /// <returns></returns>
        public async Task StartHandleClientHttpAsync()
        {
            try
            {
                _ip = ((IPEndPoint)(_client.Client.RemoteEndPoint)).Address.ToString();
            }
            catch
            {
                CloseClientConnection();
                return;
            }
            var checkBanResult = true;

            if (_ip != "127.0.0.1") // Do not check localhost ip.
            {
                checkBanResult = ClassApiBan.FilterCheckIp(_ip);
            }
            int totalWhile = 0;
            if (checkBanResult)
            {
                if (_ip != "127.0.0.1")
                {
                    ClassApiBan.FilterInsertIp(_ip);
                }
                try
                {

                    while (_clientStatus)
                    {
                        try
                        {
                            byte[] buffer = new byte[ClassConnectorSetting.MaxNetworkPacketSize];
                            using (NetworkStream clientHttpReader = new NetworkStream(_client.Client))
                            {
                                using (var bufferedStreamNetwork = new BufferedStream(clientHttpReader, ClassConnectorSetting.MaxNetworkPacketSize))
                                {
                                    int received = await bufferedStreamNetwork.ReadAsync(buffer, 0, buffer.Length);
                                    if (received > 0)
                                    {
                                        string packet = Encoding.UTF8.GetString(buffer, 0, received);
                                        try
                                        {
                                            if (!GetAndCheckForwardedIp(packet))
                                            {
                                                break;
                                            }
                                        }
                                        catch
                                        {

                                        }

                                        packet = ClassUtilsNode.GetStringBetween(packet, "GET", "HTTP");

                                        packet = packet.Replace("/", "");
                                        packet = packet.Replace(" ", "");
                                        ClassLog.Log("HTTP API - packet received from IP: " + _ip + " - " + packet, 6, 2);
                                        await HandlePacketHttpAsync(packet);
                                        break;
                                    }
                                    else
                                    {
                                        totalWhile++;
                                    }
                                    if (totalWhile >= 8)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                        catch
                        {
                            break;
                        }
                    }

                }
                catch
                {
                }

                CloseClientConnection();
            }
        }

        /// <summary>
        /// This method permit to get back the real ip behind a proxy and check the list of banned IP.
        /// </summary>
        private bool GetAndCheckForwardedIp(string packet)
        {
            var splitPacket = packet.Split(new[] { "\n" }, StringSplitOptions.None);
            foreach (var packetEach in splitPacket)
            {
                if (packetEach != null)
                {
                    if (!string.IsNullOrEmpty(packetEach))
                    {
                        if (packetEach.ToLower().Contains("x-forwarded-for: "))
                        {
                            string newIp = packetEach.ToLower().Replace("x-forwarded-for: ", "");
                            _ip = newIp;
                            ClassLog.Log("HTTP/HTTPS API - X-Forwarded-For ip of the client is: " + newIp, 7, 2);
                            var checkBanResult = ClassApiBan.FilterCheckIp(_ip);
                            if (!checkBanResult) // Is Banned
                            {
                                return false;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(newIp))
                                {
                                    ClassApiBan.FilterInsertIp(newIp);
                                }
                                return true;
                            }
                        }

                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Close connection incoming from the client.
        /// </summary>
        private void CloseClientConnection()
        {
            try
            {
                _client?.Close();
                _client?.Dispose();
            }
            catch
            {

            }
            try
            {
                if (!CancellationTokenSourceApi.IsCancellationRequested)
                {
                    CancellationTokenSourceApi.Cancel();
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Handle get request received from client.
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        private async Task HandlePacketHttpAsync(string packet)
        {
            try
            {
                long selectedIndex = 0;
                string selectedHash = string.Empty;
                if (packet.Contains("="))
                {
                    var splitPacket = packet.Split(new[] { "=" }, StringSplitOptions.None);
                    if (!long.TryParse(splitPacket[1], out selectedIndex))
                    {
                        selectedHash = splitPacket[1]; // Hash
                    }
                    packet = splitPacket[0];
                }
                switch (packet)
                {
                    case ClassApiHttpRequestEnumeration.GetCoinName:
                        await BuildAndSendHttpPacketAsync(ClassConnectorSetting.CoinName);
                        break;
                    case ClassApiHttpRequestEnumeration.GetCoinMinName:
                        await BuildAndSendHttpPacketAsync(ClassConnectorSetting.CoinNameMin);
                        break;
                    case ClassApiHttpRequestEnumeration.GetCoinMaxSupply:
                        var jsonResultObject = new ClassApiResultObject
                        {
                            result = decimal.Parse(ClassRemoteNodeSync.CoinMaxSupply.Replace(".", ","), NumberStyles.Currency, Program.GlobalCultureInfo)
                        };

                        var resultJsonObject = JsonConvert.SerializeObject(jsonResultObject);

                        await BuildAndSendHttpPacketAsync(resultJsonObject, false, null, true);
                        //await BuildAndSendHttpPacketAsync(ClassRemoteNodeSync.CoinMaxSupply);

                        break;
                    case ClassApiHttpRequestEnumeration.GetCoinCirculating:
                        jsonResultObject = new ClassApiResultObject
                        {
                            result = decimal.Parse(ClassRemoteNodeSync.CoinCirculating.Replace(".", ","), NumberStyles.Currency, Program.GlobalCultureInfo)
                        };

                        resultJsonObject = JsonConvert.SerializeObject(jsonResultObject);

                        await BuildAndSendHttpPacketAsync(resultJsonObject, false, null, true);
                        //await BuildAndSendHttpPacketAsync(ClassRemoteNodeSync.CoinCirculating);
                        break;
                    case ClassApiHttpRequestEnumeration.GetCoinTotalFee:
                        jsonResultObject = new ClassApiResultObject
                        {
                            result = decimal.Parse(ClassRemoteNodeSync.CurrentTotalFee.Replace(".", ","), NumberStyles.Currency, Program.GlobalCultureInfo)
                        };

                        resultJsonObject = JsonConvert.SerializeObject(jsonResultObject);

                        await BuildAndSendHttpPacketAsync(resultJsonObject, false, null, true);
                        //await BuildAndSendHttpPacketAsync(ClassRemoteNodeSync.CurrentTotalFee);
                        break;
                    case ClassApiHttpRequestEnumeration.GetCoinTotalMined:
                        jsonResultObject = new ClassApiResultObject
                        {
                            result = (ClassRemoteNodeSync.ListOfBlock.Count * ClassConnectorSetting.ConstantBlockReward)
                        };

                        resultJsonObject = JsonConvert.SerializeObject(jsonResultObject);

                        await BuildAndSendHttpPacketAsync(resultJsonObject, false, null, true);
                        //await BuildAndSendHttpPacketAsync(""+(ClassRemoteNodeSync.ListOfBlock.Count * 10));
                        break;
                    case ClassApiHttpRequestEnumeration.GetCoinBlockchainHeight:
                        jsonResultObject = new ClassApiResultObject
                        {
                            result = (ClassRemoteNodeSync.ListOfBlock.Count + 1)
                        };

                        resultJsonObject = JsonConvert.SerializeObject(jsonResultObject);

                        await BuildAndSendHttpPacketAsync(resultJsonObject, false, null, true);
                        //await BuildAndSendHttpPacketAsync(""+ (ClassRemoteNodeSync.ListOfBlock.Count + 1));
                        break;
                    case ClassApiHttpRequestEnumeration.GetCoinTotalBlockMined:
                        jsonResultObject = new ClassApiResultObject
                        {
                            result = ClassRemoteNodeSync.ListOfBlock.Count
                        };

                        resultJsonObject = JsonConvert.SerializeObject(jsonResultObject);

                        await BuildAndSendHttpPacketAsync(resultJsonObject, false, null, true);
                        //await BuildAndSendHttpPacketAsync("" + (ClassRemoteNodeSync.ListOfBlock.Count));
                        break;
                    case ClassApiHttpRequestEnumeration.GetCoinTotalBlockLeft:
                        jsonResultObject = new ClassApiResultObject
                        {
                            result = long.Parse(ClassRemoteNodeSync.CurrentBlockLeft)
                        };

                        resultJsonObject = JsonConvert.SerializeObject(jsonResultObject);

                        await BuildAndSendHttpPacketAsync(resultJsonObject, false, null, true);
                        //await BuildAndSendHttpPacketAsync(ClassRemoteNodeSync.CurrentBlockLeft);

                        break;
                    case ClassApiHttpRequestEnumeration.GetCoinNetworkDifficulty:
                        jsonResultObject = new ClassApiResultObject
                        {
                            result = decimal.Parse(ClassRemoteNodeSync.CurrentDifficulty.Replace(".", ","), NumberStyles.Any, Program.GlobalCultureInfo)
                        };

                        resultJsonObject = JsonConvert.SerializeObject(jsonResultObject);

                        await BuildAndSendHttpPacketAsync(resultJsonObject, false, null, true);
                        //await BuildAndSendHttpPacketAsync(ClassRemoteNodeSync.CurrentDifficulty);
                        break;
                    case ClassApiHttpRequestEnumeration.GetCoinNetworkHashrate:
                        jsonResultObject = new ClassApiResultObject
                        {
                            result = decimal.Parse(ClassRemoteNodeSync.CurrentHashrate.Replace(".", ","), NumberStyles.Any, Program.GlobalCultureInfo)
                        };

                        resultJsonObject = JsonConvert.SerializeObject(jsonResultObject);

                        await BuildAndSendHttpPacketAsync(resultJsonObject, false, null, true);
                        //await BuildAndSendHttpPacketAsync(ClassRemoteNodeSync.CurrentHashrate);
                        break;
                    case ClassApiHttpRequestEnumeration.GetCoinBlockPerId:
                        if (selectedIndex > 0)
                        {
                            selectedIndex -= 1;
                            if (ClassRemoteNodeSync.ListOfBlock.Count - 1 >= selectedIndex)
                            {
                                if (ClassRemoteNodeSync.ListOfBlock.ContainsKey(selectedIndex))
                                {
                                    var splitBlock = ClassRemoteNodeSync.ListOfBlock[selectedIndex].Split(new[] { "#" }, StringSplitOptions.None);
                                    /*Dictionary<string, string> blockContent = new Dictionary<string, string>
                                    {
                                        { "block_id", splitBlock[0] },
                                        { "block_hash", splitBlock[1] },
                                        { "block_transaction_hash", splitBlock[2] },
                                        { "block_timestamp_create", splitBlock[3] },
                                        { "block_timestamp_found", splitBlock[4] },
                                        { "block_difficulty", splitBlock[5] },
                                        { "block_reward", splitBlock[6] }
                                    };

                                    await BuildAndSendHttpPacketAsync(null, true, blockContent);
                                    blockContent.Clear();*/
                                    var blockApiObject = new ClassApiBlockObject
                                    {
                                        block_id = long.Parse(splitBlock[0]),
                                        block_hash = splitBlock[1],
                                        block_transaction_hash = splitBlock[2],
                                        block_timestamp_create = long.Parse(splitBlock[3]),
                                        block_timestamp_found = long.Parse(splitBlock[4]),
                                        block_difficulty = decimal.Parse(splitBlock[5], NumberStyles.Any, Program.GlobalCultureInfo),
                                        block_reward = decimal.Parse(splitBlock[6].Replace(".", ","), NumberStyles.Currency, Program.GlobalCultureInfo)
                                    };
                                    var jsonBlockObject = JsonConvert.SerializeObject(blockApiObject);
                                    await BuildAndSendHttpPacketAsync(jsonBlockObject, false, null, true);
                                }
                                else
                                {
                                    ClassApiBan.FilterInsertInvalidPacket(_ip);
                                    await BuildAndSendHttpPacketAsync(ClassApiHttpRequestEnumeration.PacketNotExist);
                                }
                            }
                            else
                            {
                                ClassApiBan.FilterInsertInvalidPacket(_ip);
                                await BuildAndSendHttpPacketAsync(ClassApiHttpRequestEnumeration.PacketNotExist);
                            }
                        }
                        else
                        {
                            ClassApiBan.FilterInsertInvalidPacket(_ip);
                            await BuildAndSendHttpPacketAsync(ClassApiHttpRequestEnumeration.PacketNotExist);
                        }
                        break;
                    case ClassApiHttpRequestEnumeration.GetCoinBlockPerHash:
                        if (selectedHash != string.Empty)
                        {
                            long selectedBlockIndex = ClassRemoteNodeSync.ListOfBlockHash.GetBlockIdFromHash(selectedHash);
                            if (selectedBlockIndex != -1)
                            {

                                var splitBlock = ClassRemoteNodeSync.ListOfBlock[selectedBlockIndex].Split(new[] { "#" }, StringSplitOptions.None);
                                /*Dictionary<string, string> blockContent = new Dictionary<string, string>
                                    {
                                        { "block_id", splitBlock[0] },
                                        { "block_hash", splitBlock[1] },
                                        { "block_transaction_hash", splitBlock[2] },
                                        { "block_timestamp_create", splitBlock[3] },
                                        { "block_timestamp_found", splitBlock[4] },
                                        { "block_difficulty", splitBlock[5] },
                                        { "block_reward", splitBlock[6] }
                                    };

                                await BuildAndSendHttpPacketAsync(null, true, blockContent);
                                blockContent.Clear();*/

                                var blockApiObject = new ClassApiBlockObject
                                {
                                    block_id = long.Parse(splitBlock[0]),
                                    block_hash = splitBlock[1],
                                    block_transaction_hash = splitBlock[2],
                                    block_timestamp_create = long.Parse(splitBlock[3]),
                                    block_timestamp_found = long.Parse(splitBlock[4]),
                                    block_difficulty = decimal.Parse(splitBlock[5], NumberStyles.Any, Program.GlobalCultureInfo),
                                    block_reward = decimal.Parse(splitBlock[6].Replace(".", ","), NumberStyles.Currency, Program.GlobalCultureInfo)
                                };
                                var jsonBlockObject = JsonConvert.SerializeObject(blockApiObject);
                                await BuildAndSendHttpPacketAsync(jsonBlockObject, false, null, true);

                            }
                            else
                            {
                                ClassApiBan.FilterInsertInvalidPacket(_ip);
                                await BuildAndSendHttpPacketAsync(ClassApiHttpRequestEnumeration.PacketNotExist);
                            }
                        }
                        else
                        {
                            ClassApiBan.FilterInsertInvalidPacket(_ip);
                            await BuildAndSendHttpPacketAsync(ClassApiHttpRequestEnumeration.PacketNotExist);
                        }
                        break;
                    case ClassApiHttpRequestEnumeration.GetCoinTransactionPerId:
                        if (selectedIndex > 0)
                        {
                            selectedIndex -= 1;
                            if (ClassRemoteNodeSync.ListOfTransaction.Count - 1 >= selectedIndex)
                            {
                                if (ClassRemoteNodeSync.ListOfTransaction.ContainsKey(selectedIndex))
                                {
                                    var splitTransaction = ClassRemoteNodeSync.ListOfTransaction.GetTransaction(selectedIndex).Item1.Split(new[] { "-" }, StringSplitOptions.None);
                                    /*Dictionary<string, string> transactionContent = new Dictionary<string, string>
                                    {
                                        { "transaction_id", "" + (selectedIndex + 1) },
                                        { "transaction_id_sender", splitTransaction[0] },
                                        { "transaction_fake_amount", splitTransaction[1] },
                                        { "transaction_fake_fee", splitTransaction[2] },
                                        { "transaction_id_receiver", splitTransaction[3] },
                                        { "transaction_timestamp_sended", splitTransaction[4] },
                                        { "transaction_hash", splitTransaction[5] },
                                        { "transaction_timestamp_received", splitTransaction[6] }
                                    };

                                    await BuildAndSendHttpPacketAsync(null, true, transactionContent);*/

                                    var transactionApiObject = new ClassApiTransactionObject
                                    {
                                        transaction_id = (selectedIndex + 1),
                                        transaction_id_sender = splitTransaction[0],
                                        transaction_fake_amount = decimal.Parse(splitTransaction[1].Replace(".", ","), NumberStyles.Currency, Program.GlobalCultureInfo),
                                        transaction_fake_fee = decimal.Parse(splitTransaction[2].Replace(".", ","), NumberStyles.Currency, Program.GlobalCultureInfo),
                                        transaction_id_receiver = splitTransaction[3],
                                        transaction_timemstamp_sended = long.Parse(splitTransaction[4]),
                                        transaction_hash = splitTransaction[5],
                                        transaction_timestamp_received = long.Parse(splitTransaction[6])
                                    };
                                    var jsonTransactionObject = JsonConvert.SerializeObject(transactionApiObject);
                                    await BuildAndSendHttpPacketAsync(jsonTransactionObject, false, null, true);

                                }
                                else
                                {
                                    ClassApiBan.FilterInsertInvalidPacket(_ip);
                                    await BuildAndSendHttpPacketAsync(ClassApiHttpRequestEnumeration.PacketNotExist);
                                }
                            }
                            else
                            {
                                ClassApiBan.FilterInsertInvalidPacket(_ip);
                                await BuildAndSendHttpPacketAsync(ClassApiHttpRequestEnumeration.PacketNotExist);
                            }
                        }
                        else
                        {
                            ClassApiBan.FilterInsertInvalidPacket(_ip);
                            await BuildAndSendHttpPacketAsync(ClassApiHttpRequestEnumeration.PacketNotExist);
                        }
                        break;
                    case ClassApiHttpRequestEnumeration.GetCoinTransactionPerHash:
                        if (selectedHash != string.Empty)
                        {
                            long transactionIndex = ClassRemoteNodeSync.ListOfTransactionHash.ContainsKey(selectedHash);
                            if (transactionIndex != -1)
                            {
                                var splitTransaction = ClassRemoteNodeSync.ListOfTransaction.GetTransaction(transactionIndex).Item1.Split(new[] { "-" }, StringSplitOptions.None);
                                /*Dictionary<string, string> transactionContent = new Dictionary<string, string>
                                    {
                                        { "transaction_id", "" + (transactionIndex + 1) },
                                        { "transaction_id_sender", splitTransaction[0] },
                                        { "transaction_fake_amount", splitTransaction[1] },
                                        { "transaction_fake_fee", splitTransaction[2] },
                                        { "transaction_id_receiver", splitTransaction[3] },
                                        { "transaction_timestamp_sended", splitTransaction[4] },
                                        { "transaction_hash", splitTransaction[5] },
                                        { "transaction_timestamp_received", splitTransaction[6] }
                                    };

                                await BuildAndSendHttpPacketAsync(null, true, transactionContent);
                                transactionContent.Clear();*/

                                var transactionApiObject = new ClassApiTransactionObject
                                {
                                    transaction_id = (transactionIndex + 1),
                                    transaction_id_sender = splitTransaction[0],
                                    transaction_fake_amount = decimal.Parse(splitTransaction[1].Replace(".", ","), NumberStyles.Currency, Program.GlobalCultureInfo),
                                    transaction_fake_fee = decimal.Parse(splitTransaction[2].Replace(".", ","), NumberStyles.Currency, Program.GlobalCultureInfo),
                                    transaction_id_receiver = splitTransaction[3],
                                    transaction_timemstamp_sended = long.Parse(splitTransaction[4]),
                                    transaction_hash = splitTransaction[5],
                                    transaction_timestamp_received = long.Parse(splitTransaction[6])
                                };
                                var jsonTransactionObject = JsonConvert.SerializeObject(transactionApiObject);
                                await BuildAndSendHttpPacketAsync(jsonTransactionObject, false, null, true);
                            }
                            else
                            {
                                ClassApiBan.FilterInsertInvalidPacket(_ip);
                                await BuildAndSendHttpPacketAsync(ClassApiHttpRequestEnumeration.PacketNotExist);
                            }
                        }
                        else
                        {
                            ClassApiBan.FilterInsertInvalidPacket(_ip);
                            await BuildAndSendHttpPacketAsync(ClassApiHttpRequestEnumeration.PacketNotExist);
                        }
                        break;
                    case ClassApiHttpRequestEnumeration.GetCoinNetworkFullStats:
                        /*Dictionary<string, string> networkStatsContent = new Dictionary<string, string>
                        {
                            { "coin_name", ClassConnectorSetting.CoinName },
                            { "coin_min_name", ClassConnectorSetting.CoinNameMin },
                            { "coin_max_supply",  decimal.Parse(ClassRemoteNodeSync.CoinMaxSupply, NumberStyles.Any, Program.GlobalCultureInfo).ToString()  },
                            { "coin_circulating", decimal.Parse(ClassRemoteNodeSync.CoinCirculating, NumberStyles.Any, Program.GlobalCultureInfo).ToString() },
                            { "coin_total_fee",  decimal.Parse(ClassRemoteNodeSync.CurrentTotalFee, NumberStyles.Any, Program.GlobalCultureInfo).ToString()},
                            { "coin_total_mined", (ClassRemoteNodeSync.ListOfBlock.Count *ClassConnectorSetting.ConstantBlockReward).ToString() },
                            { "coin_blockchain_height", "" + (ClassRemoteNodeSync.ListOfBlock.Count + 1) },
                            { "coin_total_block_mined", "" + ClassRemoteNodeSync.ListOfBlock.Count },
                            { "coin_total_block_left", ClassRemoteNodeSync.CurrentBlockLeft },
                            { "coin_network_difficulty", decimal.Parse(ClassRemoteNodeSync.CurrentDifficulty, NumberStyles.Any, Program.GlobalCultureInfo).ToString() },
                            { "coin_network_hashrate", decimal.Parse(ClassRemoteNodeSync.CurrentHashrate, NumberStyles.Any, Program.GlobalCultureInfo).ToString() },
                            { "coin_total_transaction", "" + ClassRemoteNodeSync.ListOfTransaction.Count }
                        };

                        await BuildAndSendHttpPacketAsync(null, true, networkStatsContent);
                        networkStatsContent.Clear();*/

                        ClassApiNetworkStatsObject networkStatsApiObject = new ClassApiNetworkStatsObject
                        {
                            coin_name = ClassConnectorSetting.CoinName,
                            coin_min_name = ClassConnectorSetting.CoinNameMin,
                            coin_max_supply = decimal.Parse(ClassRemoteNodeSync.CoinMaxSupply.Replace(".", ","), NumberStyles.Currency, Program.GlobalCultureInfo),
                            coin_circulating = decimal.Parse(ClassRemoteNodeSync.CoinCirculating.Replace(".", ","), NumberStyles.Currency, Program.GlobalCultureInfo),
                            coin_total_fee = decimal.Parse(ClassRemoteNodeSync.CurrentTotalFee.Replace(".", ","), NumberStyles.Currency, Program.GlobalCultureInfo),
                            coin_total_mined = (ClassRemoteNodeSync.ListOfBlock.Count * ClassConnectorSetting.ConstantBlockReward),
                            coin_total_block_mined = ClassRemoteNodeSync.ListOfBlock.Count,
                            coin_blockchain_height = (ClassRemoteNodeSync.ListOfBlock.Count + 1),
                            coin_total_block_left = long.Parse(ClassRemoteNodeSync.CurrentBlockLeft),
                            coin_network_difficulty = decimal.Parse(ClassRemoteNodeSync.CurrentDifficulty.Replace(".", ","), NumberStyles.Any, Program.GlobalCultureInfo),
                            coin_network_hashrate = decimal.Parse(ClassRemoteNodeSync.CurrentHashrate.Replace(".", ","), NumberStyles.Any, Program.GlobalCultureInfo),
                            coin_total_transaction = ClassRemoteNodeSync.ListOfTransaction.Count
                        };

                        var jsonNetworkStatsObject = JsonConvert.SerializeObject(networkStatsApiObject);
                        await BuildAndSendHttpPacketAsync(jsonNetworkStatsObject, false, null, true);

                        break;
                    case ClassApiHttpRequestEnumeration.PacketFavicon:
                        ClassLog.Log("HTTP API - packet received from IP: " + _ip + " - favicon request detected and ignored.", 6, 2);
                        await BuildAndSendHttpPacketAsync(ClassApiHttpRequestEnumeration.PacketNotExist);
                        break;
                    default:
                        ClassApiBan.FilterInsertInvalidPacket(_ip);
                        await BuildAndSendHttpPacketAsync(ClassApiHttpRequestEnumeration.PacketNotExist);
                        break;
                }
            }
            catch(Exception error)
            {
#if DEBUG
                Console.WriteLine("API HTTP - HandlePacketHttpAsync Exception: " + error.Message);
#endif
                await BuildAndSendHttpPacketAsync(ClassApiHttpRequestEnumeration.PacketError);

            }
        }

        /// <summary>
        /// build and send http packet to client.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private async Task BuildAndSendHttpPacketAsync(string content, bool multiResult = false, Dictionary<string, string> dictionaryContent = null, bool jsonDone = false)
        {
            string contentToSend = string.Empty;
            if (!jsonDone)
            {
                if (!multiResult)
                {
                    contentToSend = BuildJsonString(content);
                }
                else
                {
                    contentToSend = BuildFullJsonString(dictionaryContent);
                }
            }
            else
            {
                contentToSend = content;
            }
            StringBuilder builder = new StringBuilder();

            builder.AppendLine(@"HTTP/1.1 200 OK");
            builder.AppendLine(@"Content-Type: text/plain");
            builder.AppendLine(@"Content-Length: " + contentToSend.Length);
            builder.AppendLine(@"Access-Control-Allow-Origin: *");
            builder.AppendLine(@"");
            builder.AppendLine(@"" + contentToSend);
            await SendPacketAsync(builder.ToString());
            builder.Clear();
            contentToSend = string.Empty;
        }

        /// <summary>
        /// Return content converted for json.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string BuildJsonString(string content)
        {
            JObject jsonContent = new JObject
            {
                { "result", content },
                { "version", Assembly.GetExecutingAssembly().GetName().Version.ToString() }
            };
            return JsonConvert.SerializeObject(jsonContent);
        }

        /// <summary>
        /// Return content converted for json.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string BuildFullJsonString(Dictionary<string, string> dictionaryContent)
        {
            JObject jsonContent = new JObject();
            foreach (var content in dictionaryContent)
            {
                jsonContent.Add(content.Key, content.Value);
            }
            jsonContent.Add("version", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            return JsonConvert.SerializeObject(jsonContent);
        }

        /// <summary>
        /// Send packet to client.
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        private async Task SendPacketAsync(string packet)
        {
            try
            {
                using (var networkStream = new NetworkStream(_client.Client))
                {
                    using (var bufferedStreamNetwork = new BufferedStream(networkStream, ClassConnectorSetting.MaxNetworkPacketSize))
                    {
                        var bytePacket = Encoding.UTF8.GetBytes(packet);

                        await bufferedStreamNetwork.WriteAsync(bytePacket, 0, bytePacket.Length).ConfigureAwait(false);
                        await bufferedStreamNetwork.FlushAsync().ConfigureAwait(false);
                    }
                }
            }
            catch
            {
            }
        }

       
    }

}
