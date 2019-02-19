using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
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
                        ClassLog.Log("Object Sync Block disconnected, reconnect now..", 2, 3);
                        Program.RemoteNodeObjectBlock.StopConnection();
                        await Program.RemoteNodeObjectBlock.StartConnectionAsync();
                        if (Program.Closed)
                        {
                            break;
                        }
                    }
                    else
                    {
                        var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectBlock.RemoteNodeObjectLastPacketReceived;
                        var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse< currentTimestamp)
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }
                                Thread.Sleep(1000);
                            }
                            ClassLog.Log("Object Sync block disconnected, reconnect now..", 2, 3);

                            Program.RemoteNodeObjectBlock.StopConnection();
                            await Program.RemoteNodeObjectBlock.StartConnectionAsync();
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
                        ClassLog.Log("Object Sync Coin Circulating disconnected, reconnect now..", 2, 3);

                        Program.RemoteNodeObjectCoinCirculating.StopConnection();
                        await Program.RemoteNodeObjectCoinCirculating.StartConnectionAsync();

                        if (Program.Closed)
                        {
                            break;
                        }
                    }
                    else
                    {

                        var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectCoinCirculating.RemoteNodeObjectLastPacketReceived;
                        var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse< currentTimestamp)
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                Thread.Sleep(1000);
                                if (Program.Closed)
                                {
                                    break;
                                }
                            }
                            ClassLog.Log("Last packet from Object Coin Circulating more than 10 seconds ago, reconnect now..", 2, 3);

                            Program.RemoteNodeObjectCoinCirculating.StopConnection();
                            await Program.RemoteNodeObjectCoinCirculating.StartConnectionAsync();

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
                        ClassLog.Log("Object Sync Coin Max Supply disconnected, reconnect now..", 2, 3);

                        Program.RemoteNodeObjectCoinMaxSupply.StopConnection();
                        await Program.RemoteNodeObjectCoinMaxSupply.StartConnectionAsync();

                        if (Program.Closed)
                        {
                            break;
                        }
                    }
                    else
                    {
                        var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectCoinMaxSupply.RemoteNodeObjectLastPacketReceived;
                        var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse< currentTimestamp)
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }
                                Thread.Sleep(1000);
                            }
                            ClassLog.Log("Last packet from Object Sync Coin Max Supply more than 10 seconds ago, reconnect now..", 2, 3);

                            Program.RemoteNodeObjectCoinMaxSupply.StopConnection();
                            await Program.RemoteNodeObjectCoinMaxSupply.StartConnectionAsync();
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
                        ClassLog.Log("Object Sync Current Difficulty disconnected, reconnect now..", 2, 3);

                        Program.RemoteNodeObjectCurrentDifficulty.StopConnection();
                        await Program.RemoteNodeObjectCurrentDifficulty.StartConnectionAsync();

                        if (Program.Closed)
                        {
                            break;
                        }
                    }
                    else
                    {
                        var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectCurrentDifficulty.RemoteNodeObjectLastPacketReceived;
                        var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse< currentTimestamp)
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }
                                Thread.Sleep(1000);
                            }
                            ClassLog.Log("Last packet from Object Sync Current Difficulty more than 10 seconds ago, reconnect now..", 2, 3);

                            Program.RemoteNodeObjectCurrentDifficulty.StopConnection();
                            await Program.RemoteNodeObjectCurrentDifficulty.StartConnectionAsync();

                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                    }

                    if (!Program.RemoteNodeObjectCurrentRate.RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectCurrentDifficulty.RemoteNodeObjectTcpClient.ReturnStatus())
                    {
                        while (!BlockchainNetworkStatus)
                        {
                            if (Program.Closed)
                            {
                                break;
                            }
                            Thread.Sleep(1000);
                        }
                        ClassLog.Log("Object Sync Current Hashrate disconnected, reconnect now..", 2, 3);

                        Program.RemoteNodeObjectCurrentRate.StopConnection();
                        await Program.RemoteNodeObjectCurrentRate.StartConnectionAsync();

                        if (Program.Closed)
                        {
                            break;
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
                            ClassRemoteNodeSync.ListOfPublicNodes.Clear();
                            ClassLog.Log("Object Sync Want to be Public disconnected, reconnect now..", 2, 3);

                            Program.RemoteNodeObjectToBePublic.StopConnection();
                            await Program.RemoteNodeObjectToBePublic.StartConnectionAsync();

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
                        ClassLog.Log("Object Sync Total Block Mined disconnected, reconnect now..", 2, 3);

                        Program.RemoteNodeObjectTotalBlockMined.StopConnection();
                        await Program.RemoteNodeObjectTotalBlockMined.StartConnectionAsync();
                        if (Program.Closed)
                        {
                            break;
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
                        ClassLog.Log("Object Sync Total Fee disconnected, reconnect now..", 2, 3);

                        Program.RemoteNodeObjectTotalFee.StopConnection();
                        await Program.RemoteNodeObjectTotalFee.StartConnectionAsync();

                        if (Program.Closed)
                        {
                            break;
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
                        ClassLog.Log("Object Sync Total Pending Transaction disconnected, reconnect now..", 2, 3);

                        Program.RemoteNodeObjectTotalPendingTransaction.StopConnection();
                        await Program.RemoteNodeObjectTotalPendingTransaction.StartConnectionAsync();

                        if (Program.Closed)
                        {
                            break;
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
                        ClassLog.Log("Object Sync Transaction disconnected, reconnect now..", 2, 3);

                        Program.RemoteNodeObjectTransaction.StopConnection();
                        await Program.RemoteNodeObjectTransaction.StartConnectionAsync();

                        if (Program.Closed)
                        {
                            break;
                        }
                    }
                    else
                    {
                        var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectTransaction.RemoteNodeObjectLastPacketReceived;
                        var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse< currentTimestamp)
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }
                                Thread.Sleep(1000);
                            }
                            ClassLog.Log("Object Sync Transaction disconnected, reconnect now..", 2, 3);

                            Program.RemoteNodeObjectTransaction.StopConnection();
                            await Program.RemoteNodeObjectTransaction.StartConnectionAsync();

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

                        ClassLog.Log("Object Sync Total number of Transaction disconnected, reconnect now..", 2, 3);

                        Program.RemoteNodeObjectTotalTransaction.StopConnection();
                        await Program.RemoteNodeObjectTotalTransaction.StartConnectionAsync();

                        if (Program.Closed)
                        {
                            break;
                        }
                    }
                    else
                    {


                        var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectTotalTransaction.RemoteNodeObjectLastPacketReceived;
                        var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeWaitResponse< currentTimestamp)
                        {
                            while (!BlockchainNetworkStatus)
                            {
                                if (Program.Closed)
                                {
                                    break;
                                }
                                Thread.Sleep(1000);
                            }
                            ClassLog.Log("Last packet from Object Total number of Transaction more than 10 seconds ago, reconnect now..", 2, 3);

                            Program.RemoteNodeObjectTotalTransaction.StopConnection();
                            await Program.RemoteNodeObjectTotalTransaction.StartConnectionAsync();

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
                if (await ConnectToTarget(clientBlockchain, ClassConnectorSetting.SeedNodeIp[ClassUtils.GetRandomBetween(0, ClassConnectorSetting.SeedNodeIp.Count - 1)], ClassConnectorSetting.SeedNodePort))
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
                Program.RemoteNodeObjectBlock.StopConnection();
                Program.RemoteNodeObjectTransaction.StopConnection();
                Program.RemoteNodeObjectTotalTransaction.StopConnection();
                Program.RemoteNodeObjectCoinCirculating.StopConnection();
                Program.RemoteNodeObjectCoinMaxSupply.StopConnection();
                Program.RemoteNodeObjectCurrentDifficulty.StopConnection();
                Program.RemoteNodeObjectCurrentRate.StopConnection();
                Program.RemoteNodeObjectTotalBlockMined.StopConnection();
                Program.RemoteNodeObjectTotalFee.StopConnection();
                Program.RemoteNodeObjectTotalPendingTransaction.StopConnection();
            }
        }
    }
}
