using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Xiropht_Connector_All.Setting;
using Xiropht_Connector_All.Utils;
using Xiropht_RemoteNode.Data;

namespace Xiropht_RemoteNode.RemoteNode
{
    public class ClassRemoteNodeObjectStopConnectionEnumeration
    {
        public const string Reconnect = "reconnect";
        public const string Timeout = "timeout";
        public const string End = "end";
    }

    public class ClassCheckRemoteNodeSync
    {
        private static Thread _threadLoopCheckRemoteNode;
        private static int ThreadLoopCheckRemoteNodeInterval = 5 * 1000; // Check every 5 seconds.
        private static int ThreadLoopCheckBlockchainNetworkInterval = 60 * 1000; // Check every 60 seconds.
        public static bool BlockchainNetworkStatus;

        /// <summary>
        /// Check every remote node object sync connection.
        /// </summary>
        public static void EnableCheckRemoteNodeSync()
        {
            _threadLoopCheckRemoteNode = new Thread(delegate ()
            {
                while (!Program.Closed)
                {
                    Thread.Sleep(ThreadLoopCheckRemoteNodeInterval); // Make a pause for the next check.

                    try
                    {
                        if (!Program.RemoteNodeObjectBlock.RemoteNodeObjectConnectionStatus ||
                            !Program.RemoteNodeObjectBlock.RemoteNodeObjectTcpClient.ReturnStatus())
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }

                                Thread.Sleep(1000);
                            }

                            Program.RemoteNodeObjectBlock.StopConnection(ClassRemoteNodeObjectStopConnectionEnumeration
                                .Reconnect);
                            Task.Factory.StartNew(() => Program.RemoteNodeObjectBlock.StartConnectionAsync())
                                .ConfigureAwait(false);

                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                        else
                        {
                            var lastPacketReceivedTimeStamp =
                                Program.RemoteNodeObjectBlock.RemoteNodeObjectLastPacketReceived;
                            var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                            if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse <
                                currentTimestamp)
                            {
                                while (!BlockchainNetworkStatus)
                                {
                                    if (Program.Closed)
                                    {
                                        break;
                                    }

                                    Thread.Sleep(1000);
                                }

                                Program.RemoteNodeObjectBlock.StopConnection(
                                    ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                Task.Factory.StartNew(() => Program.RemoteNodeObjectBlock.StartConnectionAsync())
                                    .ConfigureAwait(false);
                                if (Program.Closed)
                                {
                                    break;
                                }
                            }
                        }

                        if (!Program.RemoteNodeObjectCoinCirculating.RemoteNodeObjectConnectionStatus ||
                            !Program.RemoteNodeObjectCoinCirculating.RemoteNodeObjectTcpClient.ReturnStatus())
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }

                                Thread.Sleep(1000);
                            }

                            Program.RemoteNodeObjectCoinCirculating.StopConnection(
                                ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                            Task.Factory
                                .StartNew(() => Program.RemoteNodeObjectCoinCirculating.StartConnectionAsync())
                                .ConfigureAwait(false);

                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                        else
                        {

                            var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectCoinCirculating
                                .RemoteNodeObjectLastPacketReceived;
                            var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                            if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse <
                                currentTimestamp)
                            {
                                while (!BlockchainNetworkStatus)
                                {
                                    Thread.Sleep(1000);
                                    if (Program.Closed)
                                    {
                                        break;
                                    }
                                }

                                Program.RemoteNodeObjectCoinCirculating.StopConnection(
                                    ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                Task.Factory
                                    .StartNew(() => Program.RemoteNodeObjectCoinCirculating.StartConnectionAsync())
                                    .ConfigureAwait(false);

                                if (Program.Closed)
                                {
                                    break;
                                }
                            }
                        }

                        if (!Program.RemoteNodeObjectCoinMaxSupply.RemoteNodeObjectConnectionStatus ||
                            !Program.RemoteNodeObjectCoinMaxSupply.RemoteNodeObjectTcpClient.ReturnStatus())
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }

                                Thread.Sleep(1000);
                            }

