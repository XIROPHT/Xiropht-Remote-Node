using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Xiropht_Connector_All.Setting;
using Xiropht_Connector_All.Utils;
using Xiropht_RemoteNode.Data;
using Xiropht_RemoteNode.Log;

namespace Xiropht_RemoteNode.RemoteNode
{
    public class ClassCheckRemoteNodeSync
    {
        private static Thread ThreadLoopCheckRemoteNode;
        private static int ThreadLoopCheckRemoteNodeInterval = 5 * 1000; // Check every 5 seconds.
        private static int ThreadLoopCheckBlockchainNetworkInterval = 60 * 1000; // Check every 60 seconds.
        public static bool BlockchainNetworkStatus;

        /// <summary>
        /// Check every remote node object sync connection.
        /// </summary>
        public static void EnableCheckRemoteNodeSync()
        {
            ThreadLoopCheckRemoteNode = new Thread(async delegate ()
            {
                while (!Program.Closed)
                {
                    Thread.Sleep(ThreadLoopCheckRemoteNodeInterval); // Make a pause for the next check.

                    if (!Program.RemoteNodeObjectBlock.RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectBlock.RemoteNodeObjectTcpClient.ReturnStatus())
                    {
                        while (!BlockchainNetworkStatus)
                        {
                            if (Program.Closed)
                            {
                                break;
                            }
                            Thread.Sleep(1000);
                        }
                        Program.RemoteNodeObjectBlock.StopConnection("reconnect");
                        await Task.Factory.StartNew(() => Program.RemoteNodeObjectBlock.StartConnectionAsync()).ConfigureAwait(false);

                        if (Program.Closed)
                        {
                            break;
                        }
                    }
                    else
                    {
                        var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectBlock.RemoteNodeObjectLastPacketReceived;
                        var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse < currentTimestamp)
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }
                                Thread.Sleep(1000);
                            }

                            Program.RemoteNodeObjectBlock.StopConnection("reconnect");
                            await Task.Factory.StartNew(() => Program.RemoteNodeObjectBlock.StartConnectionAsync()).ConfigureAwait(false);
                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                    }

                    if (!Program.RemoteNodeObjectCoinCirculating.RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectCoinCirculating.RemoteNodeObjectTcpClient.ReturnStatus())
                    {
                        while (!BlockchainNetworkStatus)
                        {
                            if (Program.Closed)
                            {
                                break;
                            }
                            Thread.Sleep(1000);
                        }

                        Program.RemoteNodeObjectCoinCirculating.StopConnection("reconnect");
                        await Task.Factory.StartNew(() => Program.RemoteNodeObjectCoinCirculating.StartConnectionAsync()).ConfigureAwait(false);

                        if (Program.Closed)
                        {
                            break;
                        }
                    }
                    else
                    {

                        var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectCoinCirculating.RemoteNodeObjectLastPacketReceived;
                        var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse < currentTimestamp)
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                Thread.Sleep(1000);
                                if (Program.Closed)
                                {
                                    break;
                                }
                            }

                            Program.RemoteNodeObjectCoinCirculating.StopConnection("reconnect");
                            await Task.Factory.StartNew(() => Program.RemoteNodeObjectCoinCirculating.StartConnectionAsync()).ConfigureAwait(false);

                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                    }

