using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xiropht_Connector_All.Remote;
using Xiropht_Connector_All.Setting;
using Xiropht_RemoteNode.Data;
using Xiropht_RemoteNode.Log;
using Xiropht_RemoteNode.Utils;

namespace Xiropht_RemoteNode.Api
{
    public class ClassApiBanObject
    {
        public string Ip;
        public long BanDate;
        public int TotalInvalidPacket;
        public bool Banned;

        public ClassApiBanObject(string ipTmp)
        {
            Ip = ipTmp;
            BanDate = 0;
        }
    }

    public class ClassApiBan
    {
        public const int MaxPacketPerSecond = 30;
        public const int MaxInvalidPacket = 10;
        public const int BanTimeInSecond = 60;
        public static Dictionary<string, ClassApiBanObject> ListBanApiIp = new Dictionary<string, ClassApiBanObject>();
    }

    /// <summary>
    /// Done for receive command from wallet or other systems and send a reply of the information asked.
    /// </summary>
    public class ClassApi
    {
        private static Thread _threadApiReceiveConnection;
        private static TcpListener _tcpListenerApiReceiveConnection;
        public static bool ApiReceiveConnectionStatus;

        /// <summary>
        /// Start to listen incoming connection to API.
        /// </summary>
        public static void StartApiRemoteNode()
        {
            ApiReceiveConnectionStatus = true;
            _tcpListenerApiReceiveConnection = new TcpListener(IPAddress.Any, ClassConnectorSetting.RemoteNodePort);
            _tcpListenerApiReceiveConnection.Start();



            // Async
            _threadApiReceiveConnection = new Thread(async delegate ()
            {
                await MainAsync();
            });
            _threadApiReceiveConnection.Start();


        }

        /// <summary>
        /// Stop api.
        /// </summary>
        public static void StopApi()
        {
            ApiReceiveConnectionStatus = false;
            if (_threadApiReceiveConnection != null && (_threadApiReceiveConnection.IsAlive || _threadApiReceiveConnection != null))
            {
                _threadApiReceiveConnection.Abort();
            }
            _tcpListenerApiReceiveConnection.Stop();
        }

        private static async Task MainAsync()
        {
            while (ApiReceiveConnectionStatus)
            {
                try
                {

                    var client = await _tcpListenerApiReceiveConnection.AcceptTcpClientAsync().ConfigureAwait(false);
                    string ip = ((IPEndPoint)(client.Client.RemoteEndPoint)).Address.ToString();



                    ClassLog.Log("API Receive incoming connection from IP: " + ip, 5, 2);

                    await Task.Factory.StartNew(new ClassApiObjectConnection(client, ip).StartHandleIncomingConnectionAsync, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).ConfigureAwait(false);


                }
                catch
                {
                }
            }
            ApiReceiveConnectionStatus = false;
        }

        /// <summary>
        /// Check if ip is banned, return ban object id if the ip is not banned.
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool CheckBanIp(string ip)
        {
            try
            {
                if (ClassApiBan.ListBanApiIp.ContainsKey(ip))
                {
                    if (ClassApiBan.ListBanApiIp[ip].BanDate > DateTimeOffset.Now.ToUnixTimeSeconds())
                    {
                        return true;
                    }
                    else
                    {
                        if (ClassApiBan.ListBanApiIp[ip].Banned)
                        {
                            ClassApiBan.ListBanApiIp[ip].Banned = false;
                            ClassApiBan.ListBanApiIp[ip].TotalInvalidPacket = 0;
                        }
                        return false;
                    }
                }
                else
                {
                    ClassApiBan.ListBanApiIp.Add(ip, new ClassApiBanObject(ip));
                }
            }
            catch
            {

            }
            return false;
        }

    }


    public class ApiObjectConnectionSendPacket : IDisposable
    {
        public byte[] packetByte;
        private bool disposed;

        public ApiObjectConnectionSendPacket(string packet)
        {
            packetByte = Encoding.UTF8.GetBytes(packet);
        }

        ~ApiObjectConnectionSendPacket()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                packetByte = null;
            }

