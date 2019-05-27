using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xiropht_Connector_All.Remote;
using Xiropht_Connector_All.Seed;
using Xiropht_Connector_All.Setting;
using Xiropht_Connector_All.Utils;
using Xiropht_RemoteNode.Data;
using Xiropht_RemoteNode.Log;
using Xiropht_RemoteNode.Object;

namespace Xiropht_RemoteNode.RemoteNode
{
    public class ClassRemoteNodeObject
    {
        #region Variable/Object

        /// <summary>
        ///     Type
        /// </summary>
        public string RemoteNodeObjectType;

        /// <summary>
        ///     Status
        /// </summary>
        public bool RemoteNodeObjectConnectionStatus;

        public bool RemoteNodeObjectThreadStatus;

        public bool RemoteNodeObjectLoginStatus;

        public long RemoteNodeObjectLastPacketReceived;

        /// <summary>
        ///     Network object
        /// </summary>
        public ClassSeedNodeConnector RemoteNodeObjectTcpClient;

        /// <summary>
        ///     Setting
        /// </summary>
        private const int RemoteNodeObjectLoopSendRequestInterval = 1;
        private const int RemoteNodeObjectLoopSendRequestInterval2 = 1000;

        private long LastKeepAlivePacketSent; // Last keep alive packet sent.
        private const int KeepAlivePacketDelay = 5; // Send keep alive packet every 1 second.

        /// <summary>
        ///     Reserved to type of transaction sync.
        /// </summary>
        public bool RemoteNodeObjectInSyncTransaction;

        public bool RemoteNodeObjectInReceiveTransaction;

        /// <summary>
        ///     Reserved to type of block sync.
        /// </summary>
        public bool RemoteNodeObjectInSyncBlock;

        public bool RemoteNodeObjectInReceiveBlock;

        public const int MaxTransactionRange = 10;
        public static bool EnableTransactionRange = true;

        #endregion



        /// <summary>
        ///     Constructor, initialize remote node object of sync.
        /// </summary>
        /// <param name="type"></param>
        public ClassRemoteNodeObject(string type)
        {
            RemoteNodeObjectType = type;
        }

        /// <summary>
        ///     Start connection, return if the connection is okay.
        /// </summary>
        /// <returns></returns>
        public async Task<bool> StartConnectionAsync()
        {
            if (RemoteNodeObjectTcpClient == null)
                RemoteNodeObjectTcpClient = new ClassSeedNodeConnector();
            else // For be sure.
                StopConnection();

            if (RemoteNodeObjectType == SyncEnumerationObject.ObjectToBePublic)
            {
                ClassRemoteNodeSync.ImPublicNode = false;
                ClassRemoteNodeSync.ListOfPublicNodes.Clear();
                ClassRemoteNodeSync.MyOwnIP = string.Empty;
            }
            if (await RemoteNodeObjectTcpClient
                .StartConnectToSeedAsync(string.Empty, ClassConnectorSetting.SeedNodePort))
            {
                RemoteNodeObjectConnectionStatus = true;
                RemoteNodeListenNetworkAsync();
                if (await RemoteNodeObjectTcpClient
                    .SendPacketToSeedNodeAsync(Program.Certificate, string.Empty, false, false))
                    if (await RemoteNodeObjectTcpClient
                        .SendPacketToSeedNodeAsync("REMOTE|" + Program.RemoteNodeWalletAddress, Program.Certificate,
                            false, true))
                    {
                        RemoteNodeSendNetworkAsync();
                        return true;
                    }

                RemoteNodeObjectConnectionStatus = false;
                return false;
            }

            RemoteNodeObjectConnectionStatus = false;
            return false;
        }

        /// <summary>
        ///     Stop sync connection, close every thread.
        /// </summary>
        public void StopConnection()
        {
            ClassLog.Log("Object sync "+ RemoteNodeObjectType+" disconnected. Reconnect in a bit of time..", 0, 2);
            RemoteNodeObjectLoginStatus = false;
            RemoteNodeObjectThreadStatus = false;
            RemoteNodeObjectConnectionStatus = false;
            RemoteNodeObjectInReceiveBlock = false;
            RemoteNodeObjectInReceiveTransaction = false;
            RemoteNodeObjectInSyncBlock = false;
            RemoteNodeObjectInSyncTransaction = false;
            RemoteNodeObjectTcpClient?.DisconnectToSeed();
            if (RemoteNodeObjectType == SyncEnumerationObject.ObjectToBePublic)
            {
                ClassRemoteNodeSync.ImPublicNode = false;
                ClassRemoteNodeSync.ListOfPublicNodes.Clear();
                ClassRemoteNodeSync.MyOwnIP = string.Empty;
            }
           
        }

