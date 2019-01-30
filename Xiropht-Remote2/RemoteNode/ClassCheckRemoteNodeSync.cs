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

                    if (!Program.RemoteNodeObjectBlock.RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectBlock.RemoteNodeObjectTcpClient.GetStatusConnectToSeed())
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
                        if (lastPacketReceivedTimeStamp + 5 < currentTimestamp)
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
                    for (int i = 0; i < Program.RemoteNodeObjectCoinCirculating.Count; i++)
                    {
                        if (i < Program.RemoteNodeObjectCoinCirculating.Count)
                        {
                            if (!Program.RemoteNodeObjectCoinCirculating[i].RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectCoinCirculating[i].RemoteNodeObjectTcpClient.GetStatusConnectToSeed())
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

                                Program.RemoteNodeObjectCoinCirculating[i].StopConnection();
                                await Program.RemoteNodeObjectCoinCirculating[i].StartConnectionAsync();

                                if (Program.Closed)
                                {
                                    break;
                                }
                            }
                            else
                            {

                                var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectCoinCirculating[i].RemoteNodeObjectLastPacketReceived;
                                var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                                if (lastPacketReceivedTimeStamp + 5 < currentTimestamp)
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

                                    Program.RemoteNodeObjectCoinCirculating[i].StopConnection();
                                    await Program.RemoteNodeObjectCoinCirculating[i].StartConnectionAsync();

                                    if (Program.Closed)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    for (int i = 0; i < Program.RemoteNodeObjectCoinMaxSupply.Count; i++)
                    {
                        if (!Program.RemoteNodeObjectCoinMaxSupply[i].RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectCoinMaxSupply[i].RemoteNodeObjectTcpClient.GetStatusConnectToSeed())
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

                            Program.RemoteNodeObjectCoinMaxSupply[i].StopConnection();
                            await Program.RemoteNodeObjectCoinMaxSupply[i].StartConnectionAsync();

                            if (Program.Closed)
                            {
                                break;
                            }
                        }
                        else
                        {

                            var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectCoinMaxSupply[i].RemoteNodeObjectLastPacketReceived;
                            var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                            if (lastPacketReceivedTimeStamp + 5 < currentTimestamp)
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

                                Program.RemoteNodeObjectCoinMaxSupply[i].StopConnection();
                                await Program.RemoteNodeObjectCoinMaxSupply[i].StartConnectionAsync();
                                if (Program.Closed)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    for (int i = 0; i < Program.RemoteNodeObjectCurrentDifficulty.Count; i++)
                    {
                        if (i < Program.RemoteNodeObjectCurrentDifficulty.Count)
                        {
                            if (!Program.RemoteNodeObjectCurrentDifficulty[i].RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectCurrentDifficulty[i].RemoteNodeObjectTcpClient.GetStatusConnectToSeed())
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

                                Program.RemoteNodeObjectCurrentDifficulty[i].StopConnection();
                                await Program.RemoteNodeObjectCurrentDifficulty[i].StartConnectionAsync();

                                if (Program.Closed)
                                {
                                    break;
                                }
                            }
                            else
                            {

                                var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectCurrentDifficulty[i].RemoteNodeObjectLastPacketReceived;
                                var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                                if (lastPacketReceivedTimeStamp + 5 < currentTimestamp)
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

                                    Program.RemoteNodeObjectCurrentDifficulty[i].StopConnection();
                                    await Program.RemoteNodeObjectCurrentDifficulty[i].StartConnectionAsync();

                                    if (Program.Closed)
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    for (int i = 0; i < Program.RemoteNodeObjectCurrentRate.Count; i++)
                    {
                        if (i < Program.RemoteNodeObjectCurrentRate.Count)
                        {
                            if (!Program.RemoteNodeObjectCurrentRate[i].RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectCurrentDifficulty[i].RemoteNodeObjectTcpClient.GetStatusConnectToSeed())
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

                                Program.RemoteNodeObjectCurrentRate[i].StopConnection();
                                await Program.RemoteNodeObjectCurrentRate[i].StartConnectionAsync();

                                if (Program.Closed)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    if (ClassRemoteNodeSync.WantToBePublicNode)
                    {
                        if (!Program.RemoteNodeObjectToBePublic.RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectToBePublic.RemoteNodeObjectTcpClient.GetStatusConnectToSeed())
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
                    for (int i = 0; i < Program.RemoteNodeObjectTotalBlockMined.Count; i++)
                    {
                        if (i < Program.RemoteNodeObjectTotalBlockMined.Count)
                        {
                            if (!Program.RemoteNodeObjectTotalBlockMined[i].RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectTotalBlockMined[i].RemoteNodeObjectTcpClient.GetStatusConnectToSeed())
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

                                Program.RemoteNodeObjectTotalBlockMined[i].StopConnection();
                                await Program.RemoteNodeObjectTotalBlockMined[i].StartConnectionAsync();
                                if (Program.Closed)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    for (int i = 0; i < Program.RemoteNodeObjectTotalFee.Count; i++)
                    {
                        if (i < Program.RemoteNodeObjectTotalFee.Count)
                        {
                            if (!Program.RemoteNodeObjectTotalFee[i].RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectTotalFee[i].RemoteNodeObjectTcpClient.GetStatusConnectToSeed())
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

                                Program.RemoteNodeObjectTotalFee[i].StopConnection();
                                await Program.RemoteNodeObjectTotalFee[i].StartConnectionAsync();

                                if (Program.Closed)
                                {
                                    break;
                                }
                            }
                        }
                    }
                    for (int i = 0; i < Program.RemoteNodeObjectTotalPendingTransaction.Count; i++)
                    {
                        if (i < Program.RemoteNodeObjectTotalPendingTransaction.Count)
                        {
                            if (!Program.RemoteNodeObjectTotalPendingTransaction[i].RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectTotalPendingTransaction[i].RemoteNodeObjectTcpClient.GetStatusConnectToSeed())
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

                                Program.RemoteNodeObjectTotalPendingTransaction[i].StopConnection();
                                await Program.RemoteNodeObjectTotalPendingTransaction[i].StartConnectionAsync();

                                if (Program.Closed)
                                {
                                    break;
                                }
                            }
                        }
                    }


                    if (!Program.RemoteNodeObjectTransaction.RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectTransaction.RemoteNodeObjectTcpClient.GetStatusConnectToSeed())
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
                        if (lastPacketReceivedTimeStamp + 5 < currentTimestamp)
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
                        
                    

                    for (int i = 0; i < Program.RemoteNodeObjectTotalTransaction.Count; i++)
                    {
                        if (i < Program.RemoteNodeObjectTotalPendingTransaction.Count)
                        {
                            if (!Program.RemoteNodeObjectTotalTransaction[i].RemoteNodeObjectConnectionStatus || !Program.RemoteNodeObjectTotalTransaction[i].RemoteNodeObjectTcpClient.GetStatusConnectToSeed())
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

                                Program.RemoteNodeObjectTotalTransaction[i].StopConnection();
                                await Program.RemoteNodeObjectTotalTransaction[i].StartConnectionAsync();

                                if (Program.Closed)
                                {
                                    break;
                                }
                            }
                            else
                            {


                                var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectTotalTransaction[i].RemoteNodeObjectLastPacketReceived;
                                var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                                if (lastPacketReceivedTimeStamp + 5 < currentTimestamp)
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

                                    Program.RemoteNodeObjectTotalTransaction[i].StopConnection();
                                    await Program.RemoteNodeObjectTotalTransaction[i].StartConnectionAsync();

                                    if (Program.Closed)
                                    {
                                        break;
                                    }
                                }
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
                if(await ConnectToTarget(clientBlockchain, ClassConnectorSetting.SeedNodeIp[ClassUtils.GetRandomBetween(0, ClassConnectorSetting.SeedNodeIp.Count-1)], ClassConnectorSetting.SeedNodePort))
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
                for (int i = 0; i < Program.TotalConnectionSync; i++)
                {
                    if (i < Program.TotalConnectionSync)
                    {
                        Program.RemoteNodeObjectTotalTransaction[i].StopConnection();
                        Program.RemoteNodeObjectCoinCirculating[i].StopConnection();
                        Program.RemoteNodeObjectCoinMaxSupply[i].StopConnection();
                        Program.RemoteNodeObjectCurrentDifficulty[i].StopConnection();
                        Program.RemoteNodeObjectCurrentRate[i].StopConnection();
                        Program.RemoteNodeObjectTotalBlockMined[i].StopConnection();
                        Program.RemoteNodeObjectTotalFee[i].StopConnection();
                        Program.RemoteNodeObjectTotalPendingTransaction[i].StopConnection();
                    }
                }
            }
        }
    }
}
