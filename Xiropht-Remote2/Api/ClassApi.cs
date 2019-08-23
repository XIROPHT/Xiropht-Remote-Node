using System;
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
            })
            {
                IsBackground = true
            };
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

                    await _tcpListenerApiReceiveConnection.AcceptTcpClientAsync().ContinueWith(async clientTask =>
                    {
                        var client = await clientTask;
                        try
                        {
                            CancellationTokenSource cancellationTokenApi = new CancellationTokenSource();
                            await Task.Factory.StartNew(async () =>
                            {
                                string ip = ((IPEndPoint)(client.Client.RemoteEndPoint)).Address.ToString();

                                ClassLog.Log("API Receive incoming connection from IP: " + ip, 5, 2);
                                using (var clientApiObjectConnection = new ClassApiObjectConnection(client, ip, cancellationTokenApi))
                                {
                                    await clientApiObjectConnection.StartHandleIncomingConnectionAsync();
                                }
                            }, cancellationTokenApi.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current).ConfigureAwait(false);
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
            ApiReceiveConnectionStatus = false;
        }
    }


    public class ApiObjectConnectionSendPacket : IDisposable
    {
        public byte[] PacketByte;
        private bool _disposed;

        public ApiObjectConnectionSendPacket(string packet)
        {
            PacketByte = Encoding.UTF8.GetBytes(packet);
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
            if (_disposed)
                return;

            if (disposing)
            {
                PacketByte = null;
            }

            _disposed = true;
        }
    }
    public class ApiObjectConnectionPacket : IDisposable
    {
        public byte[] Buffer;
        public string Packet;
        private bool _disposed;

        public ApiObjectConnectionPacket()
        {
            Buffer = new byte[ClassConnectorSetting.MaxNetworkPacketSize];
            Packet = string.Empty;
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
            if (_disposed)
                return;

            if (disposing)
            {
                Buffer = null;
                Packet = null;
            }

            _disposed = true;
        }

    }

    /// <summary>
    /// Object allowed for incoming connection.
    /// </summary>
    public class ClassApiObjectConnection : IDisposable
    {
        private bool _incomingConnectionStatus;

        private TcpClient _client;
        private string _ip;
        private int _totalPacketPerSecond;
        private bool _disposed;
        private string _malformedPacket;
        private CancellationTokenSource CancellationTokenApi;
        private bool _enableProxyMode;

        public ClassApiObjectConnection(TcpClient clientTmp, string ipTmp, CancellationTokenSource cancellationTokenApi)
        {
            _client = clientTmp;
            _ip = ipTmp;
            _malformedPacket = string.Empty;
            CancellationTokenApi = cancellationTokenApi;
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
            if (_disposed)
                return;

            if (disposing)
            {
                _client = null;
            }

            _incomingConnectionStatus = false;


            _disposed = true;
        }

        /// <summary>
        /// Check packet speed of the connection opened.
        /// </summary>
        private async Task CheckPacketSpeedAsync()
        {
            while (_incomingConnectionStatus)
            {
                try
                {

                    if (_totalPacketPerSecond >= ClassApiBan.MaxPacketPerSecond)
                    {
                        ClassApiBan.ListFilterObjects[_ip].LastBanDate = DateTimeOffset.Now.ToUnixTimeSeconds() + ClassApiBan.BanDelay;
                        ClassApiBan.ListFilterObjects[_ip].Banned = true;
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
        private async Task CheckConnection()
        {

            while (_incomingConnectionStatus)
            {

                try
                {
                    if (!ClassApiBan.FilterCheckIp(_ip))
                    {
                        _incomingConnectionStatus = false;
                        break;
                    }
                    if (!_incomingConnectionStatus)
                    {
                        _incomingConnectionStatus = false;
                        break;
                    }
                    if (!ClassUtilsNode.SocketIsConnected(_client))
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

            var checkBanResult = ClassApiBan.FilterCheckIp(_ip);

            if (!checkBanResult)
            {
                _client?.Close();
                _client?.Dispose();
            }
            else
            {
                ClassApiBan.FilterInsertIp(_ip);
                await HandleIncomingConnectionAsync();
            }

        }

        /// <summary>
        /// Handle incoming connection to the api and listen packet received.
        /// </summary>
        public async Task HandleIncomingConnectionAsync()
        {
            _incomingConnectionStatus = true;
            await Task.Factory.StartNew(CheckConnection, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current).ConfigureAwait(false);
            await Task.Factory.StartNew(CheckPacketSpeedAsync, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current).ConfigureAwait(false);

            try
            {
                while (_incomingConnectionStatus)
                {
                    if (!ClassApi.ApiReceiveConnectionStatus)
                    {
                        _incomingConnectionStatus = false;
                        break;
                    }
                    if (!ClassUtilsNode.SocketIsConnected(_client))
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
                            using (var clientApiNetworkStream = new NetworkStream(_client.Client))
                            {
                                using (var bufferedStreamNetwork = new BufferedStream(clientApiNetworkStream,
                                    ClassConnectorSetting.MaxNetworkPacketSize))
                                {
                                    using (var bufferPacket = new ApiObjectConnectionPacket())
                                    {
                                        int received;


                                        while ((received = await bufferedStreamNetwork.ReadAsync(bufferPacket.Buffer, 0,
                                                   bufferPacket.Buffer.Length)) > 0)
                                        {
                                            if (received > 0)
                                            {
                                                bufferPacket.Packet =
                                                    Encoding.UTF8.GetString(bufferPacket.Buffer, 0, received);



                                                _totalPacketPerSecond++;

                                                if (bufferPacket.Packet.Contains(ClassConnectorSetting.PacketSplitSeperator))
                                                {
                                                    if (!string.IsNullOrEmpty(_malformedPacket))
                                                    {
                                                        bufferPacket.Packet = _malformedPacket + bufferPacket.Packet;
                                                        _malformedPacket = string.Empty;
                                                    }

                                                    var splitPacket = bufferPacket.Packet.Split(new[] {ClassConnectorSetting.PacketSplitSeperator},
                                                        StringSplitOptions.None);
                                                    if (splitPacket.Length > 1)
                                                    {
                                                        foreach (var packetMerged in splitPacket)
                                                        {
                                                            if (_incomingConnectionStatus)
                                                            {
                                                                if (!string.IsNullOrEmpty(packetMerged))
                                                                {
                                                                    if (packetMerged.Length > 1)
                                                                    {

                                                                        var packetReplace =
                                                                            packetMerged.Replace(ClassConnectorSetting.PacketSplitSeperator, "");
                                                                        ClassLog.Log(
                                                                            "API - Packet received from IP: " +
                                                                            _ip + " is: " + packetReplace, 5, 2);
                                                                        if (_incomingConnectionStatus)
                                                                        {
                                                                            if (!await HandleIncomingPacketAsync(
                                                                                packetReplace))
                                                                            {
                                                                                ClassLog.Log(
                                                                                    "API - Cannot send packet to IP: " +
                                                                                    _ip + "", 5, 2);
                                                                                _incomingConnectionStatus = false;
                                                                                break;
                                                                            }
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
                                                            var packetReplace = bufferPacket.Packet.Replace(ClassConnectorSetting.PacketSplitSeperator, "");
                                                            ClassLog.Log(
                                                                "API - Packet received from IP: " + _ip + " is: " +
                                                                packetReplace, 5, 2);

                                                            if (!await HandleIncomingPacketAsync(packetReplace))
                                                            {
                                                                ClassLog.Log(
                                                                    "API - Cannot send packet to IP: " + _ip + "", 5,
                                                                    2);
                                                                _incomingConnectionStatus = false;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {

                                                    ClassLog.Log(
                                                        "API - Packet received from IP: " + _ip + " is: " +
                                                        bufferPacket.Packet, 5, 2);

                                                    if (_malformedPacket.Length >= int.MaxValue ||
                                                        (long) (_malformedPacket.Length + bufferPacket.Packet.Length) >=
                                                        int.MaxValue)
                                                    {
                                                        _malformedPacket = string.Empty;
                                                    }
                                                    else
                                                    {
                                                        if (_incomingConnectionStatus)
                                                        {
                                                            _malformedPacket += bufferPacket.Packet;
                                                        }
                                                    }
                                                }
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
            _malformedPacket = string.Empty;
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
                if (!CancellationTokenApi.IsCancellationRequested)
                {
                    CancellationTokenApi.Cancel();
                }
            }
            catch
            {

            }
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

                    var splitPacket = packet.Split(new[] {"|"}, StringSplitOptions.RemoveEmptyEntries);
                    if (!string.IsNullOrEmpty(splitPacket[0]))
                    {
                        if (float.TryParse(splitPacket[1], NumberStyles.Any, Program.GlobalCultureInfo,
                            out var walletId))
                        {
                            if (walletId < 0)
                            {
                                if (!await SendPacketAsync(ClassRemoteNodeCommandForWallet
                                        .RemoteNodeRecvPacketEnumeration.WalletIdWrong)
                                    .ConfigureAwait(false)) // Wrong Wallet ID
                                {
                                    _incomingConnectionStatus = false;
                                    return false;
                                }
                            }
                            else
                            {
                                switch (splitPacket[0])
                                {
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration
                                        .WalletAskHisNumberTransaction:
                                        if (ClassRemoteNodeSync.ListTransactionPerWallet.ContainsKey(walletId) !=
                                            -1)
                                        {
                                            if (!await SendPacketAsync(
                                                ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration
                                                    .WalletYourNumberTransaction + "|" +
                                                ClassRemoteNodeSync.ListTransactionPerWallet.GetTransactionCount(
                                                    walletId)).ConfigureAwait(false))
                                            {
                                                _incomingConnectionStatus = false;
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (!await SendPacketAsync(
                                                ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration
                                                    .WalletYourNumberTransaction + "|0").ConfigureAwait(false))
                                            {
                                                _incomingConnectionStatus = false;
                                                return false;
                                            }
                                        }

                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration
                                        .WalletAskHisAnonymityNumberTransaction:
                                        if (ClassRemoteNodeSync.ListTransactionPerWallet.ContainsKey(walletId) !=
                                            -1)
                                        {
                                            if (!await SendPacketAsync(
                                                ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration
                                                    .WalletYourAnonymityNumberTransaction + "|" +
                                                ClassRemoteNodeSync.ListTransactionPerWallet.GetTransactionCount(
                                                    walletId)).ConfigureAwait(false))
                                            {
                                                _incomingConnectionStatus = false;
                                                return false;
                                            }
                                        }
                                        else
                                        {
                                            if (!await SendPacketAsync(
                                                    ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration
                                                        .WalletYourAnonymityNumberTransaction + "|0")
                                                .ConfigureAwait(false))
                                            {
                                                _incomingConnectionStatus = false;
                                                return false;
                                            }
                                        }

                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration
                                        .WalletAskNumberTransaction:
                                        if (!await SendPacketAsync(
                                            ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration
                                                .WalletTotalNumberTransaction + "|" +
                                            ClassRemoteNodeSync.ListOfTransaction.Count).ConfigureAwait(false))
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }

                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration
                                        .AskTotalFee:
                                        if (!await SendPacketAsync(
                                                ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration
                                                    .SendRemoteNodeTotalFee + "|" +
                                                ClassRemoteNodeSync.CurrentTotalFee)
                                            .ConfigureAwait(false))
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }

                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration
                                        .AskTotalBlockMined:
                                        if (!await SendPacketAsync(
                                            ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration
                                                .SendRemoteNodeTotalBlockMined + "|" +
                                            ClassRemoteNodeSync.ListOfBlock.Count).ConfigureAwait(false))
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }

                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration
                                        .AskCoinCirculating:
                                        if (!await SendPacketAsync(
                                            ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration
                                                .SendRemoteNodeCoinCirculating + "|" +
                                            ClassRemoteNodeSync.CoinCirculating).ConfigureAwait(false))
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }

                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration
                                        .AskCoinMaxSupply:
                                        if (!await SendPacketAsync(
                                            ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration
                                                .SendRemoteNodeCoinMaxSupply + "|" +
                                            ClassRemoteNodeSync.CoinMaxSupply).ConfigureAwait(false))
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }

                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration
                                        .AskTotalPendingTransaction:
                                        if (!await SendPacketAsync(
                                            ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration
                                                .SendRemoteNodeTotalPendingTransaction + "|" +
                                            ClassRemoteNodeSync.TotalPendingTransaction).ConfigureAwait(false))
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }

                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration
                                        .AskCurrentDifficulty:
                                        if (!await SendPacketAsync(
                                            ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration
                                                .SendRemoteNodeCurrentDifficulty + "|" +
                                            ClassRemoteNodeSync.CurrentDifficulty).ConfigureAwait(false))
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }

                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration
                                        .AskCurrentRate:
                                        if (!await SendPacketAsync(
                                            ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration
                                                .SendRemoteNodeCurrentRate + "|" +
                                            ClassRemoteNodeSync.CurrentHashrate).ConfigureAwait(false))
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }

                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration
                                        .AskTotalBlockLeft:
                                        if (!string.IsNullOrEmpty(ClassRemoteNodeSync.CurrentBlockLeft))
                                        {
                                            if (!await SendPacketAsync(
                                                ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration
                                                    .SendRemoteNodeTotalBlockLeft + "|" +
                                                ClassRemoteNodeSync.CurrentBlockLeft).ConfigureAwait(false))
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
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration
                                        .AskTrustedKey:
                                        if (!string.IsNullOrEmpty(ClassRemoteNodeSync.TrustedKey))
                                        {
                                            if (!await SendPacketAsync(
                                                ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration
                                                    .SendRemoteNodeTrustedKey + "|" +
                                                ClassRemoteNodeSync.TrustedKey).ConfigureAwait(false))
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
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration
                                        .AskHashListTransaction:
                                        if (!string.IsNullOrEmpty(ClassRemoteNodeSync.HashTransactionList))
                                        {
                                            if (!await SendPacketAsync(
                                                ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration
                                                    .SendRemoteNodeTransactionHashList + "|" +
                                                ClassRemoteNodeSync.HashTransactionList).ConfigureAwait(false))
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
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration
                                        .WalletAskTransactionPerId:
                                        if (int.TryParse(splitPacket[2], out var idTransactionAskFromWallet))
                                        {
                                            if (idTransactionAskFromWallet < 0)
                                            {
                                                if (!await SendPacketAsync(ClassRemoteNodeCommandForWallet
                                                        .RemoteNodeRecvPacketEnumeration.WalletWrongIdTransaction)
                                                    .ConfigureAwait(false))
                                                {
                                                    _incomingConnectionStatus = false;
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                if (ClassRemoteNodeSync.ListTransactionPerWallet.ContainsKey(
                                                        walletId) != -1)
                                                {
                                                    if (ClassRemoteNodeSync.ListTransactionPerWallet
                                                            .GetTransactionCount(walletId) <=
                                                        idTransactionAskFromWallet)
                                                    {
                                                        if (!await SendPacketAsync(ClassRemoteNodeCommandForWallet
                                                            .RemoteNodeRecvPacketEnumeration
                                                            .WalletWrongIdTransaction).ConfigureAwait(false))
                                                        {
                                                            _incomingConnectionStatus = false;
                                                            return false;
                                                        }
                                                    }
                                                    else
                                                    {

                                                        using (var apiTransaction = new ClassApiTransaction())
                                                        {
                                                            string resultTransaction =
                                                                apiTransaction.GetTransactionFromWalletId(walletId,
                                                                    idTransactionAskFromWallet);
                                                            if (resultTransaction == "WRONG")
                                                            {
                                                                if (!await SendPacketAsync(
                                                                        ClassRemoteNodeCommandForWallet
                                                                            .RemoteNodeRecvPacketEnumeration
                                                                            .WalletWrongIdTransaction)
                                                                    .ConfigureAwait(false))
                                                                {
                                                                    _incomingConnectionStatus = false;
                                                                    return false;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (!await SendPacketAsync(
                                                                    ClassRemoteNodeCommandForWallet
                                                                        .RemoteNodeRecvPacketEnumeration
                                                                        .WalletTransactionPerId + "|" +
                                                                    resultTransaction).ConfigureAwait(false))
                                                                {
                                                                    _incomingConnectionStatus = false;
                                                                    return false;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (!await SendPacketAsync(ClassRemoteNodeCommandForWallet
                                                            .RemoteNodeRecvPacketEnumeration
                                                            .WalletWrongIdTransaction)
                                                        .ConfigureAwait(false))
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
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration
                                        .WalletAskAnonymityTransactionPerId:
                                        if (int.TryParse(splitPacket[2],
                                            out var idAnonymityTransactionAskFromWallet))
                                        {
                                            if (idAnonymityTransactionAskFromWallet < 0)
                                            {
                                                if (!await SendPacketAsync(ClassRemoteNodeCommandForWallet
                                                        .RemoteNodeRecvPacketEnumeration.WalletWrongIdTransaction)
                                                    .ConfigureAwait(false))
                                                {
                                                    _incomingConnectionStatus = false;
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                if (ClassRemoteNodeSync.ListTransactionPerWallet.ContainsKey(
                                                        walletId) != -1)
                                                {
                                                    if (ClassRemoteNodeSync.ListTransactionPerWallet
                                                            .GetTransactionCount(walletId) <=
                                                        idAnonymityTransactionAskFromWallet)
                                                    {
                                                        if (!await SendPacketAsync(ClassRemoteNodeCommandForWallet
                                                            .RemoteNodeRecvPacketEnumeration
                                                            .WalletWrongIdTransaction).ConfigureAwait(false))
                                                        {
                                                            _incomingConnectionStatus = false;
                                                            return false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        using (var apiTransaction = new ClassApiTransaction())
                                                        {
                                                            string resultTransaction =
                                                                apiTransaction.GetTransactionFromWalletId(walletId,
                                                                    idAnonymityTransactionAskFromWallet);
                                                            if (resultTransaction == "WRONG")
                                                            {
                                                                if (!await SendPacketAsync(
                                                                        ClassRemoteNodeCommandForWallet
                                                                            .RemoteNodeRecvPacketEnumeration
                                                                            .WalletWrongIdTransaction)
                                                                    .ConfigureAwait(false))
                                                                {
                                                                    _incomingConnectionStatus = false;
                                                                    return false;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                if (!await SendPacketAsync(
                                                                    ClassRemoteNodeCommandForWallet
                                                                        .RemoteNodeRecvPacketEnumeration
                                                                        .WalletAnonymityTransactionPerId + "|" +
                                                                    resultTransaction).ConfigureAwait(false))
                                                                {
                                                                    _incomingConnectionStatus = false;
                                                                    return false;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    if (!await SendPacketAsync(ClassRemoteNodeCommandForWallet
                                                            .RemoteNodeRecvPacketEnumeration
                                                            .WalletWrongIdTransaction)
                                                        .ConfigureAwait(false))
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
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration
                                        .AskTransactionPerId:
                                        if (long.TryParse(splitPacket[2], out var idTransactionAsk))
                                        {
                                            if (idTransactionAsk >= 0 && idTransactionAsk <
                                                ClassRemoteNodeSync.ListOfTransaction.Count)
                                            {
                                                if (!await SendPacketAsync(
                                                        ClassRemoteNodeCommandForWallet
                                                            .RemoteNodeRecvPacketEnumeration
                                                            .SendRemoteNodeTransactionPerId + "|" +
                                                        ClassRemoteNodeSync.ListOfTransaction
                                                            .GetTransaction(idTransactionAsk).Item1)
                                                    .ConfigureAwait(false))
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
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration
                                        .AskTransactionHashPerId:
                                        if (int.TryParse(splitPacket[2], out var idTransactionAskTmp))
                                        {
                                            if (idTransactionAskTmp >= 0 && idTransactionAskTmp <
                                                ClassRemoteNodeSync.ListOfTransaction.Count)
                                            {
                                                if (!await SendPacketAsync(
                                                    ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration
                                                        .SendRemoteNodeAskTransactionHashPerId + "|" +
                                                    ClassUtilsNode.ConvertStringToSha512(ClassRemoteNodeSync
                                                        .ListOfTransaction.GetTransaction(idTransactionAskTmp)
                                                        .Item1)).ConfigureAwait(false))
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
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration
                                        .AskBlockHashPerId:
                                        if (int.TryParse(splitPacket[2], out var idBlockAskTmp))
                                        {
                                            if (idBlockAskTmp >= 0 && idBlockAskTmp <
                                                ClassRemoteNodeSync.ListOfTransaction.Count)
                                            {
                                                if (!await SendPacketAsync(
                                                        ClassRemoteNodeCommandForWallet
                                                            .RemoteNodeRecvPacketEnumeration
                                                            .SendRemoteNodeAskBlockHashPerId + "|" +
                                                        ClassUtilsNode.ConvertStringToSha512(
                                                            ClassRemoteNodeSync.ListOfBlock[idBlockAskTmp]))
                                                    .ConfigureAwait(false))
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
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration
                                        .AskLastBlockFoundTimestamp:
                                        if (!await SendPacketAsync(
                                                ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration
                                                    .SendRemoteNodeLastBlockFoundTimestamp + "|" +
                                                ClassRemoteNodeSync
                                                    .ListOfBlock[ClassRemoteNodeSync.ListOfBlock.Count - 1]
                                                    .Split(new[] {"#"}, StringSplitOptions.None)[4])
                                            .ConfigureAwait(false))
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }

                                        break;
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration
                                        .AskBlockPerId:
                                        if (int.TryParse(splitPacket[2], out var idBlockAsk))
                                        {
                                            if (idBlockAsk >= 0 &&
                                                idBlockAsk < ClassRemoteNodeSync.ListOfBlock.Count)
                                            {
                                                if (!await SendPacketAsync(
                                                        ClassRemoteNodeCommandForWallet
                                                            .RemoteNodeRecvPacketEnumeration
                                                            .SendRemoteNodeBlockPerId + "|" +
                                                        ClassRemoteNodeSync.ListOfBlock[idBlockAsk])
                                                    .ConfigureAwait(false))
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
                                    case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration
                                        .AskHashListBlock:
                                        if (!string.IsNullOrEmpty(ClassRemoteNodeSync.HashBlockList))
                                        {
                                            if (!await SendPacketAsync(
                                                ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration
                                                    .SendRemoteNodeBlockHashList + "|" +
                                                ClassRemoteNodeSync.HashBlockList).ConfigureAwait(false))
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

                                        if (!await SendPacketAsync(ClassRemoteNodeCommandForWallet
                                                .RemoteNodeRecvPacketEnumeration.SendRemoteNodeKeepAlive)
                                            .ConfigureAwait(false))
                                        {
                                            _incomingConnectionStatus = false;
                                            return false;
                                        }

                                        break;
                                    default: // Invalid packet
                                        ClassApiBan.ListFilterObjects[_ip].TotalInvalidPacket++;
                                        break;
                                }
                            }
                        }
                        else
                        {
                            if (!await SendPacketAsync(ClassRemoteNodeCommandForWallet
                                    .RemoteNodeRecvPacketEnumeration.WalletIdWrong)
                                .ConfigureAwait(false)) // Wrong Wallet ID
                            {
                                _incomingConnectionStatus = false;
                                return false;
                            }
                        }
                    }
                    else
                    {
                        ClassApiBan.ListFilterObjects[_ip].TotalInvalidPacket++;
                        if (!await SendPacketAsync(ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration
                            .EmptyPacket).ConfigureAwait(false)) // Empty Packet
                        {
                            _incomingConnectionStatus = false;
                            return false;
                        }
                    }

                }
                catch
                {
                    ClassApiBan.ListFilterObjects[_ip].TotalInvalidPacket++;
                }

                return true;
            }

            return false;

        }

        /// <summary>
        /// Send packet to target.
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        private async Task<bool> SendPacketAsync(string packet)
        {

            try
            {
                using (var clientApiNetworkStream = new NetworkStream(_client.Client))
                {
                    using (var bufferedNetworkStream = new BufferedStream(clientApiNetworkStream, ClassConnectorSetting.MaxNetworkPacketSize))
                    {
                        using (var packetSend = new ApiObjectConnectionSendPacket(packet + ClassConnectorSetting.PacketSplitSeperator))
                        {
                            await bufferedNetworkStream.WriteAsync(packetSend.PacketByte, 0, packetSend.PacketByte.Length).ConfigureAwait(false);
                            await bufferedNetworkStream.FlushAsync().ConfigureAwait(false);
                        }
                    }
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