        /// <summary>
        ///     Listen packet from the network.
        /// </summary>
        private async void RemoteNodeListenNetworkAsync()
        {
            try
            {
                await Task.Factory.StartNew(async () =>
                {
                    RemoteNodeObjectThreadStatus = true;
                    while (RemoteNodeObjectConnectionStatus && RemoteNodeObjectThreadStatus)
                    {
                        try
                        {
                            if (!RemoteNodeObjectTcpClient.ReturnStatus())
                            {
                                RemoteNodeObjectConnectionStatus = false;
                                RemoteNodeObjectThreadStatus = false;
                                break;
                            }
                            var packetReceived = await RemoteNodeObjectTcpClient.ReceivePacketFromSeedNodeAsync(Program.Certificate, false, true).ConfigureAwait(false);


                            if (packetReceived == ClassSeedNodeStatus.SeedError)
                            {
                                RemoteNodeObjectConnectionStatus = false;
                                RemoteNodeObjectThreadStatus = false;
                                break;
                            }


                            if (packetReceived.Contains("*"))
                            {
                                var splitPacketReceived = packetReceived.Split(new[] { "*" }, StringSplitOptions.None);
                                if (splitPacketReceived.Length > 1)
                                {
                                    foreach (var packet in splitPacketReceived)
                                        if (packet != null)
                                            if (!string.IsNullOrEmpty(packet))
                                                if (packet.Length > 1)
                                                {
                                                    var packetRecv = packet.Replace("*", "");
                                                    ClassLog.Log("Packet received from blockchain: " + packet, 4, 0);

                                                    if (packetRecv == ClassSeedNodeStatus.SeedError)
                                                    {
                                                        RemoteNodeObjectConnectionStatus = false;
                                                        RemoteNodeObjectThreadStatus = false;
                                                        break;
                                                    }

                                                    await Task.Factory.StartNew(() => { RemoteNodeHandlePacketNetworkAsync(packetRecv); }, CancellationToken.None, TaskCreationOptions.RunContinuationsAsynchronously, PriorityScheduler.Lowest).ConfigureAwait(false);
                                                }
                                }
                                else
                                {
                                    ClassLog.Log("Packet received from blockchain: " + packetReceived, 4, 0);


                                    if (packetReceived.Replace("*", "") == ClassSeedNodeStatus.SeedError)
                                    {
                                        RemoteNodeObjectConnectionStatus = false;
                                        RemoteNodeObjectThreadStatus = false;
                                        break;
                                    }

                                    await Task.Factory.StartNew(() => { RemoteNodeHandlePacketNetworkAsync(packetReceived.Replace("*", "")); }, CancellationToken.None, TaskCreationOptions.RunContinuationsAsynchronously, PriorityScheduler.Lowest).ConfigureAwait(false);

                                }
                            }
                            else
                            {
                                if (packetReceived != ClassSeedNodeStatus.SeedNone)
                                    RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();

                                if (packetReceived == ClassSeedNodeStatus.SeedError)
                                {
                                    RemoteNodeObjectConnectionStatus = false;
                                    RemoteNodeObjectThreadStatus = false;
                                    break;
                                }

                                await Task.Factory.StartNew(() => { RemoteNodeHandlePacketNetworkAsync(packetReceived); }, CancellationToken.None, TaskCreationOptions.None, PriorityScheduler.Lowest).ConfigureAwait(false);

                            }
                        }
                        catch
                        {
                            RemoteNodeObjectThreadStatus = false;
                            RemoteNodeObjectConnectionStatus = false;
                            break;
                        }
                    }
                    RemoteNodeObjectThreadStatus = false;
                }, CancellationToken.None, TaskCreationOptions.LongRunning, PriorityScheduler.Lowest);
            }
            catch (Exception error)
            {
                ClassLog.Log("RemoteNodeListenNetwork start thread exception: " + error.Message, 0, 2);
                RemoteNodeObjectConnectionStatus = false;
                RemoteNodeObjectThreadStatus = false;
            }
        }