                            Program.RemoteNodeObjectCoinMaxSupply.StopConnection(
                                ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                            Task.Factory
                                .StartNew(() => Program.RemoteNodeObjectCoinMaxSupply.StartConnectionAsync())
                                .ConfigureAwait(false);

                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                        else
                        {
                            var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectCoinMaxSupply
                                .RemoteNodeObjectLastPacketReceived;
                            var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                            if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse <
                                currentTimestamp)
                            {
                                while (!BlockchainNetworkStatus)
                                {
                                    if (Program.Closed)
                                    {
                                        break;
                                    }

                                    Thread.Sleep(1000);
                                }

                                Program.RemoteNodeObjectCoinMaxSupply.StopConnection(
                                    ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                Task.Factory
                                    .StartNew(() => Program.RemoteNodeObjectCoinMaxSupply.StartConnectionAsync())
                                    .ConfigureAwait(false);

                                if (Program.Closed)
                                {
                                    break;
                                }
                            }
                        }

                        if (!Program.RemoteNodeObjectCurrentDifficulty.RemoteNodeObjectConnectionStatus ||
                            !Program.RemoteNodeObjectCurrentDifficulty.RemoteNodeObjectTcpClient.ReturnStatus())
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }

                                Thread.Sleep(1000);
                            }

                            Program.RemoteNodeObjectCurrentDifficulty.StopConnection(
                                ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                            Task.Factory
                                .StartNew(() => Program.RemoteNodeObjectCurrentDifficulty.StartConnectionAsync())
                                .ConfigureAwait(false);

                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                        else
                        {
                            var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectCurrentDifficulty
                                .RemoteNodeObjectLastPacketReceived;
                            var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                            if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse <
                                currentTimestamp)
                            {
                                while (!BlockchainNetworkStatus)
                                {
                                    if (Program.Closed)
                                    {
                                        break;
                                    }

                                    Thread.Sleep(1000);
                                }

                                Program.RemoteNodeObjectCurrentDifficulty.StopConnection(
                                    ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                Task.Factory
                                    .StartNew(() => Program.RemoteNodeObjectCurrentDifficulty.StartConnectionAsync())
                                    .ConfigureAwait(false);

                                if (Program.Closed)
                                {
                                    break;
                                }
                            }
                        }

                        if (!Program.RemoteNodeObjectCurrentRate.RemoteNodeObjectConnectionStatus ||
                            !Program.RemoteNodeObjectCurrentRate.RemoteNodeObjectTcpClient.ReturnStatus())
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }

