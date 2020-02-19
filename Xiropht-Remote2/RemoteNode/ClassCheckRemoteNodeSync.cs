using System;
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
        private static int ThreadLoopCheckRemoteNodeInterval = 5 * 1000; // Check every 5 seconds.
        private static int ThreadLoopCheckBlockchainNetworkInterval = 60 * 1000; // Check every 60 seconds.
        private static int _totalBlockchainNetworkError = 0;
        private const int MaxTotalBlockchainNetworkError = 5;
        public static bool BlockchainNetworkStatus;
        private static CancellationTokenSource _cancellationTokenSourceCheckRemoteNode;

        /// <summary>
        /// Check every remote node object sync connection.
        /// </summary>
        public static void EnableCheckRemoteNodeSync()
        {
            try
            {
                Task.Factory.StartNew(async delegate()
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

                                    await Task.Delay(1000);
                                }

                                await Program.RemoteNodeObjectBlock.StopConnection(
                                    ClassRemoteNodeObjectStopConnectionEnumeration
                                        .Reconnect);
                                await Task.Factory.StartNew(() => Program.RemoteNodeObjectBlock.StartConnectionAsync())
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

                                        await Task.Delay(1000);
                                    }

                                    await Program.RemoteNodeObjectBlock.StopConnection(
                                        ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                    await Task.Factory
                                        .StartNew(() => Program.RemoteNodeObjectBlock.StartConnectionAsync())
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

                                    await Task.Delay(1000);
                                }

                                await Program.RemoteNodeObjectCoinCirculating.StopConnection(
                                    ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                await Task.Factory
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
                                        await Task.Delay(1000);
                                        if (Program.Closed)
                                        {
                                            break;
                                        }
                                    }

                                    await Program.RemoteNodeObjectCoinCirculating.StopConnection(
                                        ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                    await Task.Factory
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

                                    await Task.Delay(1000);
                                }

                                await Program.RemoteNodeObjectCoinMaxSupply.StopConnection(
                                    ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                await Task.Factory
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

                                        await Task.Delay(1000);
                                    }

                                    await Program.RemoteNodeObjectCoinMaxSupply.StopConnection(
                                        ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                    await Task.Factory
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

                                    await Task.Delay(1000);
                                }

                                await Program.RemoteNodeObjectCurrentDifficulty.StopConnection(
                                    ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                await Task.Factory
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

                                        await Task.Delay(1000);
                                    }

                                    await Program.RemoteNodeObjectCurrentDifficulty.StopConnection(
                                        ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                    await Task.Factory
                                        .StartNew(
                                            () => Program.RemoteNodeObjectCurrentDifficulty.StartConnectionAsync())
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

                                    await Task.Delay(1000);
                                }

                                await Program.RemoteNodeObjectCurrentRate.StopConnection(
                                    ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                await Task.Factory
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

                                        await Task.Delay(1000);
                                    }

                                    await Program.RemoteNodeObjectCurrentRate.StopConnection(
                                        ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                    await Task.Factory
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

                                    await Task.Delay(1000);
                                }

                                await Program.RemoteNodeObjectTotalBlockMined.StopConnection(
                                    ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                await Task.Factory
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

                                        await Task.Delay(1000);
                                    }

                                    await Program.RemoteNodeObjectTotalBlockMined.StopConnection(
                                        ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                    await Task.Factory
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

                                    await Task.Delay(1000);
                                }

                                await Program.RemoteNodeObjectTotalFee.StopConnection(
                                    ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                await Task.Factory
                                    .StartNew(() => Program.RemoteNodeObjectTotalFee.StartConnectionAsync())
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

                                        await Task.Delay(1000);
                                    }

                                    await Program.RemoteNodeObjectTotalFee.StopConnection(
                                        ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                    await Task.Factory
                                        .StartNew(() => Program.RemoteNodeObjectTotalFee.StartConnectionAsync())
                                        .ConfigureAwait(false);

                                    if (Program.Closed)
                                    {
                                        break;
                                    }
                                }
                            }

                            if (!Program.RemoteNodeObjectTotalPendingTransaction.RemoteNodeObjectConnectionStatus ||
                                !Program.RemoteNodeObjectTotalPendingTransaction.RemoteNodeObjectTcpClient
                                    .ReturnStatus())
                            {
                                while (!BlockchainNetworkStatus)
                                {
                                    if (Program.Closed)
                                    {
                                        break;
                                    }

                                    await Task.Delay(1000);
                                }

                                await Program.RemoteNodeObjectTotalPendingTransaction.StopConnection(
                                    ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                await Task.Factory
                                    .StartNew(() =>
                                        Program.RemoteNodeObjectTotalPendingTransaction.StartConnectionAsync())
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

                                        await Task.Delay(1000);
                                    }

                                    await Program.RemoteNodeObjectTotalPendingTransaction.StopConnection(
                                        ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                    await Task.Factory.StartNew(() =>
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

                                    await Task.Delay(1000);
                                }

                                await Program.RemoteNodeObjectTransaction.StopConnection(
                                    ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                await Task.Factory
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

                                        await Task.Delay(1000);
                                    }

                                    await Program.RemoteNodeObjectTransaction.StopConnection(
                                        ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                    await Task.Factory
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

                                    await Task.Delay(1000);
                                }


                                await Program.RemoteNodeObjectTotalTransaction.StopConnection(
                                    ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                await Task.Factory
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

                                        await Task.Delay(1000);
                                    }

                                    await Program.RemoteNodeObjectTotalTransaction.StopConnection(
                                        ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                    await Task.Factory
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

                                        await Task.Delay(1000);
                                    }

                                    ClassRemoteNodeSync.ImPublicNode = false;
                                    ClassRemoteNodeSync.ListOfPublicNodes.Clear();
                                    ClassRemoteNodeSync.MyOwnIP = string.Empty;
                                    await Program.RemoteNodeObjectToBePublic.StopConnection(
                                        ClassRemoteNodeObjectStopConnectionEnumeration.Reconnect);
                                    await Task.Factory
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
                }, _cancellationTokenSourceCheckRemoteNode.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current).ConfigureAwait(false);
            }
            catch
            {
                // Catch the exception once the task is cancelled.
            }
        }

        /// <summary>
        /// Disable check of remote node sync.
        /// </summary>
        public static void DisableCheckRemoteNodeSync()
        {
            try
            {
                if (_cancellationTokenSourceCheckRemoteNode != null)
                {
                    if (!_cancellationTokenSourceCheckRemoteNode.IsCancellationRequested)
                    {
                        _cancellationTokenSourceCheckRemoteNode.Cancel();
                    }
                }
            }
            catch
            {
                // Ignored.
            }
        }

        /// <summary>
        /// Check the blockchain network status.
        /// </summary>
        public static void AutoCheckBlockchainNetwork()
        {
            _cancellationTokenSourceCheckRemoteNode = new CancellationTokenSource();

            try
            {
                Task.Factory.StartNew(async delegate
                {
                    while (!Program.Closed)
                    {
                        if (Program.Closed)
                        {
                            break;
                        }
                        await CheckBlockchainNetwork();

                        await Task.Delay(ThreadLoopCheckBlockchainNetworkInterval);
                    }
                }, _cancellationTokenSourceCheckRemoteNode.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current).ConfigureAwait(false);
            }
            catch
            {
                // Catch the exception once the task is cancelled.
            }
        }


        /// <summary>
        /// Check blockchain connection.
        /// </summary>
        /// <returns></returns>
        public static async Task CheckBlockchainNetwork()
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

                if (!testNetwork)
                {
                    _totalBlockchainNetworkError++;
                    if (_totalBlockchainNetworkError >= MaxTotalBlockchainNetworkError)
                    {
                        BlockchainNetworkStatus = false;
                        await Program.RemoteNodeObjectBlock.StopConnection(string.Empty);
                        await Program.RemoteNodeObjectTransaction.StopConnection(string.Empty);
                        await Program.RemoteNodeObjectTotalTransaction.StopConnection(string.Empty);
                        await Program.RemoteNodeObjectCoinCirculating.StopConnection(string.Empty);
                        await Program.RemoteNodeObjectCoinMaxSupply.StopConnection(string.Empty);
                        await Program.RemoteNodeObjectCurrentDifficulty.StopConnection(string.Empty);
                        await Program.RemoteNodeObjectCurrentRate.StopConnection(string.Empty);
                        await Program.RemoteNodeObjectTotalBlockMined.StopConnection(string.Empty);
                        await Program.RemoteNodeObjectTotalFee.StopConnection(string.Empty);
                        await Program.RemoteNodeObjectTotalPendingTransaction.StopConnection(string.Empty);
                    }
                }
                else
                {
                    BlockchainNetworkStatus = true;
                    _totalBlockchainNetworkError = 0;
                }
            }
            catch
            {
                _totalBlockchainNetworkError++;
                if (_totalBlockchainNetworkError >= MaxTotalBlockchainNetworkError)
                {
                    BlockchainNetworkStatus = false;
                    await Program.RemoteNodeObjectBlock.StopConnection(string.Empty);
                    await Program.RemoteNodeObjectTransaction.StopConnection(string.Empty);
                    await Program.RemoteNodeObjectTotalTransaction.StopConnection(string.Empty);
                    await Program.RemoteNodeObjectCoinCirculating.StopConnection(string.Empty);
                    await Program.RemoteNodeObjectCoinMaxSupply.StopConnection(string.Empty);
                    await Program.RemoteNodeObjectCurrentDifficulty.StopConnection(string.Empty);
                    await Program.RemoteNodeObjectCurrentRate.StopConnection(string.Empty);
                    await Program.RemoteNodeObjectTotalBlockMined.StopConnection(string.Empty);
                    await Program.RemoteNodeObjectTotalFee.StopConnection(string.Empty);
                    await Program.RemoteNodeObjectTotalPendingTransaction.StopConnection(string.Empty);
                }
            }
        }
    }
}
