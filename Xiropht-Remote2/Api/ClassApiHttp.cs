using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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
using Xiropht_RemoteNode.Data;
using Xiropht_RemoteNode.Filter;
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
        public const string GetCoinTransactionPerId = "get_coin_transaction_per_id";
        public const string PacketFavicon = "favicon.ico";
        public const string PacketNotExist = "not_exist";
    }

    public class ClassApiHttp
    {
        public static int PersonalRemoteNodeHttpPort;
        public static bool UseSSL;
        public static bool IsBehindProxy;
        public static string SSLPath;
        public static X509Certificate ApiCertificateSSL;
        private static Thread ThreadListenApiHttpConnection;
        private static TcpListener ListenerApiHttpConnection;
        private static bool ListenApiHttpConnectionStatus;

        /// <summary>
        /// Enable http/https api of the remote node, listen incoming connection throught web client.
        /// </summary>
        public static void StartApiHttpServer()
        {
            if (UseSSL)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                try
                {
                    ApiCertificateSSL = X509Certificate.CreateFromCertFile(SSLPath);


                }
                catch(Exception error)
                {
                    Console.WriteLine("Cannot open certificate: " + SSLPath + " exception error: " + error.Message);
                }
            }
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
                        var client = await ListenerApiHttpConnection.AcceptTcpClientAsync();
                        var ip = ((IPEndPoint)(client.Client.RemoteEndPoint)).Address.ToString();

                        await Task.Factory.StartNew(async () =>
                        {
                            using (var clientApiHttpObject = new ClassClientApiHttpObject(client, ip))
                            {
                                await clientApiHttpObject.StartHandleClientHttpAsync().ConfigureAwait(false);
                            }
                        }, CancellationToken.None, TaskCreationOptions.RunContinuationsAsynchronously, PriorityScheduler.Lowest).ConfigureAwait(false);
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="client"></param>
        /// <param name="ip"></param>
        public ClassClientApiHttpObject(TcpClient client, string ip)
        {
            _clientStatus = true;
            _client = client;
            _ip = ip;
        }

        /// <summary>
        /// Start to listen incoming client.
        /// </summary>
        /// <returns></returns>
        public async Task StartHandleClientHttpAsync()
        {
            await Task.Delay(1);
            var checkBanResult = false;

            if (_ip != "127.0.0.1") // Do not check localhost ip.
            {
                checkBanResult = ClassApiBan.CheckBanIp(_ip);
            }
            int totalWhile = 0;
            if (!checkBanResult)
            {
                try
                {
                    if (!ClassApiHttp.UseSSL)
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
                            catch (Exception error)
                            {
                                Console.WriteLine("HTTP API - exception error: " + error.Message);
                                break;
                            }
                        }
                    }
                    else
                    {
                        _clientSslStream = new SslStream(_client.GetStream(), false);
                        _clientSslStream.AuthenticateAsServer(ClassApiHttp.ApiCertificateSSL, false, SslProtocols.Tls12, true);
                        ClassLog.Log("HTTPS API -  SSL Authentification succeed for client IP: "+_ip, 6, 2);

                        while (_clientStatus)
                        {
                            try
                            {
                                byte[] buffer = new byte[8192];
                                int received = 0;

                                while ((received = await _clientSslStream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                                {
                                    if (received > 0)
                                    {
                                        string packet = Encoding.UTF8.GetString(buffer);
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
                                        ClassLog.Log("HTTPS API - packet received from IP: " + _ip + " - " + packet, 6, 2);
                                        await HandlePacketHttpAsync(packet);
                                        break;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                            catch (Exception error)
                            {
                                Console.WriteLine("HTTPS API - exception error: " + error.Message);
                                break;
                            }
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
                            _ip = packetEach.ToLower().Replace("x-forwarded-for: ", "");
                            ClassLog.Log("HTTP/HTTPS API - X-Forwarded-For ip of the client is: " + _ip, 7, 2);
                            var checkBanResult = ClassApiBan.CheckBanIp(_ip);
                            if (checkBanResult) // Is Banned
                            {
                                return false;
                            }
                            else
                            {
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
            _client?.Close();
            _client?.Dispose();
        }

        /// <summary>
        /// Handle get request received from client.
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        private async Task HandlePacketHttpAsync(string packet)
        {
            long selectedIndex = 0;
            if (packet.Contains("="))
            {
                var splitPacket = packet.Split(new[] { "=" }, StringSplitOptions.None);
                long.TryParse(splitPacket[1], out selectedIndex);
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
                    await BuildAndSendHttpPacketAsync(ClassRemoteNodeSync.CoinMaxSupply);
                    break;
                case ClassApiHttpRequestEnumeration.GetCoinCirculating:
                    await BuildAndSendHttpPacketAsync(ClassRemoteNodeSync.CoinCirculating);
                    break;
                case ClassApiHttpRequestEnumeration.GetCoinTotalFee:
                    await BuildAndSendHttpPacketAsync(ClassRemoteNodeSync.CurrentTotalFee);
                    break;
                case ClassApiHttpRequestEnumeration.GetCoinTotalMined:
                    await BuildAndSendHttpPacketAsync(""+(ClassRemoteNodeSync.ListOfBlock.Count * 10));
                    break;
                case ClassApiHttpRequestEnumeration.GetCoinBlockchainHeight:
                    await BuildAndSendHttpPacketAsync(""+ (ClassRemoteNodeSync.ListOfBlock.Count + 1));
                    break;
                case ClassApiHttpRequestEnumeration.GetCoinTotalBlockMined:
                    await BuildAndSendHttpPacketAsync("" + (ClassRemoteNodeSync.ListOfBlock.Count));
                    break;
                case ClassApiHttpRequestEnumeration.GetCoinTotalBlockLeft:
                    await BuildAndSendHttpPacketAsync(ClassRemoteNodeSync.CurrentBlockLeft);
                    break;
                case ClassApiHttpRequestEnumeration.GetCoinNetworkDifficulty:
                    await BuildAndSendHttpPacketAsync(ClassRemoteNodeSync.CurrentDifficulty);
                    break;
                case ClassApiHttpRequestEnumeration.GetCoinNetworkHashrate:
                    await BuildAndSendHttpPacketAsync(ClassRemoteNodeSync.CurrentHashrate);
                    break;
                case ClassApiHttpRequestEnumeration.GetCoinBlockPerId:
                    if (selectedIndex > 0)
                    {
                        selectedIndex -= 1;
                        if (ClassRemoteNodeSync.ListOfBlock.Count-1 >= selectedIndex)
                        {
                            if (ClassRemoteNodeSync.ListOfBlock.ContainsKey((int)selectedIndex))
                            {
                                var splitBlock = ClassRemoteNodeSync.ListOfBlock[(int)selectedIndex].Split(new[] { "#" }, StringSplitOptions.None);
                                Dictionary<string, string> blockContent = new Dictionary<string, string>
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
                                blockContent.Clear();
                            }
                            else
                            {
                                ClassApiBan.InsertInvalidPacket(_ip);
                                await BuildAndSendHttpPacketAsync(ClassApiHttpRequestEnumeration.PacketNotExist);
                            }
                        }
                        else
                        {
                            ClassApiBan.InsertInvalidPacket(_ip);
                            await BuildAndSendHttpPacketAsync(ClassApiHttpRequestEnumeration.PacketNotExist);
                        }
                    }
                    else
                    {
                        ClassApiBan.InsertInvalidPacket(_ip);
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
                                var splitTransaction = ClassRemoteNodeSync.ListOfTransaction.GetTransaction(selectedIndex).Split(new[] { "-" }, StringSplitOptions.None);
                                Dictionary<string, string> transactionContent = new Dictionary<string, string>
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

                                await BuildAndSendHttpPacketAsync(null, true, transactionContent);
                                transactionContent.Clear();
                            }
                            else
                            {
                                ClassApiBan.InsertInvalidPacket(_ip);
                                await BuildAndSendHttpPacketAsync(ClassApiHttpRequestEnumeration.PacketNotExist);
                            }
                        }
                        else
                        {
                            ClassApiBan.InsertInvalidPacket(_ip);
                            await BuildAndSendHttpPacketAsync(ClassApiHttpRequestEnumeration.PacketNotExist);
                        }
                    }
                    else
                    {
                        ClassApiBan.InsertInvalidPacket(_ip);
                        await BuildAndSendHttpPacketAsync(ClassApiHttpRequestEnumeration.PacketNotExist);
                    }
                    break;
                case ClassApiHttpRequestEnumeration.GetCoinNetworkFullStats:
                    Dictionary<string, string> networkStatsContent = new Dictionary<string, string>
                    {
                        { "coin_name", ClassConnectorSetting.CoinName },
                        { "coin_min_name", ClassConnectorSetting.CoinNameMin },
                        { "coin_max_supply", ClassRemoteNodeSync.CoinMaxSupply },
                        { "coin_circulating", ClassRemoteNodeSync.CoinCirculating },
                        { "coin_total_fee", ClassRemoteNodeSync.CurrentTotalFee },
                        { "coin_total_mined", "" + (ClassRemoteNodeSync.ListOfBlock.Count * 10) },
                        { "coin_blockchain_height", "" + (ClassRemoteNodeSync.ListOfBlock.Count + 1) },
                        { "coin_total_block_mined", "" + ClassRemoteNodeSync.ListOfBlock.Count },
                        { "coin_total_block_left", ClassRemoteNodeSync.CurrentBlockLeft },
                        { "coin_network_difficulty", ClassRemoteNodeSync.CurrentDifficulty },
                        { "coin_network_hashrate", ClassRemoteNodeSync.CurrentHashrate },
                        { "coin_total_transaction", "" + ClassRemoteNodeSync.ListOfTransaction.Count }
                    };

                    await BuildAndSendHttpPacketAsync(null, true, networkStatsContent);
                    networkStatsContent.Clear();

                    break;
                case ClassApiHttpRequestEnumeration.PacketFavicon:
                    ClassLog.Log("HTTP API - packet received from IP: " + _ip + " - favicon request detected and ignored.", 6, 2);
                    await BuildAndSendHttpPacketAsync(ClassApiHttpRequestEnumeration.PacketNotExist);
                    break;
                default:
                    ClassApiBan.InsertInvalidPacket(_ip);
                    await BuildAndSendHttpPacketAsync(ClassApiHttpRequestEnumeration.PacketNotExist);
                    break;
            }
        }

        /// <summary>
        /// build and send http packet to client.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private async Task BuildAndSendHttpPacketAsync(string content, bool multiResult = false, Dictionary<string, string> dictionaryContent = null)
        {
            string contentToSend = string.Empty;
            if (!multiResult)
            {
                contentToSend = BuildJsonString(content);
            }
            else
            {
               contentToSend = BuildFullJsonString(dictionaryContent);
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
                var bytePacket = Encoding.UTF8.GetBytes(packet);
                if (!ClassApiHttp.UseSSL)
                {

                    using (var networkStream = new NetworkStream(_client.Client))
                    {
                        using (var bufferedStreamNetwork = new BufferedStream(networkStream, ClassConnectorSetting.MaxNetworkPacketSize))
                        {
                            await bufferedStreamNetwork.WriteAsync(bytePacket, 0, bytePacket.Length).ConfigureAwait(false);
                            await bufferedStreamNetwork.FlushAsync().ConfigureAwait(false);
                        }
                    }
                }
                else
                {
                    await _clientSslStream.WriteAsync(bytePacket, 0, bytePacket.Length).ConfigureAwait(false);
                    await _clientSslStream.FlushAsync().ConfigureAwait(false);
                }
            }
            catch
            {
            }
        }

       
    }

}