                                Thread.Sleep(1000);
                            }

                            Program.RemoteNodeObjectCurrentRate.StopConnection(
                                ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                            Task.Factory
                                .StartNew(() => Program.RemoteNodeObjectCurrentRate.StartConnectionAsync())
                                .ConfigureAwait(false);

                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                        else
                        {
                            var lastPacketReceivedTimeStamp =
                                Program.RemoteNodeObjectCurrentRate.RemoteNodeObjectLastPacketReceived;
                            var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                            if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse <
                                currentTimestamp)
                            {
                                while (!BlockchainNetworkStatus)
                                {
                                    if (Program.Closed)
                                    {
                                        break;
                                    }

                                    Thread.Sleep(1000);
                                }

                                Program.RemoteNodeObjectCurrentRate.StopConnection(
                                    ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                Task.Factory
                                    .StartNew(() => Program.RemoteNodeObjectCurrentRate.StartConnectionAsync())
                                    .ConfigureAwait(false);

                                if (Program.Closed)
                                {
                                    break;
                                }
                            }
                        }

                        if (!Program.RemoteNodeObjectTotalBlockMined.RemoteNodeObjectConnectionStatus ||
                            !Program.RemoteNodeObjectTotalBlockMined.RemoteNodeObjectTcpClient.ReturnStatus())
                        {
                            while (!BlockchainNetworkStatus)
                            {

                                if (Program.Closed)
                                {
                                    break;
                                }

                                Thread.Sleep(1000);
                            }

                            Program.RemoteNodeObjectTotalBlockMined.StopConnection(
                                ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                            Task.Factory
                                .StartNew(() => Program.RemoteNodeObjectTotalBlockMined.StartConnectionAsync())
                                .ConfigureAwait(false);
                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                        else
                        {
                            var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectTotalBlockMined
                                .RemoteNodeObjectLastPacketReceived;
                            var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                            if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse <
                                currentTimestamp)
                            {
                                while (!BlockchainNetworkStatus)
                                {
                                    if (Program.Closed)
                                    {
                                        break;
                                    }

                                    Thread.Sleep(1000);
                                }

                                Program.RemoteNodeObjectTotalBlockMined.StopConnection(
                                    ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                Task.Factory
                                    .StartNew(() => Program.RemoteNodeObjectTotalBlockMined.StartConnectionAsync())
                                    .ConfigureAwait(false);

                                if (Program.Closed)
                                {
                                    break;
                                }
                            }
                        }

                        if (!Program.RemoteNodeObjectTotalFee.RemoteNodeObjectConnectionStatus ||
                            !Program.RemoteNodeObjectTotalFee.RemoteNodeObjectTcpClient.ReturnStatus())
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }

                                Thread.Sleep(1000);
                            }

                            Program.RemoteNodeObjectTotalFee.StopConnection(
                                ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                            Task.Factory.StartNew(() => Program.RemoteNodeObjectTotalFee.StartConnectionAsync())
                                .ConfigureAwait(false);

                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                        else
                        {
                            var lastPacketReceivedTimeStamp =
                                Program.RemoteNodeObjectTotalFee.RemoteNodeObjectLastPacketReceived;
                            var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                            if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse <
                                currentTimestamp)
                            {
                                while (!BlockchainNetworkStatus)
                                {
                                    if (Program.Closed)
                                    {
                                        break;
                                    }

                                    Thread.Sleep(1000);
                                }

                                Program.RemoteNodeObjectTotalFee.StopConnection(
                                    ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                Task.Factory
                                    .StartNew(() => Program.RemoteNodeObjectTotalFee.StartConnectionAsync())
                                    .ConfigureAwait(false);

                                if (Program.Closed)
                                {
                                    break;
                                }
                            }
                        }

                        if (!Program.RemoteNodeObjectTotalPendingTransaction.RemoteNodeObjectConnectionStatus ||
                            !Program.RemoteNodeObjectTotalPendingTransaction.RemoteNodeObjectTcpClient.ReturnStatus())
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }

                                Thread.Sleep(1000);
                            }

                            Program.RemoteNodeObjectTotalPendingTransaction.StopConnection(
                                ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                            Task.Factory
                                .StartNew(() => Program.RemoteNodeObjectTotalPendingTransaction.StartConnectionAsync())
                                .ConfigureAwait(false);

                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                        else
                        {
                            var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectTotalPendingTransaction
                                .RemoteNodeObjectLastPacketReceived;
                            var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                            if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse <
                                currentTimestamp)
                            {
                                while (!BlockchainNetworkStatus)
                                {
                                    if (Program.Closed)
                                    {
                                        break;
                                    }

                                    Thread.Sleep(1000);
                                }

                                Program.RemoteNodeObjectTotalPendingTransaction.StopConnection(
                                    ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                Task.Factory.StartNew(() =>
                                        Program.RemoteNodeObjectTotalPendingTransaction.StartConnectionAsync())
                                    .ConfigureAwait(false);

                                if (Program.Closed)
                                {
                                    break;
                                }
                            }
                        }

                        if (!Program.RemoteNodeObjectTransaction.RemoteNodeObjectConnectionStatus ||
                            !Program.RemoteNodeObjectTransaction.RemoteNodeObjectTcpClient.ReturnStatus())
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }

                                Thread.Sleep(1000);
                            }

                            Program.RemoteNodeObjectTransaction.StopConnection(
                                ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                            Task.Factory
                                .StartNew(() => Program.RemoteNodeObjectTransaction.StartConnectionAsync())
                                .ConfigureAwait(false);

                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                        else
                        {
                            var lastPacketReceivedTimeStamp =
                                Program.RemoteNodeObjectTransaction.RemoteNodeObjectLastPacketReceived;
                            var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                            if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse <
                                currentTimestamp)
                            {
                                while (!BlockchainNetworkStatus)
                                {
                                    if (Program.Closed)
                                    {
                                        break;
                                    }

                                    Thread.Sleep(1000);
                                }

                                Program.RemoteNodeObjectTransaction.StopConnection(
                                    ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                Task.Factory
                                    .StartNew(() => Program.RemoteNodeObjectTransaction.StartConnectionAsync())
                                    .ConfigureAwait(false);

                                if (Program.Closed)
                                {
                                    break;
                                }
                            }
                        }

                        if (!Program.RemoteNodeObjectTotalTransaction.RemoteNodeObjectConnectionStatus ||
                            !Program.RemoteNodeObjectTotalTransaction.RemoteNodeObjectTcpClient.ReturnStatus())
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }

                                Thread.Sleep(1000);
                            }


                            Program.RemoteNodeObjectTotalTransaction.StopConnection(
                                ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                            Task.Factory
                                .StartNew(() => Program.RemoteNodeObjectTotalTransaction.StartConnectionAsync())
                                .ConfigureAwait(false);

                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                        else
                        {


                            var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectTotalTransaction
                                .RemoteNodeObjectLastPacketReceived;
                            var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                            if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse <
                                currentTimestamp)
                            {
                                while (!BlockchainNetworkStatus)
                                {
                                    if (Program.Closed)
                                    {
                                        break;
                                    }

                                    Thread.Sleep(1000);
                                }

                                Program.RemoteNodeObjectTotalTransaction.StopConnection(
                                    ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                Task.Factory
                                    .StartNew(() => Program.RemoteNodeObjectTotalTransaction.StartConnectionAsync())
                                    .ConfigureAwait(false);

                                if (Program.Closed)
                                {
                                    break;
                                }
                            }
                        }

                        if (ClassRemoteNodeSync.WantToBePublicNode)
                        {
                            if (!Program.RemoteNodeObjectToBePublic.RemoteNodeObjectConnectionStatus ||
                                !Program.RemoteNodeObjectToBePublic.RemoteNodeObjectTcpClient.ReturnStatus())
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
                                Program.RemoteNodeObjectToBePublic.StopConnection(
                                    ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                Task.Factory
                                    .StartNew(() => Program.RemoteNodeObjectToBePublic.StartConnectionAsync())
                                    .ConfigureAwait(false);
                                if (Program.Closed)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    catch
                    {
                        // Ignored
                    }
                }
            });
            _threadLoopCheckRemoteNode.Start();
        }

        public static void DisableCheckRemoteNodeSync()
        {
            if (_threadLoopCheckRemoteNode != null && (_threadLoopCheckRemoteNode.IsAlive || _threadLoopCheckRemoteNode != null))
            {
                _threadLoopCheckRemoteNode.Abort();
                GC.SuppressFinalize(_threadLoopCheckRemoteNode);
            }
        }

        public static void AutoCheckBlockchainNetwork()
        {
            var threadCheckBlockchainNetwork = new Thread(delegate ()
            {
                while (!Program.Closed)
                {
                    if (Program.Closed)
                    {
                        break;
                    }
                    CheckBlockchainNetwork();
                   
                    Thread.Sleep(ThreadLoopCheckBlockchainNetworkInterval);
                }
            });
            threadCheckBlockchainNetwork.Start();
        }


        /// <summary>
        /// Check blockchain connection.
        /// </summary>
        /// <returns></returns>
        public static void CheckBlockchainNetwork()
        {
            try
            {
                bool testNetwork = false;
                foreach (var seedNode in ClassConnectorSetting.SeedNodeIp.ToArray())
                {
                    if (!testNetwork)
                    {
                        Task taskCheckSeedNode = Task.Run(async () =>
                            testNetwork =
                                await CheckTcp.CheckTcpClientAsync(seedNode.Key,
                                    ClassConnectorSetting.SeedNodeTokenPort));
                        taskCheckSeedNode.Wait(ClassConnectorSetting.MaxTimeoutConnect);
                        if (testNetwork) break;

                    }
                }

                BlockchainNetworkStatus = testNetwork;
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