        /// <summary>
        ///     Depending of the type of sync, the remote node will send the right packet to sync the right information.
        /// </summary>
        private async void RemoteNodeSendNetworkAsync()
        {

            await Task.Factory.StartNew(async () =>
            {
                while (RemoteNodeObjectConnectionStatus && RemoteNodeObjectThreadStatus)
                {
                    if (RemoteNodeObjectLoginStatus)
                    {
                        switch (RemoteNodeObjectType)
                        {
                            #region Sync Block

                            case SyncEnumerationObject.ObjectBlock:

                                if (int.TryParse(ClassRemoteNodeSync.TotalBlockMined, out var askBlock))
                                {
                                    if (!string.IsNullOrEmpty(ClassRemoteNodeSync.TotalBlockMined)
                                    ) // Ask Blocks only when this information is sync.
                                    {
                                        if (ClassRemoteNodeSync.ListOfBlock.Count.ToString() !=
                                            ClassRemoteNodeSync.TotalBlockMined)
                                        {
                                            if (int.TryParse(ClassRemoteNodeSync.TotalBlockMined,
                                                out var totalBlockMined))
                                            {
                                                if (ClassRemoteNodeSync.ListOfBlock.Count > totalBlockMined)
                                                {
                                                    ClassLog.Log("Too much block, clean sync: ", 2, 3);
                                                    ClassRemoteNodeSync.ListOfBlock.Clear();
                                                    ClassRemoteNodeKey.DataBlockRead = string.Empty;
                                                }

                                                askBlock -= ClassRemoteNodeSync.ListOfBlock.Count;
                                                var totalBlockSaved = ClassRemoteNodeSync.ListOfBlock.Count;
                                                for (var i = 0; i < askBlock; i++)
                                                {
                                                    var cancelBlock = false;
                                                    var blockIdAsked = totalBlockSaved + i;
                                                    RemoteNodeObjectInReceiveBlock = true;
                                                    if (!await RemoteNodeObjectTcpClient
                                                        .SendPacketToSeedNodeAsync(
                                                            ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                                                .RemoteAskBlockPerId + "|" + blockIdAsked,
                                                            Program.Certificate,
                                                            false, true))
                                                    {
                                                        RemoteNodeObjectInReceiveBlock = false;
                                                        RemoteNodeObjectConnectionStatus = false;
                                                        break;
                                                    }

                                                    while (RemoteNodeObjectInReceiveBlock)
                                                    {
                                                        var lastPacketReceivedTimeStamp = Program.RemoteNodeObjectBlock
                                                            .RemoteNodeObjectLastPacketReceived;
                                                        var currentTimestamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                                                        if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeSyncResponse < currentTimestamp)
                                                        {
                                                            ClassLog.Log(
                                                                "Sync object block mined, take too much time to receive a block, cancel and retry now.",
                                                                2, 3);
                                                            cancelBlock = true;
                                                            RemoteNodeObjectInSyncBlock = false;
                                                            RemoteNodeObjectInReceiveBlock = false;
                                                            break;
                                                        }

                                                        if (!RemoteNodeObjectConnectionStatus) break;

                                                        await Task.Delay(1);
                                                    }

                                                    if (cancelBlock)
                                                    {
                                                        RemoteNodeObjectInSyncBlock = false;
                                                        RemoteNodeObjectInReceiveBlock = false;
                                                        i = askBlock;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!await RemoteNodeObjectTcpClient
                                                .SendPacketToSeedNodeAsync(
                                                    ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                                        .RemoteAskTotalBlockMined, Program.Certificate, false, true)
                                                )
                                            {
                                                RemoteNodeObjectConnectionStatus = false;
                                                break;
                                            }

                                            await Task.Delay(100);
                                        }
                                    }

                                    RemoteNodeObjectInSyncBlock = false;
                                }

                                RemoteNodeObjectInSyncBlock = false;
                                break;

                            #endregion

                            #region Sync Transaction

                            case SyncEnumerationObject.ObjectTransaction:

                                if (long.TryParse(ClassRemoteNodeSync.TotalTransaction, out _))
                                {
                                    if (!string.IsNullOrEmpty(ClassRemoteNodeSync.TotalTransaction)
                                    ) // Ask Transactions only when this information is sync.
                                    {
                                        RemoteNodeObjectInSyncTransaction = true;
                                        if (ClassRemoteNodeSync.ListOfTransaction.Count.ToString() !=
                                            ClassRemoteNodeSync.TotalTransaction)
                                        {
                                            if (long.TryParse(ClassRemoteNodeSync.TotalTransaction,
                                                out var totalTransaction))
                                            {
                                                if (ClassRemoteNodeSync.ListOfTransaction.Count > totalTransaction)
                                                {
                                                    ClassLog.Log("Too much transaction, clean sync: ", 2, 3);
                                                    ClassRemoteNodeSync.ListOfTransaction.Clear();
                                                    ClassRemoteNodeKey.DataTransactionRead = string.Empty;
                                                }


                                                if (long.TryParse(ClassRemoteNodeSync.TotalTransaction,
                                                    out var askTransaction))
                                                {
                                                    long totalTransactionSaved =
                                                        ClassRemoteNodeSync.ListOfTransaction.Count;

                                                    if (totalTransactionSaved < askTransaction)
                                                    {
                                                        for (long i = totalTransactionSaved; i < askTransaction; i++)
                                                        {
                                                            if (long.TryParse(ClassRemoteNodeSync.TotalTransaction, out var askTransactionTmp))
                                                            {
                                                                if (ClassRemoteNodeSync.ListOfTransaction.Count < askTransactionTmp)
                                                                {
                                                                    bool cancelTransaction = false;
                                                                    totalTransactionSaved = ClassRemoteNodeSync.ListOfTransaction.Count;
                                                                    long transactionIdAsked = i;

                                                                    if (transactionIdAsked <= askTransactionTmp)
                                                                    {
                                                                        RemoteNodeObjectInReceiveTransaction = true;
                                                                        if (EnableTransactionRange)
                                                                        {
                                                                            if (!await RemoteNodeObjectTcpClient
                                                                                .SendPacketToSeedNodeAsync(
                                                                                    ClassRemoteNodeCommand
                                                                                        .ClassRemoteNodeSendToSeedEnumeration
                                                                                        .RemoteAskTransactionPerRange + "|" +
                                                                                    transactionIdAsked + "|" + MaxTransactionRange, Program.Certificate, false, true))
                                                                            {
                                                                                RemoteNodeObjectConnectionStatus = false;
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            if (!await RemoteNodeObjectTcpClient
                                                                                .SendPacketToSeedNodeAsync(
                                                                                    ClassRemoteNodeCommand
                                                                                        .ClassRemoteNodeSendToSeedEnumeration
                                                                                        .RemoteAskTransactionPerId + "|" +
                                                                                    transactionIdAsked, Program.Certificate, false, true))
                                                                            {
                                                                                RemoteNodeObjectConnectionStatus = false;
                                                                            }
                                                                        }
                                                                    }
                                                                    while (RemoteNodeObjectInReceiveTransaction)
                                                                    {
                                                                        var lastPacketReceivedTimeStamp = RemoteNodeObjectLastPacketReceived;
                                                                        var currentTimestamp =
                                                                            DateTimeOffset.Now.ToUnixTimeSeconds();
                                                                        if (lastPacketReceivedTimeStamp + ClassConnectorSetting.MaxDelayRemoteNodeSyncResponse < currentTimestamp)
                                                                        {
                                                                            ClassLog.Log(
                                                                                "Sync object transaction, take too much time to receive a transaction, cancel and retry now.",
                                                                                2, 3);
                                                                            cancelTransaction = true;
                                                                            break;
                                                                        }

                                                                        if (!RemoteNodeObjectConnectionStatus)
                                                                        {
                                                                            break;
                                                                        }

                                                                        if (!EnableTransactionRange)
                                                                        {
                                                                            await Task.Delay(1);
                                                                        }
                                                                        else
                                                                        {
                                                                            await Task.Delay(MaxTransactionRange);
                                                                        }

                                                                    }
                                                                    if (cancelTransaction)
                                                                    {
                                                                        RemoteNodeObjectConnectionStatus = false;
                                                                        break;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (!await RemoteNodeObjectTcpClient
                                                .SendPacketToSeedNodeAsync(
                                                    ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                                        .RemoteNumberOfTransaction, Program.Certificate, false, true))
                                            {
                                                RemoteNodeObjectConnectionStatus = false;
                                                break;
                                            }

                                            await Task.Delay(100);
                                        }

                                        RemoteNodeObjectInSyncTransaction = false;
                                    }

                                }

                                break;

                            #endregion

                            #region Sync Total Block Mined Information

                            case SyncEnumerationObject.ObjectBlockMined:
                                if (!await RemoteNodeObjectTcpClient
                                    .SendPacketToSeedNodeAsync(
                                        ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                            .RemoteAskTotalBlockMined, Program.Certificate, false, true)
                                    )
                                    RemoteNodeObjectConnectionStatus = false;

                                break;

                            #endregion

                            #region Sync Coin Circulating Information

                            case SyncEnumerationObject.ObjectCoinCirculating:
                                if (!await RemoteNodeObjectTcpClient
                                    .SendPacketToSeedNodeAsync(
                                        ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                            .RemoteAskCoinCirculating, Program.Certificate, false, true)
                                    )
                                    RemoteNodeObjectConnectionStatus = false;

                                break;

                            #endregion

                            #region Sync Max Coin Supply Information

                            case SyncEnumerationObject.ObjectCoinSupply:
                                if (!await RemoteNodeObjectTcpClient
                                    .SendPacketToSeedNodeAsync(
                                        ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                            .RemoteAskCoinMaxSupply, Program.Certificate, false, true)
                                    )
                                    RemoteNodeObjectConnectionStatus = false;

                                break;

                            #endregion

                            #region Sync Current Mining Difficulty Information

                            case SyncEnumerationObject.ObjectCurrentDifficulty:
                                if (!await RemoteNodeObjectTcpClient
                                    .SendPacketToSeedNodeAsync(
                                        ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                            .RemoteAskCurrentDifficulty, Program.Certificate, false, true)
                                    )
                                    RemoteNodeObjectConnectionStatus = false;

                                break;

                            #endregion

                            #region Sync Current Mining Hashrate Information

                            case SyncEnumerationObject.ObjectCurrentRate:
                                if (!await RemoteNodeObjectTcpClient
                                    .SendPacketToSeedNodeAsync(
                                        ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                            .RemoteAskCurrentRate, Program.Certificate, false, true)
                                    )
                                    RemoteNodeObjectConnectionStatus = false;

                                break;

                            #endregion

                            #region Sync Total Pending Transaction Information

                            case SyncEnumerationObject.ObjectPendingTransaction:
                                if (!await RemoteNodeObjectTcpClient
                                    .SendPacketToSeedNodeAsync(
                                        ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                            .RemoteAskTotalPendingTransaction, Program.Certificate, false, true)
                                    )
                                    RemoteNodeObjectConnectionStatus = false;

                                break;

                            #endregion

                            #region Sync Public Node List Information

                            case SyncEnumerationObject.ObjectToBePublic:
                                if (string.IsNullOrEmpty(ClassRemoteNodeSync.MyOwnIP))
                                {
                                    if (!await RemoteNodeObjectTcpClient
                                        .SendPacketToSeedNodeAsync(
                                            ClassSeedNodeCommand.ClassSendSeedEnumeration.RemoteAskOwnIP,
                                            Program.Certificate,
                                            false, true)

                                    )
                                    { // We ask seed nodes instead blockchain for get the public ip of the node.
                                        RemoteNodeObjectConnectionStatus = false;
                                    }
                                }
                                else
                                {



                                    if (!await RemoteNodeObjectTcpClient
                                        .SendPacketToSeedNodeAsync(
                                            ClassSeedNodeCommand.ClassSendSeedEnumeration.WalletAskRemoteNode,
                                            Program.Certificate, false, true)
                                        )
                                    { // We ask seed nodes instead blockchain.
                                        RemoteNodeObjectConnectionStatus = false;
                                    }

                                }

                                break;

                            #endregion

                            #region Sync Total Fee Information

                            case SyncEnumerationObject.ObjectTotalFee:
                                if (!await RemoteNodeObjectTcpClient
                                    .SendPacketToSeedNodeAsync(
                                        ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteAskTotalFee,
                                        Program.Certificate, false, true))
                                    RemoteNodeObjectConnectionStatus = false;

                                break;

                            #endregion

                            #region Sync Total Transaction Information

                            case SyncEnumerationObject.ObjectTotalTransaction:
                                if (!await RemoteNodeObjectTcpClient
                                    .SendPacketToSeedNodeAsync(
                                        ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                            .RemoteNumberOfTransaction, Program.Certificate, false, true)
                                    )
                                    RemoteNodeObjectConnectionStatus = false;

                                break;

                                #endregion
                        }
                    }
                    if (RemoteNodeObjectType != SyncEnumerationObject.ObjectTransaction && RemoteNodeObjectType != SyncEnumerationObject.ObjectBlock)
                    {
                       await Task.Delay(RemoteNodeObjectLoopSendRequestInterval2); // Make a pause for the next request.
                    }
                    else
                    {
                        if (RemoteNodeObjectType == SyncEnumerationObject.ObjectTransaction)
                        {
                            if (long.TryParse(ClassRemoteNodeSync.TotalTransaction, out var totalTransactionToSync))
                            {
                                if (totalTransactionToSync <= ClassRemoteNodeSync.ListOfTransaction.Count)
                                {
                                    await Task.Delay(RemoteNodeObjectLoopSendRequestInterval2); // Make a pause for the next sync of transaction.
                                }
                                else
                                {
                                    await Task.Delay(RemoteNodeObjectLoopSendRequestInterval);
                                }
                            }
                        }
                        else
                        {
                            if (int.TryParse(ClassRemoteNodeSync.TotalBlockMined, out var totalBlockMinedToSync))
                            {
                                if (totalBlockMinedToSync <= ClassRemoteNodeSync.ListOfBlock.Count)
                                {
                                    await Task.Delay(RemoteNodeObjectLoopSendRequestInterval2); // Make a pause for the next sync of transaction.
                                }
                                else
                                {
                                    await Task.Delay(RemoteNodeObjectLoopSendRequestInterval);
                                }
                            }
                        }
                    }
                    if (RemoteNodeObjectLoginStatus)
                    {
                        if (!RemoteNodeObjectInSyncTransaction && !RemoteNodeObjectInSyncBlock && !RemoteNodeObjectInReceiveTransaction && !RemoteNodeObjectInReceiveBlock)
                        {
                            if (LastKeepAlivePacketSent + KeepAlivePacketDelay < DateTimeOffset.Now.ToUnixTimeSeconds())
                            {
                                LastKeepAlivePacketSent = DateTimeOffset.Now.ToUnixTimeSeconds();
                                if (!await RemoteNodeObjectTcpClient.SendPacketToSeedNodeAsync(ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteKeepAlive, Program.Certificate, false, true))
                                {
                                    RemoteNodeObjectConnectionStatus = false;
                                    break;
                                }
                            }
                        }
                    }
                }

                RemoteNodeObjectLoginStatus = false;
                RemoteNodeObjectConnectionStatus = false;
                RemoteNodeObjectInReceiveBlock = false;
                RemoteNodeObjectInReceiveTransaction = false;
                RemoteNodeObjectInSyncBlock = false;
                RemoteNodeObjectInSyncTransaction = false;
            }, CancellationToken.None, TaskCreationOptions.LongRunning, PriorityScheduler.Lowest);
        }

        /// <summary>
        ///     Handle packet received from the network.
        /// </summary>
        /// <param name="packet"></param>
        private async void RemoteNodeHandlePacketNetworkAsync(string packet)
        {
            try
            {
                var packetSplit = packet.Split(new[] { "|" }, StringSplitOptions.None);

                switch (packetSplit[0])
                {
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteAcceptedLogin: // if login is accepted, the remote node can start to sync informations.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        RemoteNodeObjectLoginStatus = true;
                        switch (RemoteNodeObjectType)
                        {
                            case SyncEnumerationObject.ObjectBlock:
                                ClassLog.Log("Blockchain accept your node to sync block.", 0, 1);

                                break;
                            case SyncEnumerationObject.ObjectBlockMined:
                                ClassLog.Log(
                                    "Blockchain accept your node to sync the total number of block mined information.",
                                    0, 1);
                                break;
                            case SyncEnumerationObject.ObjectCoinCirculating:
                                ClassLog.Log(
                                    "Blockchain accept your node to sync the total of coin circulating information.", 0,
                                    1);
                                break;
                            case SyncEnumerationObject.ObjectCoinSupply:
                                ClassLog.Log("Blockchain accept your node to sync coin max supply information.", 0, 1);
                                break;
                            case SyncEnumerationObject.ObjectCurrentDifficulty:
                                ClassLog.Log(
                                    "Blockchain accept your node to sync current mining difficulty information.", 0, 1);
                                break;
                            case SyncEnumerationObject.ObjectCurrentRate:
                                ClassLog.Log("Blockchain accept your node to sync current mining hashrate information.",
                                    0, 1);
                                break;
                            case SyncEnumerationObject.ObjectPendingTransaction:
                                ClassLog.Log(
                                    "Blockchain accept your node to sync total pending transaction information.", 0, 1);
                                break;
                            case SyncEnumerationObject.ObjectTotalFee:
                                ClassLog.Log("Blockchain accept your node to sync total fee information.", 0, 1);
                                break;
                            case SyncEnumerationObject.ObjectTotalTransaction:
                                ClassLog.Log(
                                    "Blockchain accept your node to sync total number of transaction information.", 0,
                                    1);
                                break;
                            case SyncEnumerationObject.ObjectTransaction:
                                ClassLog.Log("Blockchain accept your node to sync transaction.", 0, 1);
                                break;
                            case SyncEnumerationObject.ObjectToBePublic:
                                ClassLog.Log(
                                    "Blockchain accept your node to try to list your remote node on the public list.",
                                    0, 1);
                                break;
                        }

                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteKeepAlive: // Receive a keep alive packet.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendBlockPerId: // Receive a block information.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        var splitBlock = packetSplit[1].Split(new[] { "END" }, StringSplitOptions.None);
                        if (splitBlock.Length > 1)
                        {
                            for (var i = 0; i < splitBlock.Length; i++)
                                if (splitBlock[i] != null)
                                    if (splitBlock[i].Length > 0)
                                    {
                                        var blockSubString = splitBlock[i].Substring(0, splitBlock[i].Length - 1);
                                        var blockLineSplit = blockSubString.Split(new[] { "#" }, StringSplitOptions.None);
                                        var blockIdTmp = int.Parse(blockLineSplit[0]);
                                        if (!ClassRemoteNodeSync.ListOfBlock.ContainsKey(blockIdTmp - 1))
                                        {
                                            if (!ClassRemoteNodeSync.ListOfBlock.ContainsValue(blockSubString))
                                            {
                                                ClassRemoteNodeSync.ListOfBlock.Add(blockIdTmp - 1, blockSubString);
                                                ClassRemoteNodeSync.ListOfBlockHash.InsertBlockHash(blockLineSplit[1], blockIdTmp - 1);
                                                if (ClassRemoteNodeSync.ListOfBlock.Count.ToString() ==
                                                    ClassRemoteNodeSync.TotalBlockMined)
                                                {
                                                    ClassLog.Log(
                                                        "Block mined synced, " + ClassRemoteNodeSync.ListOfBlock.Count +
                                                        "/" + ClassRemoteNodeSync.TotalBlockMined, 0, 1);
                                                    //ClassRemoteNodeKey.StartUpdateHashBlockList();
                                                    if (!await RemoteNodeObjectTcpClient.SendPacketToSeedNodeAsync(ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteAskSchemaBlock, Program.Certificate, false, true))
                                                    {
                                                        RemoteNodeObjectConnectionStatus = false;
                                                        RemoteNodeObjectLoginStatus = false;
                                                        RemoteNodeObjectConnectionStatus = false;
                                                        RemoteNodeObjectInReceiveBlock = false;
                                                        RemoteNodeObjectInReceiveTransaction = false;
                                                        RemoteNodeObjectInSyncBlock = false;
                                                        RemoteNodeObjectInSyncTransaction = false;
                                                        Console.WriteLine("Remote Node Object sync disconnected, ask schema block failed. Restart connection in a minute.");
                                                    }
                                                }
                                                else
                                                {
                                                    ClassLog.Log(
                                                        "Block mined synced at: " + ClassRemoteNodeSync.ListOfBlock.Count +
                                                        "/" + ClassRemoteNodeSync.TotalBlockMined, 0, 2);
                                                }
                                            }
                                        }
                                    }
                        }
                        else
                        {
                            var blockSubString = packetSplit[1].Substring(0, packetSplit[1].Length - 1);
                            var blockLineSplit = blockSubString.Split(new[] { "#" }, StringSplitOptions.None);
                            var blockIdTmp = int.Parse(blockLineSplit[0]);

                            if (!ClassRemoteNodeSync.ListOfBlock.ContainsKey(blockIdTmp - 1))
                            {
                                if (!ClassRemoteNodeSync.ListOfBlock.ContainsValue(blockSubString))
                                {
                                    ClassRemoteNodeSync.ListOfBlock.Add(blockIdTmp - 1, blockSubString);
                                    ClassRemoteNodeSync.ListOfBlockHash.InsertBlockHash(blockLineSplit[1], blockIdTmp - 1);
                                    if (ClassRemoteNodeSync.ListOfBlock.Count.ToString() ==
                                        ClassRemoteNodeSync.TotalBlockMined)
                                    {
                                        ClassLog.Log(
                                            "Block mined synced, " + ClassRemoteNodeSync.ListOfBlock.Count + "/" +
                                            ClassRemoteNodeSync.TotalBlockMined, 0, 1);
                                        //ClassRemoteNodeKey.StartUpdateHashBlockList();

                                        if (!await RemoteNodeObjectTcpClient.SendPacketToSeedNodeAsync(ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteAskSchemaBlock, Program.Certificate, false, true))
                                        {
                                            RemoteNodeObjectConnectionStatus = false;
                                            RemoteNodeObjectLoginStatus = false;
                                            RemoteNodeObjectConnectionStatus = false;
                                            RemoteNodeObjectInReceiveBlock = false;
                                            RemoteNodeObjectInReceiveTransaction = false;
                                            RemoteNodeObjectInSyncBlock = false;
                                            RemoteNodeObjectInSyncTransaction = false;
                                            Console.WriteLine("Remote Node Object sync disconnected, ask schema block failed. Restart connection in a minute.");
                                        }
                                    }
                                    else
                                    {
                                        ClassLog.Log(
                                            "Block mined synced at: " + ClassRemoteNodeSync.ListOfBlock.Count + "/" +
                                            ClassRemoteNodeSync.TotalBlockMined, 0, 2);
                                    }
                                }
                            }
                        }

                        RemoteNodeObjectInReceiveBlock = false;

                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendCoinCirculating: // Receive coin circulating information.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ClassRemoteNodeSync.CoinCirculating = packetSplit[1];
                        ClassLog.Log("Total Coin Circulating: " + packetSplit[1], 2, 2);
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendCoinMaxSupply: // Receive coin max supply information.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ClassRemoteNodeSync.CoinMaxSupply = packetSplit[1];
                        ClassLog.Log("Coin Max Supply: " + packetSplit[1], 2, 2);
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendCurrentDifficulty: // Receive current mining difficulty information.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ClassRemoteNodeSync.CurrentDifficulty = packetSplit[1];
                        ClassLog.Log("Current Mining Difficulty: " + packetSplit[1], 2, 2);
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendCurrentRate: // Receive current mining hashrate information.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ClassRemoteNodeSync.CurrentHashrate = packetSplit[1];
                        ClassLog.Log("Current Mining Hashrate: " + packetSplit[1], 2, 2);
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendNumberOfTransaction: // Receive total number of transaction information.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ClassRemoteNodeSync.TotalTransaction = packetSplit[1].Replace("SEND-NUMBER-OF-TRANSACTION", "");
                        if (long.Parse(ClassRemoteNodeSync.TotalTransaction) <
                            ClassRemoteNodeSync.ListOfTransaction.Count && !Program.RemoteNodeObjectTransaction.RemoteNodeObjectInSyncTransaction)
                        {
                            ClassRemoteNodeSync.ListOfTransaction.Clear();
                            ClassRemoteNodeSync.ListTransactionPerWallet.Clear();
                            ClassRemoteNodeSave.ClearTransactionSyncSave();
                        }

                        ClassLog.Log("Total Transaction: " + packetSplit[1].Replace("SEND-NUMBER-OF-TRANSACTION", ""),
                            2, 2);
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendTotalBlockMined: // Receive total block mined information.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ClassRemoteNodeSync.TotalBlockMined = packetSplit[1];
                        if (int.TryParse(packetSplit[1], out var totalBlockMined))
                        {
                            if (totalBlockMined < ClassRemoteNodeSync.ListOfBlock.Count && !Program.RemoteNodeObjectCoinCirculating.RemoteNodeObjectInSyncBlock)
                            {
                                ClassRemoteNodeSync.ListOfBlock.Clear();
                                ClassRemoteNodeSave.ClearBlockSyncSave();
                            }
                            var totalBlockLeft =
                                Math.Round(
                                    (decimal.Parse(ClassRemoteNodeSync.CoinMaxSupply.Replace(".", ","), System.Globalization.NumberStyles.Any,
                                         Program.GlobalCultureInfo) / 10) - totalBlockMined, 0);
                            ClassRemoteNodeSync.CurrentBlockLeft = "" + totalBlockLeft;
                            ClassLog.Log("Total Block Left: " + ClassRemoteNodeSync.CurrentBlockLeft, 2, 2);
                        }

                        ClassLog.Log("Total Block Mined: " + packetSplit[1], 2, 2);
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendTotalFee: // Receive current total fee information.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ClassRemoteNodeSync.CurrentTotalFee = packetSplit[1];
                        ClassLog.Log("Current Total Fee: " + packetSplit[1], 2, 2);
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendTotalPendingTransaction: // Receive total number of pending transaction information.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ClassRemoteNodeSync.TotalPendingTransaction = packetSplit[1];
                        ClassLog.Log("Total Pending Transaction: " + packetSplit[1], 2, 2);
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendTransactionPerId: // Receive a transaction information.
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();

                        ClassLog.Log("Transaction Received: " + packetSplit[1], 2, 2);

                        // Remove gzip decompression, too slow and unusefull for 0,4 KB per transaction..
                        //var decompressTransaction = ClassUtils.DecompressData(packetSplit[1]);
                        //ClassLog.Log("Transaction received decompressed: " + decompressTransaction, 2, 2);

                        var splitTransaction = packetSplit[1].Split(new[] { "$" }, StringSplitOptions.None);
                        if (splitTransaction.Length > 0)
                        {
                            for (int i = 0; i < splitTransaction.Length; i++)
                            {
                                if (splitTransaction[i] != null)
                                {
                                    if (splitTransaction[i].Length > 0)
                                    {
                                        var transactionSubString = splitTransaction[i].Replace("$", "");



                                        if (!ClassRemoteNodeSortingTransactionPerWallet.AddNewTransactionSortedPerWallet(transactionSubString))
                                        {
                                            ClassLog.Log("Transaction ID: " + ClassRemoteNodeSync.ListOfTransaction.Count + " error, asking again the transaction. Data: " + transactionSubString, 0, 3);

                                            RemoteNodeObjectConnectionStatus = false;
                                            RemoteNodeObjectLoginStatus = false;
                                            RemoteNodeObjectTcpClient?.DisconnectToSeed();
                                        }
                                        else
                                        {
                                            try
                                            {
                                                ClassRemoteNodeSync.ListOfTransaction.InsertTransaction(ClassRemoteNodeSync.ListOfTransaction.Count, transactionSubString);
                                                if ((ClassRemoteNodeSync.ListOfTransaction.Count).ToString() == ClassRemoteNodeSync.TotalTransaction)
                                                {
                                                    ClassLog.Log("Transaction synced, " + (ClassRemoteNodeSync.ListOfTransaction.Count) + "/" + ClassRemoteNodeSync.TotalTransaction, 0, 1);
                                                    if (!await RemoteNodeObjectTcpClient.SendPacketToSeedNodeAsync(ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteAskSchemaTransaction, Program.Certificate, false, true))
                                                    {
                                                        RemoteNodeObjectConnectionStatus = false;
                                                        RemoteNodeObjectLoginStatus = false;
                                                        RemoteNodeObjectConnectionStatus = false;
                                                        RemoteNodeObjectInReceiveBlock = false;
                                                        RemoteNodeObjectInReceiveTransaction = false;
                                                        RemoteNodeObjectInSyncBlock = false;
                                                        RemoteNodeObjectInSyncTransaction = false;
                                                        Console.WriteLine("Remote Node Object sync disconnected, ask schema transaction failed. Restart connection in a minute.");
                                                    }
                                                }
                                                else
                                                {
                                                    ClassLog.Log("Transaction synced at: " + (ClassRemoteNodeSync.ListOfTransaction.Count) + "/" + ClassRemoteNodeSync.TotalTransaction, 0, 2);
                                                }

                                            }
                                            catch
                                            {

                                            }
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {
                            var transactionSubString = packetSplit[1].Replace("$", "");

                            if (!ClassRemoteNodeSortingTransactionPerWallet.AddNewTransactionSortedPerWallet(transactionSubString))
                            {
                                ClassLog.Log("Transaction ID: " + ClassRemoteNodeSync.ListOfTransaction.Count + " error, asking again the transaction. Data: " + transactionSubString, 0, 3);

                                RemoteNodeObjectConnectionStatus = false;
                                RemoteNodeObjectLoginStatus = false;
                                RemoteNodeObjectTcpClient?.DisconnectToSeed();

                            }
                            else
                            {
                                try
                                {
                                    ClassRemoteNodeSync.ListOfTransaction.InsertTransaction(ClassRemoteNodeSync.ListOfTransaction.Count, transactionSubString);
                                    if ((ClassRemoteNodeSync.ListOfTransaction.Count).ToString() == ClassRemoteNodeSync.TotalTransaction)
                                    {
                                        ClassLog.Log("Transaction synced, " + (ClassRemoteNodeSync.ListOfTransaction.Count) + "/" + ClassRemoteNodeSync.TotalTransaction, 0, 1);
                                        if (!await RemoteNodeObjectTcpClient.SendPacketToSeedNodeAsync(ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteAskSchemaTransaction, Program.Certificate, false, true))
                                        {
                                            RemoteNodeObjectConnectionStatus = false;
                                            RemoteNodeObjectLoginStatus = false;
                                            RemoteNodeObjectConnectionStatus = false;
                                            RemoteNodeObjectInReceiveBlock = false;
                                            RemoteNodeObjectInReceiveTransaction = false;
                                            RemoteNodeObjectInSyncBlock = false;
                                            RemoteNodeObjectInSyncTransaction = false;
                                            Console.WriteLine("Remote Node Object sync disconnected, ask schema transaction failed. Restart connection in a minute.");
                                        }
                                    }
                                    else
                                    {
                                        ClassLog.Log("Transaction synced at: " + (ClassRemoteNodeSync.ListOfTransaction.Count) + "/" + ClassRemoteNodeSync.TotalTransaction, 0, 2);

                                    }
                                }
                                catch
                                {

                                }
                            }

                        }

                        RemoteNodeObjectInReceiveTransaction = false;
                
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendCheckBlockPerId:
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (int.TryParse(packetSplit[1], out var blockId))
                            if (ClassRemoteNodeSync.ListOfBlock.Count > blockId)
                                if (!await RemoteNodeObjectTcpClient
                                    .SendPacketToSeedNodeAsync(
                                        ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                            .RemoteCheckBlockPerId + "|" + ClassRemoteNodeSync.ListOfBlock[blockId],
                                        Program.Certificate, false, true))
                                {
                                    RemoteNodeObjectConnectionStatus = false;
                                    RemoteNodeObjectLoginStatus = false;
                                    RemoteNodeObjectConnectionStatus = false;
                                    RemoteNodeObjectInReceiveBlock = false;
                                    RemoteNodeObjectInReceiveTransaction = false;
                                    RemoteNodeObjectInSyncBlock = false;
                                    RemoteNodeObjectInSyncTransaction = false;
                                    Console.WriteLine(
                                        "Remote Node Object sync disconnected. Restart connection in a minute.");
                                }

                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendCheckTransactionPerId:
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (long.TryParse(packetSplit[1], out var transactionId))
                            if (ClassRemoteNodeSync.ListOfTransaction.Count > transactionId)
                                if (!await RemoteNodeObjectTcpClient.SendPacketToSeedNodeAsync(
                                    ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration
                                        .RemoteCheckTransactionPerId + "|" +
                                    ClassRemoteNodeSync.ListOfTransaction.GetTransaction(transactionId), Program.Certificate, false,
                                    true))
                                {
                                    RemoteNodeObjectConnectionStatus = false;
                                    RemoteNodeObjectLoginStatus = false;
                                    RemoteNodeObjectConnectionStatus = false;
                                    RemoteNodeObjectInReceiveBlock = false;
                                    RemoteNodeObjectInReceiveTransaction = false;
                                    RemoteNodeObjectInSyncBlock = false;
                                    RemoteNodeObjectInSyncTransaction = false;
                                    Console.WriteLine(
                                        "Remote Node Object sync disconnected. Restart connection in a minute.");
                                }

                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendCheckBlockHash:
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (!await RemoteNodeObjectTcpClient
                            .SendPacketToSeedNodeAsync(
                                ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteCheckBlockHash + "|" +
                                ClassRemoteNodeSync.HashBlockList, Program.Certificate, false, true)
                            )
                        {
                            RemoteNodeObjectConnectionStatus = false;
                            RemoteNodeObjectLoginStatus = false;
                            RemoteNodeObjectConnectionStatus = false;
                            RemoteNodeObjectInReceiveBlock = false;
                            RemoteNodeObjectInReceiveTransaction = false;
                            RemoteNodeObjectInSyncBlock = false;
                            RemoteNodeObjectInSyncTransaction = false;
                            Console.WriteLine("Remote Node Object sync disconnected. Restart connection in a minute.");
                        }

                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendCheckTransactionHash:
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (!await RemoteNodeObjectTcpClient
                            .SendPacketToSeedNodeAsync(
                                ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteCheckTransactionHash +
                                "|" + ClassRemoteNodeSync.HashTransactionList, Program.Certificate, false, true)
                            )
                        {
                            RemoteNodeObjectConnectionStatus = false;
                            RemoteNodeObjectLoginStatus = false;
                            RemoteNodeObjectConnectionStatus = false;
                            RemoteNodeObjectInReceiveBlock = false;
                            RemoteNodeObjectInReceiveTransaction = false;
                            RemoteNodeObjectInSyncBlock = false;
                            RemoteNodeObjectInSyncTransaction = false;
                            Console.WriteLine("Remote Node Object sync disconnected. Restart connection in a minute.");
                        }

                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendCheckTrustedKey:
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (!await RemoteNodeObjectTcpClient
                            .SendPacketToSeedNodeAsync(
                                ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteCheckTrustedKey +
                                "|" + ClassRemoteNodeSync.TrustedKey, Program.Certificate, false, true)
                            )
                        {
                            RemoteNodeObjectConnectionStatus = false;
                            RemoteNodeObjectLoginStatus = false;
                            RemoteNodeObjectConnectionStatus = false;
                            RemoteNodeObjectInReceiveBlock = false;
                            RemoteNodeObjectInReceiveTransaction = false;
                            RemoteNodeObjectInSyncBlock = false;
                            RemoteNodeObjectInSyncTransaction = false;
                            Console.WriteLine("Remote Node Object sync disconnected. Restart connection in a minute.");
                        }

                        break;
                    case ClassSeedNodeCommand.ClassReceiveSeedEnumeration.WalletSendRemoteNode:
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        if (!string.IsNullOrEmpty(ClassRemoteNodeSync.MyOwnIP))
                        {
                            var splitRemoteNodeList = packet.Split(new[] { "|" }, StringSplitOptions.None);
                            var imPublic = false;
                            var listTmpNode = new List<string>();
                            foreach (var node in splitRemoteNodeList)
                                if (node != ClassSeedNodeCommand.ClassReceiveSeedEnumeration.WalletSendRemoteNode)
                                    if (!string.IsNullOrEmpty(node))
                                    {
                                        listTmpNode.Add(node);
                                        ClassLog.Log("Public Remote Node Host: " + node, 1, 2);
                                        if (node == ClassRemoteNodeSync.MyOwnIP) imPublic = true;
                                    }

                            ClassRemoteNodeSync.ListOfPublicNodes = listTmpNode;
                            if (!imPublic)
                            {
                                ClassRemoteNodeSync.ImPublicNode = false;
                                ClassLog.Log("Your remote node is not listed on the public list.", 1, 2);

                                if (ClassRemoteNodeSync.ListOfBlock.Count ==
                                    int.Parse(ClassRemoteNodeSync.TotalBlockMined) &&
                                    ClassRemoteNodeSync.ListOfTransaction.Count ==
                                    long.Parse(ClassRemoteNodeSync.TotalTransaction))
                                {
                                    if (!await RemoteNodeObjectTcpClient
                                        .SendPacketToSeedNodeAsync(
                                            ClassSeedNodeCommand.ClassSendSeedEnumeration.RemoteAskToBePublic,
                                            Program.Certificate, false, true))
                                    {
                                        RemoteNodeObjectConnectionStatus = false;
                                        RemoteNodeObjectLoginStatus = false;
                                        RemoteNodeObjectConnectionStatus = false;
                                        RemoteNodeObjectInReceiveBlock = false;
                                        RemoteNodeObjectInReceiveTransaction = false;
                                        RemoteNodeObjectInSyncBlock = false;
                                        RemoteNodeObjectInSyncTransaction = false;
                                        ClassRemoteNodeSync.ListOfPublicNodes.Clear();
                                        Console.WriteLine(
                                            "Remote Node Object sync disconnected. Restart connection in a minute.");
                                    }
                                    else
                                    {
                                        if (!await RemoteNodeObjectTcpClient.SendPacketToSeedNodeAsync(ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteAskSchemaTransaction, Program.Certificate, false, true))
                                        {
                                            RemoteNodeObjectConnectionStatus = false;
                                            RemoteNodeObjectLoginStatus = false;
                                            RemoteNodeObjectConnectionStatus = false;
                                            RemoteNodeObjectInReceiveBlock = false;
                                            RemoteNodeObjectInReceiveTransaction = false;
                                            RemoteNodeObjectInSyncBlock = false;
                                            RemoteNodeObjectInSyncTransaction = false;
                                            Console.WriteLine("Remote Node Object sync disconnected, ask schema transaction failed. Restart connection in a minute.");
                                        }
                                        else
                                        {
                                            if (!await RemoteNodeObjectTcpClient.SendPacketToSeedNodeAsync(ClassRemoteNodeCommand.ClassRemoteNodeSendToSeedEnumeration.RemoteAskSchemaBlock, Program.Certificate, false, true))
                                            {
                                                RemoteNodeObjectConnectionStatus = false;
                                                RemoteNodeObjectLoginStatus = false;
                                                RemoteNodeObjectConnectionStatus = false;
                                                RemoteNodeObjectInReceiveBlock = false;
                                                RemoteNodeObjectInReceiveTransaction = false;
                                                RemoteNodeObjectInSyncBlock = false;
                                                RemoteNodeObjectInSyncTransaction = false;
                                                Console.WriteLine("Remote Node Object sync disconnected, ask schema block failed. Restart connection in a minute.");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    ClassLog.Log("Your remote node will ask to be public once he get his sync finish.",
                                        1, 2);
                                }
                            }
                            else
                            {
                                ClassLog.Log("Your remote node is listed on the public list.", 1, 1);
                                ClassRemoteNodeSync.ImPublicNode = true;
                            }
                        }

                        break;
                    case ClassSeedNodeCommand.ClassReceiveSeedEnumeration.RemoteSendOwnIP:
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ClassRemoteNodeSync.MyOwnIP = packetSplit[1];
                        ClassLog.Log("Your own public IP is: " + ClassRemoteNodeSync.MyOwnIP, 2, 3);
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendSchemaTransaction:
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ClassRemoteNodeSync.SchemaHashTransaction = packetSplit[1];
                        //Console.WriteLine("Schema transaction received: " + packetSplit[1]);
                        ClassRemoteNodeKey.StartUpdateHashTransactionList();
                        break;
                    case ClassRemoteNodeCommand.ClassRemoteNodeRecvFromSeedEnumeration.RemoteSendSchemaBlock:
                        RemoteNodeObjectLastPacketReceived = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ClassRemoteNodeSync.SchemaHashBlock = packetSplit[1];
                        //Console.WriteLine("Schema block received: " + packetSplit[1]);
                        ClassRemoteNodeKey.StartUpdateHashBlockList();
                        break;
                }
            }
            catch (Exception)
            {
                RemoteNodeObjectConnectionStatus = false;
                RemoteNodeObjectLoginStatus = false;
                RemoteNodeObjectInReceiveBlock = false;
                RemoteNodeObjectInReceiveTransaction = false;
                RemoteNodeObjectInSyncBlock = false;
                RemoteNodeObjectInSyncTransaction = false;
                if (RemoteNodeObjectType == SyncEnumerationObject.ObjectToBePublic)
                {
                    ClassRemoteNodeSync.ListOfPublicNodes.Clear();
                }
                ClassLog.Log("Remote Node Object sync disconnected. Restart connection in a minute.", 2, 3);
            }
        }

    }
}