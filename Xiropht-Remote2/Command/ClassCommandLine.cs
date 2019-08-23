using System;
using System.Diagnostics;
using System.Threading;
using Xiropht_Connector_All.Utils;
using Xiropht_RemoteNode.Api;
using Xiropht_RemoteNode.Data;
using Xiropht_RemoteNode.Log;
using Xiropht_RemoteNode.RemoteNode;

namespace Xiropht_RemoteNode.Command
{
    public class ClassCommandLineEnumeration
    {
        public const string CommandLineHelp = "help";
        public const string CommandLineStatus = "status";
        public const string CommandLineTransaction = "transaction";
        public const string CommandLineBlock = "block";
        public const string CommandLineLog = "log";
        public const string CommandLineClearSync = "clearsync";
        public const string CommandLineFilterList = "filterlist";
        public const string CommandLineBanList = "banlist";
        public const string CommandLineSave = "save";
        public const string CommandLineExit = "exit";
    }

    public class ClassCommandLine
    {
        public static bool CommandLine(string command)
        {
            var splitCommand = command.Split(new char[0], StringSplitOptions.None);
            try
            {
                switch (splitCommand[0])
                {
                    case ClassCommandLineEnumeration.CommandLineHelp:
                        Console.WriteLine("Command list: ");
                        Console.WriteLine(ClassCommandLineEnumeration.CommandLineStatus + " -> Get Network status of your node");
                        Console.WriteLine(ClassCommandLineEnumeration.CommandLineTransaction + " -> Get the number of transaction(s) sync.");
                        Console.WriteLine(ClassCommandLineEnumeration.CommandLineBlock + " -> Get the number of block(s) sync.");
                        Console.WriteLine(ClassCommandLineEnumeration.CommandLineLog + " -> Can set level of log to show: (default) log 0 max level 4");
                        Console.WriteLine(ClassCommandLineEnumeration.CommandLineClearSync + " -> Clear the sync of the remote node.");
                        Console.WriteLine(ClassCommandLineEnumeration.CommandLineFilterList + " -> show every incoming connections ip's and their status.");
                        Console.WriteLine(ClassCommandLineEnumeration.CommandLineBanList + " -> show every incoming connections ip's banned.");
                        Console.WriteLine(ClassCommandLineEnumeration.CommandLineSave + " -> Save sync.");
                        Console.WriteLine(ClassCommandLineEnumeration.CommandLineExit + " -> Save sync and Exit the node.");
                        break;
                    case ClassCommandLineEnumeration.CommandLineStatus:
                        Console.WriteLine("Total Transaction Sync: " + (ClassRemoteNodeSync.ListOfTransaction.Count));
                        Console.WriteLine("Total Transaction in the Blockchain: " + ClassRemoteNodeSync.TotalTransaction);
                        long totalTransactionSortedPerWallet = ClassRemoteNodeSync.ListTransactionPerWallet.Count;

                        Console.WriteLine("Total Transaction Sorted for Wallet(s): " + totalTransactionSortedPerWallet);
                        Console.WriteLine("Total Block(s) Sync: " + (ClassRemoteNodeSync.ListOfBlock.Count));
                        Console.WriteLine("Total Block(s) mined in the Blockchain: " + ClassRemoteNodeSync.TotalBlockMined);
                        Console.WriteLine("Total Block(s) left to mining: " + ClassRemoteNodeSync.CurrentBlockLeft);
                        Console.WriteLine("Total pending transaction in the network: " + ClassRemoteNodeSync.TotalPendingTransaction);
                        Console.WriteLine("Total Fee in the network: " + ClassRemoteNodeSync.CurrentTotalFee, Program.GlobalCultureInfo);
                        Console.WriteLine("Current Mining Difficulty: " + ClassRemoteNodeSync.CurrentDifficulty);
                        if (ClassRemoteNodeSync.CurrentHashrate != null)
                        {
                            Console.WriteLine("Current Mining Hashrate: " + ClassUtils.GetTranslateHashrate(ClassRemoteNodeSync.CurrentHashrate.Replace(".", ","), 2).Replace(",", "."), Program.GlobalCultureInfo);
                        }
                        Console.WriteLine("Total Coin Max Supply: " + ClassRemoteNodeSync.CoinMaxSupply, Program.GlobalCultureInfo);

                        Console.WriteLine("Total Coin Circulating: " + ClassRemoteNodeSync.CoinCirculating, Program.GlobalCultureInfo);

                        if (ClassRemoteNodeSync.WantToBePublicNode)
                        {
                            string publicNodes = string.Empty;
                            for (int i = 0; i < ClassRemoteNodeSync.ListOfPublicNodes.Count; i++)
                            {
                                if (i < ClassRemoteNodeSync.ListOfPublicNodes.Count)
                                {
                                    publicNodes += ClassRemoteNodeSync.ListOfPublicNodes[i] + " ";
                                }
                            }
                            Console.WriteLine("List of Public Remote Node: " + publicNodes);
                            string status = "NOT LISTED";
                            if (ClassRemoteNodeSync.ImPublicNode)
                            {
                                status = "LISTED";
                            }
                            Console.WriteLine("Public Status of the Remote Node: " + status);

                        }
                            Console.WriteLine("Trusted Key: " + ClassRemoteNodeSync.TrustedKey);
                            Console.WriteLine("Hash Transaction Key: " + ClassRemoteNodeSync.HashTransactionList);
                            Console.WriteLine("Hash Block Key: " + ClassRemoteNodeSync.HashBlockList);
                        
                        break;
                    case ClassCommandLineEnumeration.CommandLineTransaction:
                        Console.WriteLine("Total Transaction Sync: " + (ClassRemoteNodeSync.ListOfTransaction.Count));
                        Console.WriteLine("Total Transaction in the Blockchain: " + ClassRemoteNodeSync.TotalTransaction);
                        break;
                    case ClassCommandLineEnumeration.CommandLineBlock:
                        Console.WriteLine("Total Block(s) Sync: " + (ClassRemoteNodeSync.ListOfBlock.Count));
                        Console.WriteLine("Total Block(s) mined in the Blockchain: " + ClassRemoteNodeSync.TotalBlockMined);
                        Console.WriteLine("Total Block(s) left to mining: " + ClassRemoteNodeSync.CurrentBlockLeft);
                        break;
                    case ClassCommandLineEnumeration.CommandLineLog:
                        if (!string.IsNullOrEmpty(splitCommand[1]))
                        {
                            if (int.TryParse(splitCommand[1], out var logLevel))
                            {
                                if (logLevel < 0)
                                {
                                    logLevel = 0;
                                }
                                if (logLevel > 7)
                                {
                                    logLevel = 7;
                                }
                                Console.WriteLine("Log Level " + Program.LogLevel + " -> " + logLevel);
                                Program.LogLevel = logLevel;
                            }
                            else
                            {
                                Console.WriteLine("Wrong argument: " + splitCommand[1] + " should be a number.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Empty/Missing argument.");
                        }
                        break;
                    case ClassCommandLineEnumeration.CommandLineClearSync:
                        if (Program.RemoteNodeObjectTransaction.RemoteNodeObjectInSyncTransaction || Program.RemoteNodeObjectBlock.RemoteNodeObjectInSyncBlock)
                        {
                            Console.WriteLine("Cannot clean remote node sync, your remote node is currently on sync.");
                            Console.WriteLine("If you absolutly want to clear your sync, close the remote node, remove the Blockchain folder and restart.");
                        }
                        else
                        {
                            ClassRemoteNodeSync.ListOfTransaction.Clear();
                            ClassRemoteNodeSync.ListOfTransactionHash.Clear();
                            ClassRemoteNodeSync.ListTransactionPerWallet.Clear();
                            ClassRemoteNodeSync.ListOfBlock.Clear();
                            ClassRemoteNodeSync.ListOfBlockHash.Clear();
                            ClassRemoteNodeSave.ClearBlockSyncSave();
                            ClassRemoteNodeSave.ClearTransactionSyncSave();
                            ClassRemoteNodeKey.DataBlockRead = string.Empty;
                            ClassRemoteNodeKey.DataTransactionRead = string.Empty;
                            ClassRemoteNodeSave.TotalBlockSaved = 0;
                            ClassRemoteNodeSave.TotalTransactionSaved = 0;
                            ClassRemoteNodeSave.DataTransactionSaved = string.Empty;
                            ClassRemoteNodeSave.DataBlockSaved = string.Empty;


                            Console.WriteLine("Clear finish, restart sync..");
                            ClassRemoteNodeKey.StartUpdateHashTransactionList();
                            ClassRemoteNodeKey.StartUpdateHashBlockList();
                            ClassRemoteNodeKey.StartUpdateTrustedKey();
                        }
                        break;
                    case ClassCommandLineEnumeration.CommandLineFilterList:
                        if (ClassApiBan.ListFilterObjects.Count > 0)
                        {
                            foreach (var objectBan in ClassApiBan.ListFilterObjects)
                            {
                                if (objectBan.Value.Banned)
                                {
                                    long banDelay = objectBan.Value.LastBanDate - DateTimeOffset.Now.ToUnixTimeSeconds();
                                    Console.WriteLine("IP: " + objectBan.Value.Ip + " Total Invalid Packet:" + objectBan.Value.TotalInvalidPacket + " banned pending: " + banDelay + " second(s).");
                                }
                                else
                                {
                                    Console.WriteLine("IP: " + objectBan.Value.Ip + " Total Invalid Packet:" + objectBan.Value.TotalInvalidPacket + " not banned.");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Their is any incoming ip on the list.");
                        }
                        break;
                    case ClassCommandLineEnumeration.CommandLineBanList:
                        if (ClassApiBan.ListFilterObjects.Count > 0)
                        {
                            foreach(var objectBan in ClassApiBan.ListFilterObjects)
                            {
                                if (objectBan.Value.Banned)
                                {
                                    long banDelay = objectBan.Value.LastBanDate - DateTimeOffset.Now.ToUnixTimeSeconds();
                                    Console.WriteLine("IP: " + objectBan.Value.Ip + " Total Invalid Packet:" + objectBan.Value.TotalInvalidPacket + " banned pending: " + banDelay + " second(s).");
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Their is any incoming ip on the list.");
                        }
                        break;
                    case ClassCommandLineEnumeration.CommandLineSave:
                        Console.WriteLine("Stop auto save system. Start manual save sync..");
                        while (ClassRemoteNodeSave.InSaveTransactionDatabase)
                        {
                            Thread.Sleep(1000);
                        }
                        ClassRemoteNodeSave.TotalTransactionSaved = 0;
                        ClassRemoteNodeSave.DataTransactionSaved = string.Empty;
                        ClassRemoteNodeSave.SaveTransaction(false);
                        while (ClassRemoteNodeSave.InSaveBlockDatabase)
                        {
                            Thread.Sleep(1000);
                        }
                        ClassRemoteNodeSave.TotalBlockSaved = 0;
                        ClassRemoteNodeSave.DataBlockSaved = string.Empty;
                        ClassRemoteNodeSave.SaveBlock(false);
                        Console.WriteLine("Sync saved.");
                        Console.WriteLine("Restart auto save system.");
                        ClassRemoteNodeSave.SaveTransaction();
                        ClassRemoteNodeSave.SaveBlock();
                        break;
                    case ClassCommandLineEnumeration.CommandLineExit:
                        Program.Closed = true;
                        Console.WriteLine("Disable auto reconnect remote node..");
                        ClassCheckRemoteNodeSync.DisableCheckRemoteNodeSync();
                        Thread.Sleep(1000);
                        Console.WriteLine("Stop each connection of the remote node.");
                        Program.RemoteNodeObjectBlock.StopConnection(ClassRemoteNodeObjectStopConnectionEnumeration.End);
                        Program.RemoteNodeObjectTransaction.StopConnection(ClassRemoteNodeObjectStopConnectionEnumeration.End);
                        Program.RemoteNodeObjectTotalTransaction.StopConnection(ClassRemoteNodeObjectStopConnectionEnumeration.End);
                        Program.RemoteNodeObjectCoinCirculating.StopConnection(ClassRemoteNodeObjectStopConnectionEnumeration.End);
                        Program.RemoteNodeObjectCoinMaxSupply.StopConnection(ClassRemoteNodeObjectStopConnectionEnumeration.End);
                        Program.RemoteNodeObjectCurrentDifficulty.StopConnection(ClassRemoteNodeObjectStopConnectionEnumeration.End);
                        Program.RemoteNodeObjectCurrentRate.StopConnection(ClassRemoteNodeObjectStopConnectionEnumeration.End);
                        Program.RemoteNodeObjectTotalBlockMined.StopConnection(ClassRemoteNodeObjectStopConnectionEnumeration.End);
                        Program.RemoteNodeObjectTotalFee.StopConnection(ClassRemoteNodeObjectStopConnectionEnumeration.End);
                        Program.RemoteNodeObjectTotalPendingTransaction.StopConnection(ClassRemoteNodeObjectStopConnectionEnumeration.End);
                        ClassLog.StopWriteLog();

                        Thread.Sleep(1000);
                        Console.WriteLine("Stop api..");
                        ClassApi.StopApi();
                        Console.WriteLine("Starting save sync..");
                        while (ClassRemoteNodeSave.InSaveTransactionDatabase)
                        {
                            Thread.Sleep(1000);
                        }
                        ClassRemoteNodeSave.TotalTransactionSaved = 0;
                        ClassRemoteNodeSave.DataTransactionSaved = string.Empty;
                        ClassRemoteNodeSave.SaveTransaction(false);
                        while (ClassRemoteNodeSave.InSaveBlockDatabase)
                        {
                            Thread.Sleep(1000);
                        }
                        ClassRemoteNodeSave.TotalBlockSaved = 0;
                        ClassRemoteNodeSave.DataBlockSaved = string.Empty;
                        ClassRemoteNodeSave.SaveBlock(false);
                        Console.WriteLine("Sync saved.");
                        Process.GetCurrentProcess().Kill();
                        return false;
                }
            }
            catch(Exception error)
            {
                Console.WriteLine("Command line error: "+error.Message);
            }
            return true;
        }
    }
}
