using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xiropht_Connector_All.Remote;
using Xiropht_Connector_All.RPC;
using Xiropht_Connector_All.Setting;
using Xiropht_RemoteNode.Data;
using Xiropht_RemoteNode.Log;
using Xiropht_RemoteNode.Token;
using Xiropht_RemoteNode.Utils;

namespace Xiropht_RemoteNode.Api
{


    /// <summary>
    /// Done for receive command from wallet or other systems and send a reply of the information asked.
    /// </summary>
    public class ClassApi
    {
        private static CancellationTokenSource _cancellationTokenApi;
        private static TcpListener _tcpListenerApiReceiveConnection;

        /// <summary>
        /// Start to listen incoming connection to API.
        /// </summary>
        public static void StartApiRemoteNode()
        {
            _cancellationTokenApi = new CancellationTokenSource();

            _tcpListenerApiReceiveConnection = new TcpListener(IPAddress.Any, ClassConnectorSetting.RemoteNodePort);
            _tcpListenerApiReceiveConnection.Start();

            try
            {
                // Async
                Task.Factory.StartNew(async delegate()
                {
                    while (!Program.Closed)
                    {
                        try
                        {

                            await _tcpListenerApiReceiveConnection.AcceptTcpClientAsync().ContinueWith(
                                async clientTask =>
                                {
                                    var client = await clientTask;
                                    try
                                    {
                                        CancellationTokenSource cancellationTokenApi = new CancellationTokenSource();
                                        await Task.Factory.StartNew(async () =>
                                                {
                                                    string ip = ((IPEndPoint) (client.Client.RemoteEndPoint)).Address
                                                        .ToString();

                                                    ClassLog.Log("API Receive incoming connection from IP: " + ip, 5,
                                                        2);
                                                    using (var clientApiObjectConnection =
                                                        new ClassApiObjectConnection(client, ip, cancellationTokenApi))
                                                    {
                                                        await clientApiObjectConnection
                                                            .StartHandleIncomingConnectionAsync();
                                                    }
                                                }, cancellationTokenApi.Token, TaskCreationOptions.LongRunning,
                                                TaskScheduler.Current)
                                            .ConfigureAwait(false);
                                    }
                                    catch
                                    {

                                    }
                                }, _cancellationTokenApi.Token);
                        }
                        catch
                        {
                        }
                    }

                }, _cancellationTokenApi.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current).ConfigureAwait(false);
            }
            catch
            {

            }


        }