                    if (!Program.RemoteNodeObjectCoinMaxSupply.RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectCoinMaxSupply.RemoteNodeObjectTcpClient.ReturnStatus())
                    {
                        while (!BlockchainNetworkStatus)
                        {
                            if (Program.Closed)
                            {
                                break;
                            }
                            Thread.Sleep(1000);
                        }
                        Program.RemoteNodeObjectCoinMaxSupply.StopConnection("reconnect");
                        await Task.Factory.StartNew(() => Program.RemoteNodeObjectCoinMaxSupply.StartConnectionAsync()).ConfigureAwait(false);

                        if (Program.Closed)
                        {
                            break;
                        }
                    }
                    else
                    {
                        var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectCoinMaxSupply.RemoteNodeObjectLastPacketReceived;
                        var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse < currentTimestamp)
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }
                                Thread.Sleep(1000);
                            }

                            Program.RemoteNodeObjectCoinMaxSupply.StopConnection("reconnect");
                            await Task.Factory.StartNew(() => Program.RemoteNodeObjectCoinMaxSupply.StartConnectionAsync()).ConfigureAwait(false);

                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                    }

                    if (!Program.RemoteNodeObjectCurrentDifficulty.RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectCurrentDifficulty.RemoteNodeObjectTcpClient.ReturnStatus())
                    {
                        while (!BlockchainNetworkStatus)
                        {
                            if (Program.Closed)
                            {
                                break;
                            }
                            Thread.Sleep(1000);
                        }

                        Program.RemoteNodeObjectCurrentDifficulty.StopConnection("reconnect");
                        await Task.Factory.StartNew(() => Program.RemoteNodeObjectCurrentDifficulty.StartConnectionAsync()).ConfigureAwait(false);

                        if (Program.Closed)
                        {
                            break;
                        }
                    }
                    else
                    {
                        var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectCurrentDifficulty.RemoteNodeObjectLastPacketReceived;
                        var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse < currentTimestamp)
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }
                                Thread.Sleep(1000);
                            }

                            Program.RemoteNodeObjectCurrentDifficulty.StopConnection("reconnect");
                            await Task.Factory.StartNew(() => Program.RemoteNodeObjectCurrentDifficulty.StartConnectionAsync()).ConfigureAwait(false);

                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                    }

                    if (!Program.RemoteNodeObjectCurrentRate.RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectCurrentRate.RemoteNodeObjectTcpClient.ReturnStatus())
                    {
                        while (!BlockchainNetworkStatus)
                        {
                            if (Program.Closed)
                            {
                                break;
                            }
                            Thread.Sleep(1000);
                        }

                        Program.RemoteNodeObjectCurrentRate.StopConnection("reconnect");
                        await Task.Factory.StartNew(() => Program.RemoteNodeObjectCurrentRate.StartConnectionAsync()).ConfigureAwait(false);

                        if (Program.Closed)
                        {
                            break;
                        }
                    }
                    else
                    {
                        var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectCurrentRate.RemoteNodeObjectLastPacketReceived;
                        var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse < currentTimestamp)
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }
                                Thread.Sleep(1000);
                            }

                            Program.RemoteNodeObjectCurrentRate.StopConnection("reconnect");
                            await Task.Factory.StartNew(() => Program.RemoteNodeObjectCurrentRate.StartConnectionAsync()).ConfigureAwait(false);

                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                    }

                    if (!Program.RemoteNodeObjectTotalBlockMined.RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectTotalBlockMined.RemoteNodeObjectTcpClient.ReturnStatus())
                    {
                        while (!BlockchainNetworkStatus)
                        {

                            if (Program.Closed)
                            {
                                break;
                            }
                            Thread.Sleep(1000);
                        }

                        Program.RemoteNodeObjectTotalBlockMined.StopConnection("reconnect");
                        await Task.Factory.StartNew(() => Program.RemoteNodeObjectTotalBlockMined.StartConnectionAsync()).ConfigureAwait(false);
                        if (Program.Closed)
                        {
                            break;
                        }
                    }
                    else
                    {
                        var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectTotalBlockMined.RemoteNodeObjectLastPacketReceived;
                        var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse < currentTimestamp)
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }
                                Thread.Sleep(1000);
                            }

                            Program.RemoteNodeObjectTotalBlockMined.StopConnection("reconnect");
                            await Task.Factory.StartNew(() => Program.RemoteNodeObjectTotalBlockMined.StartConnectionAsync()).ConfigureAwait(false);

                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                    }

                    if (!Program.RemoteNodeObjectTotalFee.RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectTotalFee.RemoteNodeObjectTcpClient.ReturnStatus())
                    {
                        while (!BlockchainNetworkStatus)
                        {
                            if (Program.Closed)
                            {
                                break;
                            }
                            Thread.Sleep(1000);
                        }

                        Program.RemoteNodeObjectTotalFee.StopConnection("reconnect");
                        await Task.Factory.StartNew(() => Program.RemoteNodeObjectTotalFee.StartConnectionAsync()).ConfigureAwait(false);

                        if (Program.Closed)
                        {
                            break;
                        }
                    }
                    else
                    {
                        var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectTotalFee.RemoteNodeObjectLastPacketReceived;
                        var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse < currentTimestamp)
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }
                                Thread.Sleep(1000);
                            }

                            Program.RemoteNodeObjectTotalFee.StopConnection("reconnect");
                            await Task.Factory.StartNew(() => Program.RemoteNodeObjectTotalFee.StartConnectionAsync()).ConfigureAwait(false);

                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                    }

                    if (!Program.RemoteNodeObjectTotalPendingTransaction.RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectTotalPendingTransaction.RemoteNodeObjectTcpClient.ReturnStatus())
                    {
                        while (!BlockchainNetworkStatus)
                        {
                            if (Program.Closed)
                            {
                                break;
                            }
                            Thread.Sleep(1000);
                        }

                        Program.RemoteNodeObjectTotalPendingTransaction.StopConnection("reconnect");
                        await Task.Factory.StartNew(() => Program.RemoteNodeObjectTotalPendingTransaction.StartConnectionAsync()).ConfigureAwait(false);

                        if (Program.Closed)
                        {
                            break;
                        }
                    }
                    else
                    {
                        var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectTotalPendingTransaction.RemoteNodeObjectLastPacketReceived;
                        var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse < currentTimestamp)
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }
                                Thread.Sleep(1000);
                            }

                            Program.RemoteNodeObjectTotalPendingTransaction.StopConnection("reconnect");
                            await Task.Factory.StartNew(() => Program.RemoteNodeObjectTotalPendingTransaction.StartConnectionAsync()).ConfigureAwait(false);

                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                    }

                    if (!Program.RemoteNodeObjectTransaction.RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectTransaction.RemoteNodeObjectTcpClient.ReturnStatus())
                    {
                        while (!BlockchainNetworkStatus)
                        {
                            if (Program.Closed)
                            {
                                break;
                            }
                            Thread.Sleep(1000);
                        }

                        Program.RemoteNodeObjectTransaction.StopConnection("reconnect");
                        await Task.Factory.StartNew(() => Program.RemoteNodeObjectTransaction.StartConnectionAsync()).ConfigureAwait(false);

                        if (Program.Closed)
                        {
                            break;
                        }
                    }
                    else
                    {
                        var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectTransaction.RemoteNodeObjectLastPacketReceived;
                        var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse < currentTimestamp)
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }
                                Thread.Sleep(1000);
                            }

                            Program.RemoteNodeObjectTransaction.StopConnection("reconnect");
                            await Task.Factory.StartNew(() => Program.RemoteNodeObjectTransaction.StartConnectionAsync()).ConfigureAwait(false);

                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                    }

                    if (!Program.RemoteNodeObjectTotalTransaction.RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectTotalTransaction.RemoteNodeObjectTcpClient.ReturnStatus())
                    {
                        while (!BlockchainNetworkStatus)
                        {
                            if (Program.Closed)
                            {
                                break;
                            }
                            Thread.Sleep(1000);
                        }


                        Program.RemoteNodeObjectTotalTransaction.StopConnection("reconnect");
                        await Task.Factory.StartNew(() => Program.RemoteNodeObjectTotalTransaction.StartConnectionAsync()).ConfigureAwait(false);

                        if (Program.Closed)
                        {
                            break;
                        }
                    }
                    else
                    {


                        var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectTotalTransaction.RemoteNodeObjectLastPacketReceived;
                        var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse < currentTimestamp)
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }
                                Thread.Sleep(1000);
                            }

                            Program.RemoteNodeObjectTotalTransaction.StopConnection("reconnect");
                            await Task.Factory.StartNew(() => Program.RemoteNodeObjectTotalTransaction.StartConnectionAsync()).ConfigureAwait(false);

                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                    }

                    if (ClassRemoteNodeSync.WantToBePublicNode)
                    {
                        if (!Program.RemoteNodeObjectToBePublic.RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectToBePublic.RemoteNodeObjectTcpClient.ReturnStatus())
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }
                                Thread.Sleep(1000);
                            }
                            ClassRemoteNodeSync.ImPublicNode = false;
                            ClassRemoteNodeSync.ListOfPublicNodes.Clear();
                            ClassRemoteNodeSync.MyOwnIP = string.Empty;
                            Program.RemoteNodeObjectToBePublic.StopConnection("reconnect");
                            await Program.RemoteNodeObjectToBePublic.StartConnectionAsync();

                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                    }
                }
            });
            ThreadLoopCheckRemoteNode.Start();
        }

        public static void DisableCheckRemoteNodeSync()
        {
            if (ThreadLoopCheckRemoteNode != null && (ThreadLoopCheckRemoteNode.IsAlive || ThreadLoopCheckRemoteNode != null))
            {
                ThreadLoopCheckRemoteNode.Abort();
                GC.SuppressFinalize(ThreadLoopCheckRemoteNode);
            }
        }

        public static void AutoCheckBlockchainNetwork()
        {
            var threadCheckBlockchainNetwork = new Thread(async delegate ()
            {
                while (!Program.Closed)
                {
                    if (Program.Closed)
                    {
                        break;
                    }
                    await CheckBlockchainNetworkAsync();
                   
                    Thread.Sleep(ThreadLoopCheckBlockchainNetworkInterval);
                }
            });
            threadCheckBlockchainNetwork.Start();
        }


        public static async Task<bool> ConnectToTarget(TcpClient client, string host, int port)
        {

            var clientTask = client.ConnectAsync(host, port);
            var delayTask = Task.Delay(ClassConnectorSetting.MaxTimeoutConnect);

            var completedTask = await Task.WhenAny(new[] { clientTask, delayTask });
            return completedTask == clientTask;

        }

        /// <summary>
        /// Check blockchain connection.
        /// </summary>
        /// <returns></returns>
        public static async Task CheckBlockchainNetworkAsync()
        {
            try
            {
                var clientBlockchain = new TcpClient();
                if (await ConnectToTarget(clientBlockchain, ClassConnectorSetting.SeedNodeIp.Keys.ElementAt(ClassUtils.GetRandomBetween(0, ClassConnectorSetting.SeedNodeIp.Count - 1)), ClassConnectorSetting.SeedNodePort))
                {
                    BlockchainNetworkStatus = true;
                }
                else
                {
                    BlockchainNetworkStatus = false;
                }
                clientBlockchain?.Close();
                clientBlockchain?.Dispose();
            }
            catch
            {
                BlockchainNetworkStatus = false;
                Program.RemoteNodeObjectBlock.StopConnection(string.Empty);
                Program.RemoteNodeObjectTransaction.StopConnection(string.Empty);
                Program.RemoteNodeObjectTotalTransaction.StopConnection(string.Empty);
                Program.RemoteNodeObjectCoinCirculating.StopConnection(string.Empty);
                Program.RemoteNodeObjectCoinMaxSupply.StopConnection(string.Empty);
                Program.RemoteNodeObjectCurrentDifficulty.StopConnection(string.Empty);
                Program.RemoteNodeObjectCurrentRate.StopConnection(string.Empty);
                Program.RemoteNodeObjectTotalBlockMined.StopConnection(string.Empty);
                Program.RemoteNodeObjectTotalFee.StopConnection(string.Empty);
                Program.RemoteNodeObjectTotalPendingTransaction.StopConnection(string.Empty);
            }
        }
    }
}
