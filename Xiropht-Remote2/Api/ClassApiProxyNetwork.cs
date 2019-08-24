using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Xiropht_Connector_All.Seed;
using Xiropht_Connector_All.Setting;
using Xiropht_Connector_All.Utils;
using Xiropht_Connector_All.Wallet;

namespace Xiropht_RemoteNode.Api
{
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

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="walletAddress"></param>
        /// <param name="apiObjectConnection"></param>
        public ClassApiProxyNetwork(string walletAddress, ClassApiObjectConnection apiObjectConnection)
        {
            _walletAddress = walletAddress;
            _apiObjectConnection = apiObjectConnection;
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
                _seedNodeConnector = new ClassSeedNodeConnector();
                if (!await _seedNodeConnector.StartConnectToSeedAsync(string.Empty))
                {
                    ConnectionAlive = false;
                    return false;
                }

                _certificate = ClassUtils.GenerateCertificate();

                if (!await _seedNodeConnector.SendPacketToSeedNodeAsync(_certificate, string.Empty))
                {
                    ConnectionAlive = false;
                    return false;
                }

                if (!ListenNetwork())
                {
                    ConnectionAlive = false;
                    return false;
                }

                if (!await _seedNodeConnector.SendPacketToSeedNodeAsync(ClassConnectorSettingEnumeration.WalletLoginType + ClassConnectorSetting.PacketContentSeperator + _walletAddress, _certificate, false, true))
                {
                    ConnectionAlive = false;
                    return false;
                }

                await Task.Factory.StartNew(CheckNetworkProxyStatus, _apiObjectConnection.CancellationTokenApi.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current).ConfigureAwait(false);
            }
            catch
            {
                ConnectionAlive = false;
                return false;
            }


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
                return await _seedNodeConnector.SendPacketToSeedNodeAsync(packet, _certificate, false, true);
            }
            catch
            {
                return false;
            }
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
                                if (packetReceived.Contains(ClassConnectorSetting.PacketSplitSeperator))
                                {
                                    var splitPacketReceived = packetReceived.Split(
                                        new[] {ClassConnectorSetting.PacketSplitSeperator},
                                        StringSplitOptions.None);
                                    foreach (var packet in splitPacketReceived)
                                    {
                                        if (!string.IsNullOrEmpty(packet))
                                        {
                                            if (!FilteringPacket(packet))
                                            {
                                                break;
                                            }

                                            if (!await _apiObjectConnection.SendPacketAsync(packet))
                                            {
                                                break;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    if (!FilteringPacket(packetReceived))
                                    {
                                        break;
                                    }
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


        private bool FilteringPacket(string packet)
        {
            packet = packet.Replace(ClassConnectorSetting.PacketSplitSeperator, "");

            switch (packet)
            {
                case ClassWalletCommand.ClassWalletReceiveEnumeration.WalletInvalidPacket:
                case ClassWalletCommand.ClassWalletReceiveEnumeration.WalletInvalidAsk:
                    ClassApiBan.FilterInsertIp(_apiObjectConnection.Ip);
                    ClassApiBan.FilterInsertInvalidPacket(_apiObjectConnection.Ip);
                    return false;
                case ClassWalletCommand.ClassWalletReceiveEnumeration.DisconnectPacket:
                    return false;
            }

            return true;
        }


    }
}
