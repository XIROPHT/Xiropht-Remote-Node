using System;
using System.Threading.Tasks;
using Xiropht_Connector_All.Seed;
using Xiropht_Connector_All.Setting;
using Xiropht_Connector_All.Utils;
using Xiropht_Connector_All.Wallet;
using Xiropht_RemoteNode.Log;

namespace Xiropht_RemoteNode.Api
{
    public class ClapiApiProxyLimitation
    {
        public const int MaxNonePacket = 20;
    }

    public class ClassApiProxyNetwork : IDisposable
    {

        #region disposing functions
        private bool _disposed;

        ~ClassApiProxyNetwork()
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

            try
            {
                _seedNodeConnector?.DisconnectToSeed();
                _seedNodeConnector?.Dispose();
            }
            catch
            {

            }

            ConnectionAlive = false;


            _disposed = true;
        }

        #endregion

        private ClassSeedNodeConnector _seedNodeConnector;
        private ClassApiObjectConnection _apiObjectConnection;
        private string _walletAddress;
        private string _certificate;
        public bool ConnectionAlive;
        private int _maxNonePacket;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="walletAddress"></param>
        /// <param name="apiObjectConnection"></param>
        /// <param name="certificate"></param>
        public ClassApiProxyNetwork(string walletAddress, ClassApiObjectConnection apiObjectConnection, string certificate)
        {
            _walletAddress = walletAddress;
            _apiObjectConnection = apiObjectConnection;
            _certificate = certificate;
        }

        /// <summary>
        /// Start to connect to the network like a proxy.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> StartProxyNetwork()
        {
            ConnectionAlive = true;

            try
            {
                ClassLog.Log("API Proxy - Attempt to connect wallet address: " + _walletAddress + " with certificate: " + _certificate, 5, 0);

                _seedNodeConnector = new ClassSeedNodeConnector();
                if (!await _seedNodeConnector.StartConnectToSeedAsync(string.Empty))
                {
                    ConnectionAlive = false;
                    ClassLog.Log("API Proxy - Connection to the network unsuccessfully done for wallet address: " + _walletAddress, 5, 0);

                    return false;
                }


                if (!await _seedNodeConnector.SendPacketToSeedNodeAsync(_certificate, string.Empty))
                {
                    ConnectionAlive = false;
                    ClassLog.Log("API Proxy - Connection to the network can't send certificate for wallet address: " + _walletAddress, 5, 0);
                    return false;
                }

                if (!ListenNetwork())
                {
                    ConnectionAlive = false;
                    ClassLog.Log("API Proxy - Connection to the network can't enable listen packet for wallet address: " + _walletAddress, 5, 0);
                    return false;
                }



                if (!await _seedNodeConnector.SendPacketToSeedNodeAsync(ClassConnectorSettingEnumeration.WalletLoginType + ClassConnectorSetting.PacketContentSeperator + _walletAddress + ClassConnectorSetting.PacketSplitSeperator, _certificate, false, true))
                {
                    ConnectionAlive = false;
                    ClassLog.Log("API Proxy - Connection to the network can't send login packet for wallet address: " + _walletAddress, 5, 0);
                    return false;
                }

                await Task.Factory.StartNew(CheckNetworkProxyStatus, _apiObjectConnection.CancellationTokenApi.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current).ConfigureAwait(false);
            }
            catch
            {
                ConnectionAlive = false;
                return false;
            }

            ClassLog.Log("API Proxy - Connection to the network propertly enabled for wallet address: "+_walletAddress, 5, 0);

            return true;
        }