        /// <summary>
        /// Stop api.
        /// </summary>
        public static void StopApi()
        {
            try
            {
                if (_cancellationTokenApi != null)
                {
                    if (!_cancellationTokenApi.IsCancellationRequested)
                    {
                        _cancellationTokenApi.Cancel();
                    }
                }
            }
            catch
            {

            }
            _tcpListenerApiReceiveConnection.Stop();
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
        public bool IncomingConnectionStatus;

        private TcpClient _client;
        public string Ip;
        private int _totalPacketPerSecond;
        private bool _disposed;
        private string _malformedPacket;
        public CancellationTokenSource CancellationTokenApi;
        private bool EnableProxyMode;
        private ClassApiProxyNetwork _apiProxyNetwork;

        public ClassApiObjectConnection(TcpClient clientTmp, string ipTmp, CancellationTokenSource cancellationTokenApi)
        {
            _client = clientTmp;
            Ip = ipTmp;
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

            IncomingConnectionStatus = false;


            _disposed = true;
        }

        /// <summary>
        /// Check packet speed of the connection opened.
        /// </summary>
        private async Task CheckPacketSpeedAsync()
        {
            while (IncomingConnectionStatus)
            {
                try
                {

                    if (_totalPacketPerSecond >= ClassApiBan.MaxPacketPerSecond)
                    {
                        ClassApiBan.ListFilterObjects[Ip].LastBanDate = DateTimeOffset.Now.ToUnixTimeSeconds() + ClassApiBan.BanDelay;
                        ClassApiBan.ListFilterObjects[Ip].Banned = true;
                        IncomingConnectionStatus = false;
                        break;
                    }
                    ClassLog.Log("API - Total packets received from IP: " + Ip + " is: " + _totalPacketPerSecond, 5, 2);
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

            while (IncomingConnectionStatus)
            {

                try
                {
                    if (!ClassApiBan.FilterCheckIp(Ip))
                    {
                        IncomingConnectionStatus = false;
                        break;
                    }
                    if (!IncomingConnectionStatus)
                    {
                        IncomingConnectionStatus = false;
                        break;
                    }
                    if (!ClassUtilsNode.SocketIsConnected(_client))
                    {
                        IncomingConnectionStatus = false;
                        break;
                    }

                    if (EnableProxyMode)
                    {
                        if (_apiProxyNetwork != null)
                        {
                            if (!_apiProxyNetwork.ConnectionAlive)
                            {
                                IncomingConnectionStatus = false;
                                break;
                            }
                        }
                    }
                }
                catch
                {
                    IncomingConnectionStatus = false;
                    break;
                }
                await Task.Delay(1000);
            }
            StopClientApiConnection();
            Dispose();
        }

        public async Task StartHandleIncomingConnectionAsync()
        {

            var checkBanResult = ClassApiBan.FilterCheckIp(Ip);

            if (!checkBanResult)
            {
                _client?.Close();
                _client?.Dispose();
            }
            else
            {
                ClassApiBan.FilterInsertIp(Ip);
                await HandleIncomingConnectionAsync();
            }

        }

        /// <summary>
        /// Handle incoming connection to the api and listen packet received.
        /// </summary>
        public async Task HandleIncomingConnectionAsync()
        {
            IncomingConnectionStatus = true;
            await Task.Factory.StartNew(CheckConnection, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current).ConfigureAwait(false);
            await Task.Factory.StartNew(CheckPacketSpeedAsync, CancellationToken.None, TaskCreationOptions.LongRunning, TaskScheduler.Current).ConfigureAwait(false);

            try
            {

                while (IncomingConnectionStatus)
                {
                    if (Program.Closed)
                    {
                        IncomingConnectionStatus = false;
                        break;
                    }
                    if (!ClassUtilsNode.SocketIsConnected(_client))
                    {
                        IncomingConnectionStatus = false;
                        break;
                    }
                    if (!IncomingConnectionStatus)
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

                                                if (!EnableProxyMode)
                                                {

                                                    if (bufferPacket.Packet.Contains(ClassConnectorSetting
                                                        .PacketSplitSeperator))
                                                    {
                                                        if (!string.IsNullOrEmpty(_malformedPacket))
                                                        {
                                                            bufferPacket.Packet =
                                                                _malformedPacket + bufferPacket.Packet;
                                                            _malformedPacket = string.Empty;
                                                        }

                                                        var splitPacket = bufferPacket.Packet.Split(
                                                            new[] {ClassConnectorSetting.PacketSplitSeperator},
                                                            StringSplitOptions.None);
                                                        if (splitPacket.Length > 1)
                                                        {
                                                            foreach (var packetMerged in splitPacket)
                                                            {
                                                                if (IncomingConnectionStatus)
                                                                {
                                                                    if (!string.IsNullOrEmpty(packetMerged))
                                                                    {
                                                                        if (packetMerged.Length > 1)
                                                                        {

                                                                            var packetReplace =
                                                                                packetMerged.Replace(
                                                                                    ClassConnectorSetting
                                                                                        .PacketSplitSeperator, "");
                                                                            ClassLog.Log(
                                                                                "API - Packet received from IP: " +
                                                                                Ip + " is: " + packetReplace, 5, 2);
                                                                            if (IncomingConnectionStatus)
                                                                            {
                                                                                if (!await HandleIncomingPacketAsync(
                                                                                    packetReplace))
                                                                                {
                                                                                    ClassLog.Log(
                                                                                        "API - Cannot send packet to IP: " +
                                                                                        Ip + "", 5, 2);
                                                                                    IncomingConnectionStatus = false;
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
                                                            if (IncomingConnectionStatus)
                                                            {
                                                                var packetReplace =
                                                                    bufferPacket.Packet.Replace(
                                                                        ClassConnectorSetting.PacketSplitSeperator, "");
                                                                ClassLog.Log(
                                                                    "API - Packet received from IP: " + Ip + " is: " +
                                                                    packetReplace, 5, 2);

                                                                if (!await HandleIncomingPacketAsync(packetReplace))
                                                                {
                                                                    ClassLog.Log(
                                                                        "API - Cannot send packet to IP: " + Ip + "", 5,
                                                                        2);
                                                                    IncomingConnectionStatus = false;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {


                                                        if (_malformedPacket.Length >= int.MaxValue ||
                                                            (long) (_malformedPacket.Length + bufferPacket.Packet.Length
                                                            ) >=
                                                            int.MaxValue)
                                                        {
                                                            _malformedPacket = string.Empty;
                                                        }
                                                        else
                                                        {
                                                            if (IncomingConnectionStatus)
                                                            {
                                                                _malformedPacket += bufferPacket.Packet;
                                                            }
                                                        }

                                                    }
                                                }
                                                else
                                                {
                                                    if (_apiProxyNetwork != null)
                                                    {
                                                        if (!await _apiProxyNetwork.SendPacketToNetwork(bufferPacket.Packet))
                                                        {
                                                            IncomingConnectionStatus = false;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                            if (!IncomingConnectionStatus)
                            {
                                break;
                            }
                        }
                        catch (Exception error)
                        {
                            IncomingConnectionStatus = false;
                            ClassLog.Log("API Incoming connection from IP: " + Ip + " closed by exception: " + error.Message, 5, 2);
                            break;
                        }
                    }
                    catch (Exception error)
                    {
                        IncomingConnectionStatus = false;
                        ClassLog.Log("API Incoming connection from IP: " + Ip + " closed by exception: " + error.Message, 5, 2);
                        break;
                    }
                }
            }
            catch (Exception error)
            {
                IncomingConnectionStatus = false;
                ClassLog.Log("API Incoming connection from IP: " + Ip + " closed by exception: " + error.Message, 5, 2);
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

            try
            {
                if (EnableProxyMode)
                {
                    _apiProxyNetwork?.Dispose();
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
            if (!IncomingConnectionStatus)
            {
                return false;
            }

            if (IncomingConnectionStatus)
            {

                try
                {

                    var splitPacket = packet.Split(new[] {ClassConnectorSetting.PacketContentSeperator},
                        StringSplitOptions.RemoveEmptyEntries);
                    if (!string.IsNullOrEmpty(splitPacket[0]))
                    {
                        if (splitPacket[0] == ClassConnectorSettingEnumeration.WalletLoginProxy)
                        {
                            ClassLog.Log(
                                "API - Attempt to connect wallet address: " + splitPacket[1] +
                                " by proxy mode to the network.", 5, 0);

                            if (splitPacket[1].Length >= ClassConnectorSetting.MinWalletAddressSize &&
                                splitPacket[1].Length <= ClassConnectorSetting.MaxWalletAddressSize)
                            {
                                if (ClassRemoteNodeSync.DictionaryCacheValidWalletAddress.ContainsKey(
                                    splitPacket[1]))
                                {
                                    EnableProxyMode = true;
                                    _apiProxyNetwork =
                                        new ClassApiProxyNetwork(splitPacket[1], this, splitPacket[2]);
                                    if (!await _apiProxyNetwork.StartProxyNetwork())
                                    {
                                        IncomingConnectionStatus = false;
                                        return false;
                                    }
                                }
                                else
                                {
                                    if (await ClassTokenNetwork.CheckWalletAddressExistAsync(splitPacket[1]))
                                    {
                                        EnableProxyMode = true;
                                        _apiProxyNetwork =
                                            new ClassApiProxyNetwork(splitPacket[1], this, splitPacket[2]);
                                        if (!await _apiProxyNetwork.StartProxyNetwork())
                                        {
                                            IncomingConnectionStatus = false;
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        IncomingConnectionStatus = false;
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                ClassApiBan.ListFilterObjects[Ip].TotalInvalidPacket++;
                                IncomingConnectionStatus = false;
                                return false;
                            }
                        }
                        else if (splitPacket[0] == ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteAskProxyConfirmation)
                        {
                            if (splitPacket[1].Length >= ClassConnectorSetting.MinWalletAddressSize &&
                                splitPacket[1].Length <= ClassConnectorSetting.MaxWalletAddressSize)
                            {
                                if (ClassRemoteNodeSync.DictionaryCacheValidWalletAddress.ContainsKey(
                                    splitPacket[1]))
                                {
                                    string packetQuestionResult = await ClassTokenNetwork.GetWalletQuestionConfirmation(splitPacket[1]);
                                    if (!string.IsNullOrEmpty(packetQuestionResult))
                                    {
                                        if (packetQuestionResult == "WRONG")
                                        {
                                            ClassApiBan.ListFilterObjects[Ip].TotalInvalidPacket++;
                                            IncomingConnectionStatus = false;
                                            return false;
                                        }

                                        await SendPacketAsync(ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteSendProxyConfirmation + ClassConnectorSetting.PacketContentSeperator + packetQuestionResult);
                                    }
                                    else
                                    {
                                        IncomingConnectionStatus = false;
                                        return false;
                                    }
                                }
                                else
                                {
                                    if (await ClassTokenNetwork.CheckWalletAddressExistAsync(splitPacket[1]))
                                    {
                                        string packetQuestionResult = await ClassTokenNetwork.GetWalletQuestionConfirmation(splitPacket[1]);
                                        if (!string.IsNullOrEmpty(packetQuestionResult))
                                        {
                                            if (packetQuestionResult == "WRONG")
                                            {
                                                ClassApiBan.ListFilterObjects[Ip].TotalInvalidPacket++;
                                                IncomingConnectionStatus = false;
                                                return false;
                                            }

                                            await SendPacketAsync(ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteSendProxyConfirmation + ClassConnectorSetting.PacketContentSeperator + packetQuestionResult);
                                        }
                                        else
                                        {
                                            IncomingConnectionStatus = false;
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        IncomingConnectionStatus = false;
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                ClassApiBan.ListFilterObjects[Ip].TotalInvalidPacket++;
                                IncomingConnectionStatus = false;
                                return false;
                            }
                        }

                        else
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
                                        IncomingConnectionStatus = false;
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
                                                    IncomingConnectionStatus = false;
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                if (!await SendPacketAsync(
                                                    ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration
                                                        .WalletYourNumberTransaction + "|0").ConfigureAwait(false))
                                                {
                                                    IncomingConnectionStatus = false;
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
                                                    IncomingConnectionStatus = false;
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
                                                    IncomingConnectionStatus = false;
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
                                                IncomingConnectionStatus = false;
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
                                                IncomingConnectionStatus = false;
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
                                                IncomingConnectionStatus = false;
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
                                                IncomingConnectionStatus = false;
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
                                                IncomingConnectionStatus = false;
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
                                                IncomingConnectionStatus = false;
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
                                                IncomingConnectionStatus = false;
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
                                                IncomingConnectionStatus = false;
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
                                                    IncomingConnectionStatus = false;
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                IncomingConnectionStatus = false;
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
                                                    IncomingConnectionStatus = false;
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                IncomingConnectionStatus = false;
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
                                                    IncomingConnectionStatus = false;
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                IncomingConnectionStatus = false;
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
                                                        IncomingConnectionStatus = false;
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
                                                                IncomingConnectionStatus = false;
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
                                                                        IncomingConnectionStatus = false;
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
                                                                        IncomingConnectionStatus = false;
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
                                                            IncomingConnectionStatus = false;
                                                            return false;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                IncomingConnectionStatus = false;
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
                                                        IncomingConnectionStatus = false;
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
                                                                IncomingConnectionStatus = false;
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
                                                                        IncomingConnectionStatus = false;
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
                                                                        IncomingConnectionStatus = false;
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
                                                            IncomingConnectionStatus = false;
                                                            return false;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                IncomingConnectionStatus = false;
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
                                                        IncomingConnectionStatus = false;
                                                        return false;
                                                    }
                                                }
                                                else
                                                {
                                                    IncomingConnectionStatus = false;
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                IncomingConnectionStatus = false;
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
                                                        IncomingConnectionStatus = false;
                                                        return false;
                                                    }
                                                }
                                                else
                                                {
                                                    IncomingConnectionStatus = false;
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                IncomingConnectionStatus = false;
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
                                                        IncomingConnectionStatus = false;
                                                        return false;
                                                    }
                                                }
                                                else
                                                {
                                                    IncomingConnectionStatus = false;
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                IncomingConnectionStatus = false;
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
                                                IncomingConnectionStatus = false;
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
                                                        IncomingConnectionStatus = false;
                                                        return false;
                                                    }
                                                }
                                                else
                                                {
                                                    IncomingConnectionStatus = false;
                                                    return false;
                                                }
                                            }
                                            else
                                            {
                                                IncomingConnectionStatus = false;
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
                                                    IncomingConnectionStatus = false;
                                                    return false;
                                                }

                                            }
                                            else
                                            {
                                                IncomingConnectionStatus = false;
                                                return false;
                                            }

                                            break;
                                        case ClassRemoteNodeCommandForWallet.RemoteNodeSendPacketEnumeration.KeepAlive:

                                            if (!await SendPacketAsync(ClassRemoteNodeCommandForWallet
                                                    .RemoteNodeRecvPacketEnumeration.SendRemoteNodeKeepAlive)
                                                .ConfigureAwait(false))
                                            {
                                                IncomingConnectionStatus = false;
                                                return false;
                                            }

                                            break;
                                        default: // Invalid packet
                                            ClassApiBan.ListFilterObjects[Ip].TotalInvalidPacket++;
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
                                    IncomingConnectionStatus = false;
                                    return false;
                                }
                            }
                        }

                    }
                    else
                    {
                        ClassApiBan.ListFilterObjects[Ip].TotalInvalidPacket++;
                        if (!await SendPacketAsync(ClassRemoteNodeCommandForWallet.RemoteNodeRecvPacketEnumeration
                            .EmptyPacket).ConfigureAwait(false)) // Empty Packet
                        {
                            IncomingConnectionStatus = false;
                            return false;
                        }
                    }

                }
                catch
                {
                    ClassApiBan.ListFilterObjects[Ip].TotalInvalidPacket++;
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
        public async Task<bool> SendPacketAsync(string packet)
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