            disposed = true;
        }
    }
    public class ApiObjectConnectionPacket : IDisposable
    {
        public char[] buffer;
        public string packet;
        private bool disposed;

        public ApiObjectConnectionPacket()
        {
            buffer = new char[8192];
            packet = string.Empty;
        }

        ~ApiObjectConnectionPacket()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                buffer = null;
                packet = null;
            }

            disposed = true;
        }

    }

    /// <summary>
    /// Object allowed for incoming connection.
    /// </summary>
    public class ClassApiObjectConnection
    {
        private bool _incomingConnectionStatus;

        private TcpClient _client;
        private string _ip;
        private NetworkStream _clientApiNetworkStream;
        private StreamReader _clientApiStreamReader;
        private int _totalPacketPerSecond;
        private bool disposed;
        private long _lastPacketReceived;

        public ClassApiObjectConnection(TcpClient clientTmp, string ipTmp)
        {
            _client = clientTmp;
            _ip = ipTmp;
        }

        ~ClassApiObjectConnection()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                _client = null;
                _clientApiNetworkStream = null;
                _clientApiStreamReader = null;

            }

            _incomingConnectionStatus = false;


            disposed = true;
        }

        /// <summary>
        /// Check packet speed of the connection opened.
        /// </summary>
        private async void CheckPacketSpeedAsync()
        {
            while (_incomingConnectionStatus)
            {
                try
                {

                    if (_totalPacketPerSecond >= ClassApiBan.MaxPacketPerSecond)
                    {
                        ClassApiBan.ListBanApiIp[_ip].BanDate = DateTimeOffset.Now.ToUnixTimeSeconds() + ClassApiBan.BanTimeInSecond;
                        ClassApiBan.ListBanApiIp[_ip].Banned = true;
                        _incomingConnectionStatus = false;
                        break;
                    }
                    ClassLog.Log("API - Total packets received from IP: " + _ip + " is: " + _totalPacketPerSecond, 5, 2);
                    _totalPacketPerSecond = 0;
                    await Task.Delay(1000);
                }
                catch
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Check the status of the connection opened.
        /// </summary>
        private async void CheckConnection()
        {

            while (_incomingConnectionStatus)
            {

                try
                {
                    if (!_incomingConnectionStatus)
                    {
                        _incomingConnectionStatus = false;
                        break;
                    }
                    if (!Utils.ClassUtilsNode.SocketIsConnected(_client))
                    {
                        _incomingConnectionStatus = false;
                        break;
                    }
                }
                catch
                {
                    _incomingConnectionStatus = false;
                    break;
                }
                await Task.Delay(1000);
            }
            StopClientApiConnection();
            Dispose();
        }

        public async Task StartHandleIncomingConnectionAsync()
        {

            var checkBanResult = ClassApi.CheckBanIp(_ip);

            if (checkBanResult)
            {
                _client?.Close();
                _client?.Dispose();
            }
            else
            {
                _lastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                await HandleIncomingConnectionAsync();
            }

        }

        /// <summary>
        /// Handle incoming connection to the api and listen packet received.
        /// </summary>
        public async Task HandleIncomingConnectionAsync()
        {
            _incomingConnectionStatus = true;
            _clientApiNetworkStream = new NetworkStream(_client.Client);
            _clientApiStreamReader = new StreamReader(_clientApiNetworkStream);
            await Task.Factory.StartNew(CheckConnection, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).ConfigureAwait(false);
            await Task.Factory.StartNew(CheckPacketSpeedAsync, CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default).ConfigureAwait(false);

            try
            {
                while (_incomingConnectionStatus)
                {
                    if (!ClassApi.ApiReceiveConnectionStatus)
                    {
                        _incomingConnectionStatus = false;
                        break;
                    }
                    if (!Utils.ClassUtilsNode.SocketIsConnected(_client))
                    {
                        _incomingConnectionStatus = false;
                        break;
                    }
                    if (!_incomingConnectionStatus)
                    {
                        break;
                    }
                    try
                    {

                        try
                        {
                            using (var bufferPacket = new ApiObjectConnectionPacket())
                            {
                                int received = await _clientApiStreamReader.ReadAsync(bufferPacket.buffer, 0, bufferPacket.buffer.Length);

                                if (received > 0)
                                {
                                    bufferPacket.packet = new string(bufferPacket.buffer, 0, received);


                                    _totalPacketPerSecond++;
                                    _lastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();


                                    if (bufferPacket.packet.Contains("*"))
                                    {
                                        var splitPacket = bufferPacket.packet.Split(new[] { "*" }, StringSplitOptions.None);
                                        if (splitPacket.Length > 1)
                                        {
                                            foreach (var packetMerged in splitPacket)
                                            {
                                                if (_incomingConnectionStatus)
                                                {
                                                    if (packetMerged != null)
                                                    {
                                                        if (!string.IsNullOrEmpty(packetMerged))
                                                        {
                                                            if (packetMerged.Length > 1)
                                                            {

                                                                var packetReplace = packetMerged.Replace("*", "");
                                                                ClassLog.Log("API - Packet received from IP: " + _ip + " is: " + packetReplace, 5, 2);

                                                                await Task.Run(async delegate
                                                                {
                                                                    if (_incomingConnectionStatus)
                                                                    {
                                                                        if (!await HandleIncomingPacketAsync(packetReplace))
                                                                        {
                                                                            ClassLog.Log("API - Cannot send packet to IP: " + _ip + "", 5, 2);
                                                                            _incomingConnectionStatus = false;
                                                                        }
                                                                    }
                                                                });
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (_incomingConnectionStatus)
                                            {
                                                var packetReplace = bufferPacket.packet.Replace("*", "");
                                                ClassLog.Log("API - Packet received from IP: " + _ip + " is: " + packetReplace, 5, 2);

                                                await Task.Run(async delegate
                                                {
                                                    if (!await HandleIncomingPacketAsync(packetReplace))
                                                    {
                                                        ClassLog.Log("API - Cannot send packet to IP: " + _ip + "", 5, 2);
                                                        _incomingConnectionStatus = false;
                                                    }
                                                });
                                            }
                                        }
                                    }
                                    else
                                    {

                                        ClassLog.Log("API - Packet received from IP: " + _ip + " is: " + bufferPacket.packet, 5, 2);

                                        if (_incomingConnectionStatus)
                                        {

                                            if (!await HandleIncomingPacketAsync(bufferPacket.packet))
                                            {
                                                ClassLog.Log("API - Cannot send packet to IP: " + _ip + "", 5, 2);
                                                _incomingConnectionStatus = false;
                                            }
                                        }
                                    }
                                }
                            }


                            if (!_incomingConnectionStatus)
                            {
                                break;
                            }
                        }
                        catch (Exception error)
                        {
                            _incomingConnectionStatus = false;
                            ClassLog.Log("API Incoming connection from IP: " + _ip + " closed by exception: " + error.Message, 5, 2);
                            break;
                        }
                    }
                    catch (Exception error)
                    {
                        _incomingConnectionStatus = false;
                        ClassLog.Log("API Incoming connection from IP: " + _ip + " closed by exception: " + error.Message, 5, 2);
                        break;
                    }
                }
            }
            catch (Exception error)
            {
                _incomingConnectionStatus = false;
                ClassLog.Log("API Incoming connection from IP: " + _ip + " closed by exception: " + error.Message, 5, 2);
            }

            StopClientApiConnection();
        }

        private void StopClientApiConnection()
        {
            _clientApiNetworkStream?.Close();
            _clientApiNetworkStream?.Dispose();
            _clientApiStreamReader?.Close();
            _clientApiStreamReader?.Dispose();
            _client?.Close();
            _client?.Dispose();
        }

        /// <summary>
        /// Handle incoming packet and send the information asked.
        /// </summary>
        /// <param name="packet"></param>
        private async Task<bool> HandleIncomingPacketAsync(string packet)
        {
            if (!_incomingConnectionStatus)
            {
                return false;
            }
            if (_incomingConnectionStatus)
            {
                try
                {
                    var splitPacket = packet.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                    if (!string.IsNullOrEmpty(splitPacket[0]))
                    {
                        if (float.TryParse(splitPacket[1], NumberStyles.Any, Program.GlobalCultureInfo, out var walletId))
                        {
                            if (walletId < 0)
                            {
                                if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.WalletIdWrong).ConfigureAwait(false)) // Wrong Wallet ID
                                {
                                    _incomingConnectionStatus = false;
                                    return false;
                                }
                            }
                            else
                            {
                                switch (splitPacket[0])
                                {
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.WalletAskHisNumberTransaction:
                                        if (ClassRemoteNodeSync.ListTransactionPerWallet.ContainsKey(walletId))
                                        {
                                            if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.WalletYourNumberTransaction + "|" + ClassRemoteNodeSync.ListTransactionPerWallet[walletId].Count).ConfigureAwait(false))
                                            {
                                                _incomingConnectionStatus = false;
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.WalletYourNumberTransaction + "|0").ConfigureAwait(false))
                                            {
                                                _incomingConnectionStatus = false;
                                                return false;
                                            }
                                        }
                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.WalletAskHisAnonymityNumberTransaction:
                                        if (ClassRemoteNodeSync.ListTransactionPerWallet.ContainsKey(walletId))
                                        {
                                            if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.WalletYourAnonymityNumberTransaction + "|" + ClassRemoteNodeSync.ListTransactionPerWallet[walletId].Count).ConfigureAwait(false))
                                            {
                                                _incomingConnectionStatus = false;
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.WalletYourAnonymityNumberTransaction + "|0").ConfigureAwait(false))
                                            {
                                                _incomingConnectionStatus = false;
                                                return false;
                                            }
                                        }
                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.WalletAskNumberTransaction:
                                        if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.WalletTotalNumberTransaction + "|" + ClassRemoteNodeSync.ListOfTransaction.Count).ConfigureAwait(false))
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }
                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.AskTotalFee:
                                        if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.SendRemoteNodeTotalFee + "|" + ClassRemoteNodeSync.CurrentTotalFee).ConfigureAwait(false))
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }
                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.AskTotalBlockMined:
                                        if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.SendRemoteNodeTotalBlockMined + "|" + ClassRemoteNodeSync.ListOfBlock.Count).ConfigureAwait(false))
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }
                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.AskCoinCirculating:
                                        if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.SendRemoteNodeCoinCirculating + "|" + ClassRemoteNodeSync.CoinCirculating).ConfigureAwait(false))
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }
                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.AskCoinMaxSupply:
                                        if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.SendRemoteNodeCoinMaxSupply + "|" + ClassRemoteNodeSync.CoinMaxSupply).ConfigureAwait(false))
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }
                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.AskTotalPendingTransaction:
                                        if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.SendRemoteNodeTotalPendingTransaction + "|" + ClassRemoteNodeSync.TotalPendingTransaction).ConfigureAwait(false))
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }
                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.AskCurrentDifficulty:
                                        if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.SendRemoteNodeCurrentDifficulty + "|" + ClassRemoteNodeSync.CurrentDifficulty).ConfigureAwait(false))
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }
                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.AskCurrentRate:
                                        if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.SendRemoteNodeCurrentRate + "|" + ClassRemoteNodeSync.CurrentHashrate).ConfigureAwait(false))
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }
                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.AskTotalBlockLeft:
                                        if (!string.IsNullOrEmpty(ClassRemoteNodeSync.CurrentBlockLeft))
                                        {
                                            if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.SendRemoteNodeTotalBlockLeft + "|" + ClassRemoteNodeSync.CurrentBlockLeft).ConfigureAwait(false))
                                            {
                                                _incomingConnectionStatus = false;
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }
                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.AskTrustedKey:
                                        if (!string.IsNullOrEmpty(ClassRemoteNodeSync.TrustedKey))
                                        {
                                            if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.SendRemoteNodeTrustedKey + "|" + ClassRemoteNodeSync.TrustedKey).ConfigureAwait(false))
                                            {
                                                _incomingConnectionStatus = false;
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }
                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.AskHashListTransaction:
                                        if (!string.IsNullOrEmpty(ClassRemoteNodeSync.HashTransactionList))
                                        {
                                            if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.SendRemoteNodeTransactionHashList + "|" + ClassRemoteNodeSync.HashTransactionList).ConfigureAwait(false))
                                            {
                                                _incomingConnectionStatus = false;
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }
                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.WalletAskTransactionPerId:
                                        if (int.TryParse(splitPacket[2], out var idTransactionAskFromWallet))
                                        {
                                            if (idTransactionAskFromWallet < 0)
                                            {
                                                if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.WalletWrongIdTransaction).ConfigureAwait(false))
                                                {
                                                    _incomingConnectionStatus = false;
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                if (ClassRemoteNodeSync.ListTransactionPerWallet.ContainsKey(walletId))
                                                {
                                                    if (ClassRemoteNodeSync.ListTransactionPerWallet[walletId].Count <= idTransactionAskFromWallet)
                                                    {
                                                        if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.WalletWrongIdTransaction).ConfigureAwait(false))
                                                        {
                                                            _incomingConnectionStatus = false;
                                                            return false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        string transaction = ClassRemoteNodeSync.ListTransactionPerWallet[walletId][idTransactionAskFromWallet];
                                                        if (transaction == "WRONG")
                                                        {
                                                            if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.WalletWrongIdTransaction).ConfigureAwait(false))
                                                            {
                                                                _incomingConnectionStatus = false;
                                                                return false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.WalletTransactionPerId + "|" + transaction).ConfigureAwait(false))
                                                            {
                                                                _incomingConnectionStatus = false;
                                                                return false;
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.WalletWrongIdTransaction).ConfigureAwait(false))
                                                    {
                                                        _incomingConnectionStatus = false;
                                                        return false;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }
                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.WalletAskAnonymityTransactionPerId:
                                        if (int.TryParse(splitPacket[2], out var idAnonymityTransactionAskFromWallet))
                                        {
                                            if (idAnonymityTransactionAskFromWallet < 0)
                                            {
                                                if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.WalletWrongIdTransaction).ConfigureAwait(false))
                                                {
                                                    _incomingConnectionStatus = false;
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                if (ClassRemoteNodeSync.ListTransactionPerWallet.ContainsKey(walletId))
                                                {
                                                    if (ClassRemoteNodeSync.ListTransactionPerWallet[walletId].Count <= idAnonymityTransactionAskFromWallet)
                                                    {
                                                        if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.WalletWrongIdTransaction).ConfigureAwait(false))
                                                        {
                                                            _incomingConnectionStatus = false;
                                                            return false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        string transaction = ClassRemoteNodeSync.ListTransactionPerWallet[walletId][idAnonymityTransactionAskFromWallet];
                                                        if (transaction == "WRONG")
                                                        {
                                                            if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.WalletWrongIdTransaction).ConfigureAwait(false))
                                                            {
                                                                _incomingConnectionStatus = false;
                                                                return false;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.WalletAnonymityTransactionPerId + "|" + transaction).ConfigureAwait(false))
                                                            {
                                                                _incomingConnectionStatus = false;
                                                                return false;
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.WalletWrongIdTransaction).ConfigureAwait(false))
                                                    {
                                                        _incomingConnectionStatus = false;
                                                        return false;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }
                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.AskTransactionPerId:
                                        if (int.TryParse(splitPacket[2], out var idTransactionAsk))
                                        {
                                            if (idTransactionAsk >= 0 && idTransactionAsk < ClassRemoteNodeSync.ListOfTransaction.Count)
                                            {
                                                if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.SendRemoteNodeTransactionPerId + "|" + ClassRemoteNodeSync.ListOfTransaction[idTransactionAsk]).ConfigureAwait(false))
                                                {
                                                    _incomingConnectionStatus = false;
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                _incomingConnectionStatus = false;
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }
                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.AskTransactionHashPerId:
                                        if (int.TryParse(splitPacket[2], out var idTransactionAskTmp))
                                        {
                                            if (idTransactionAskTmp >= 0 && idTransactionAskTmp < ClassRemoteNodeSync.ListOfTransaction.Count)
                                            {
                                                if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.SendRemoteNodeAskTransactionHashPerId + "|" + ClassUtilsNode.ConvertStringToSha512(ClassRemoteNodeSync.ListOfTransaction[idTransactionAskTmp])).ConfigureAwait(false))
                                                {
                                                    _incomingConnectionStatus = false;
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                _incomingConnectionStatus = false;
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }
                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.AskBlockHashPerId:
                                        if (int.TryParse(splitPacket[2], out var idBlockAskTmp))
                                        {
                                            if (idBlockAskTmp >= 0 && idBlockAskTmp < ClassRemoteNodeSync.ListOfTransaction.Count)
                                            {
                                                if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.SendRemoteNodeAskBlockHashPerId + "|" + ClassUtilsNode.ConvertStringToSha512(ClassRemoteNodeSync.ListOfBlock[idBlockAskTmp])).ConfigureAwait(false))
                                                {
                                                    _incomingConnectionStatus = false;
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                _incomingConnectionStatus = false;
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }
                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.AskLastBlockFoundTimestamp:
                                        if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.SendRemoteNodeLastBlockFoundTimestamp + "|" + ClassRemoteNodeSync.ListOfBlock[ClassRemoteNodeSync.ListOfBlock.Count - 1].Split(new[] { "#" }, StringSplitOptions.None)[4]).ConfigureAwait(false))
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }

                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.AskBlockPerId:
                                        if (int.TryParse(splitPacket[2], out var idBlockAsk))
                                        {
                                            if (idBlockAsk >= 0 && idBlockAsk < ClassRemoteNodeSync.ListOfBlock.Count)
                                            {
                                                if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.SendRemoteNodeBlockPerId + "|" + ClassRemoteNodeSync.ListOfBlock[idBlockAsk]).ConfigureAwait(false))
                                                {
                                                    _incomingConnectionStatus = false;
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                _incomingConnectionStatus = false;
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }
                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.AskHashListBlock:
                                        if (!string.IsNullOrEmpty(ClassRemoteNodeSync.HashBlockList))
                                        {
                                            if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.SendRemoteNodeBlockHashList + "|" + ClassRemoteNodeSync.HashBlockList).ConfigureAwait(false))
                                            {
                                                _incomingConnectionStatus = false;
                                                return false;
                                            }

                                        }
                                        else
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }
                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.KeepAlive:

                                        if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.SendRemoteNodeKeepAlive).ConfigureAwait(false))
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }

                                        break;
                                    default: // Invalid packet
                                        ClassApiBan.ListBanApiIp[_ip].TotalInvalidPacket++;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.WalletIdWrong).ConfigureAwait(false)) // Wrong Wallet ID
                            {
                                _incomingConnectionStatus = false;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        ClassApiBan.ListBanApiIp[_ip].TotalInvalidPacket++;
                        if (!await SendPacketAsync(_client, ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration.EmptyPacket).ConfigureAwait(false)) // Empty Packet
                        {
                            _incomingConnectionStatus = false;
                            return false;
                        }
                    }
                }
                catch
                {
                    _incomingConnectionStatus = false;
                    return false;
                }

                return true;
            }

            return false;

        }

        /// <summary>
        /// Send packet to target.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="packet"></param>
        /// <returns></returns>
        private async Task<bool> SendPacketAsync(TcpClient client, string packet)
        {

            try
            {

                using (var packetSend = new ApiObjectConnectionSendPacket(packet + "*"))
                {
                    await client.GetStream().WriteAsync(packetSend.packetByte, 0, packetSend.packetByte.Length).ConfigureAwait(false);
                    await client.GetStream().FlushAsync().ConfigureAwait(false);
                }
            }
            catch
            {

                return false;
            }


            return true;
        }
    }
}