        /// <summary>
        /// Check network proxy status.
        /// </summary>
        /// <returns></returns>
        private async Task CheckNetworkProxyStatus()
        {
            while (_apiObjectConnection.IncomingConnectionStatus && ConnectionAlive)
            {
                try
                {
                    if (!_seedNodeConnector.ReturnStatus())
                    {
                        break;
                    }

                    if (_apiObjectConnection.CancellationTokenApi != null)
                    {
                        if (_apiObjectConnection.CancellationTokenApi.IsCancellationRequested)
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }

                    if (!ClassApiBan.FilterCheckIp(_apiObjectConnection.Ip))
                    {
                        break;
                    }

                }
                catch
                {
                    break;
                }

                await Task.Delay(1000);
            }

            ConnectionAlive = false;
        }

        /// <summary>
        /// Send packets to the network.
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        public async Task<bool> SendPacketToNetwork(string packet)
        {
            try
            {
                if (!await _seedNodeConnector.SendPacketToSeedNodeAsync(packet, string.Empty))
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Listen network packets.
        /// </summary>
        /// <returns></returns>
        private bool ListenNetwork()
        {
            try
            {
                Task.Factory.StartNew(async delegate
                    {


                        while (_seedNodeConnector.ReturnStatus() && _apiObjectConnection.IncomingConnectionStatus && ConnectionAlive)
                        {
                            try
                            {
                                string packetReceived =
                                    await _seedNodeConnector.ReceivePacketFromSeedNodeAsync(_certificate, false,
                                        true);


                                ClassLog.Log("API Proxy - Packet Received from network: " + packetReceived, 5, 1);
                                if (packetReceived.Contains(ClassConnectorSetting.PacketSplitSeperator))
                                {
                                    var splitPacketReceived = packetReceived.Split(
                                        new[] {ClassConnectorSetting.PacketSplitSeperator},
                                        StringSplitOptions.None);
                                    foreach (var packet in splitPacketReceived)
                                    {
                                        if (!string.IsNullOrEmpty(packet))
                                        {
                                            FilteringPacket(packet);
                                        }
                                    }
                                }
                                else
                                {
                                    FilteringPacket(packetReceived);
                                }

                                if (packetReceived != ClassSeedNodeStatus.SeedNone)
                                {
                                    packetReceived = ClassAlgo.GetEncryptedResult(ClassAlgoEnumeration.Rijndael,
                                        packetReceived,
                                        ClassConnectorSetting.MAJOR_UPDATE_1_SECURITY_CERTIFICATE_SIZE, _seedNodeConnector.AesIvCertificate, _seedNodeConnector.AesSaltCertificate);
                                    if (!await _apiObjectConnection.SendPacketAsync(packetReceived))
                                    {
                                        break;
                                    }
                                }
                            }
                            catch
                            {
                                break;
                            }
                        }

                        ConnectionAlive = false;

                    }, _apiObjectConnection.CancellationTokenApi.Token,
                    TaskCreationOptions.LongRunning, TaskScheduler.Current).ConfigureAwait(false);
            }
            catch
            {
                ConnectionAlive = false;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Filtering packets received from the network.
        /// </summary>
        /// <param name="packet"></param>
        private void FilteringPacket(string packet)
        {
            packet = packet.Replace(ClassConnectorSetting.PacketSplitSeperator, "");

            switch (packet)
            {
                case ClassWalletCommand.ClassWalletReceiveEnumeration.WalletInvalidPacket:
                case ClassWalletCommand.ClassWalletReceiveEnumeration.WalletInvalidAsk:
                    ClassApiBan.FilterInsertIp(_apiObjectConnection.Ip);
                    ClassApiBan.FilterInsertInvalidPacket(_apiObjectConnection.Ip);
                    break;
                case ClassSeedNodeStatus.SeedError:
                case ClassWalletCommand.ClassWalletReceiveEnumeration.DisconnectPacket:
                    ConnectionAlive = false;
                    break;
                case ClassSeedNodeStatus.SeedNone:
                    _maxNonePacket++;
                    if (_maxNonePacket >= ClapiApiProxyLimitation.MaxNonePacket)
                    {
                        ConnectionAlive = false;
                    }
                    break;
            }

        }


    }
}
