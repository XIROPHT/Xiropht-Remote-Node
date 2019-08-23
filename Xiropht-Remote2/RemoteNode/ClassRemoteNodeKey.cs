using System;
using System.Threading;
using Xiropht_RemoteNode.Data;
using Xiropht_RemoteNode.Log;

namespace Xiropht_RemoteNode.RemoteNode
{
    public class ClassRemoteNodeKey
    {
        private static Thread _threadUpdateTrustedKey;

        private static readonly int ThreadUpdateTrustedKeyInterval = 500;

        public static string DataTransactionRead;
        public static string DataBlockRead;
        private static bool _inGenerateTransactionKey;
        private static bool _inGenerateBlockKey;

        public static void StartUpdateTrustedKey()
        {
            _threadUpdateTrustedKey = new Thread(delegate ()
            {
                while (!Program.Closed)
                {
                    try
                    {
                        if (ClassRemoteNodeSync.ListOfBlock.Count > 0)
                            ClassRemoteNodeSync.TrustedKey = Utils.ClassUtilsNode.ConvertStringToSha512(
                                ClassRemoteNodeSync.CoinCirculating + ClassRemoteNodeSync.CoinMaxSupply +
                                ClassRemoteNodeSync.CurrentDifficulty + ClassRemoteNodeSync.CurrentHashrate +
                                ClassRemoteNodeSync.TotalBlockMined + ClassRemoteNodeSync.CurrentTotalFee +
                                ClassRemoteNodeSync.TotalPendingTransaction + ClassRemoteNodeSync
                                    .ListOfBlock[ClassRemoteNodeSync.ListOfBlock.Count - 1]
                                    .Split(new[] { "#" }, StringSplitOptions.None)[6]);
                        else
                            ClassRemoteNodeSync.TrustedKey = Utils.ClassUtilsNode.ConvertStringToSha512(
                                ClassRemoteNodeSync.CoinCirculating + ClassRemoteNodeSync.CoinMaxSupply +
                                ClassRemoteNodeSync.CurrentDifficulty + ClassRemoteNodeSync.CurrentHashrate +
                                ClassRemoteNodeSync.TotalBlockMined + ClassRemoteNodeSync.CurrentTotalFee +
                                ClassRemoteNodeSync.TotalPendingTransaction);
                        ClassLog.Log("Trusted key generated: " + ClassRemoteNodeSync.TrustedKey + " ", 1, 1);
                    }
                    catch (Exception error)
                    {
                        ClassLog.Log("Can't generate trusted key, error: " + error.Message, 0, 1);
                    }

                    Thread.Sleep(ThreadUpdateTrustedKeyInterval);
                }
            });
            _threadUpdateTrustedKey.Start();
        }

        public static void StartUpdateHashTransactionList()
        {

            if (!_inGenerateTransactionKey)
            {
                _inGenerateTransactionKey = true;

                try
                {
                    string transactionBlock = string.Empty;
                    string schema = ClassRemoteNodeSync.SchemaHashTransaction;

                    if (!string.IsNullOrEmpty(schema))
                    {
                        var splitSchema = schema.Split(new[] { ";" }, StringSplitOptions.None);
                        foreach (var transaction in splitSchema)
                        {
                            if (!string.IsNullOrEmpty(transaction))
                            {
                                if (long.TryParse(transaction, out var transactionId))
                                {
                                    if (ClassRemoteNodeSync.ListOfTransaction.ContainsKey(transactionId))
                                    {
                                        transactionBlock += ClassRemoteNodeSync.ListOfTransaction.GetTransaction(transactionId);
                                    }
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(transactionBlock))
                        {
                            ClassRemoteNodeSync.HashTransactionList = Utils.ClassUtilsNode.ConvertStringToSha512(transactionBlock);
                        }
                    }
                }
                catch
                {
                    //
                }
                ClassLog.Log(
                    "Hash key from transaction list generated: " + ClassRemoteNodeSync.HashTransactionList + " ", 1, 1);
                _inGenerateTransactionKey = false;

            }
        }

        public static void StartUpdateHashBlockList()
        {
            if (!_inGenerateBlockKey)
            {
                _inGenerateBlockKey = true;

                try
                {

                    string blockBLock = string.Empty;
                    string schema = ClassRemoteNodeSync.SchemaHashBlock;

                    if (!string.IsNullOrEmpty(schema))
                    {
                        var splitSchema = schema.Split(new[] { ";" }, StringSplitOptions.None);
                        foreach (var block in splitSchema)
                        {
                            if (!string.IsNullOrEmpty(block))
                            {
                                if (int.TryParse(block, out var blockId))
                                {
                                    if (ClassRemoteNodeSync.ListOfBlock.ContainsKey(blockId))
                                    {
                                        blockBLock += ClassRemoteNodeSync.ListOfBlock[blockId];
                                    }
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(blockBLock))
                        {
                            ClassRemoteNodeSync.HashBlockList = Utils.ClassUtilsNode.ConvertStringToSha512(blockBLock);
                        }
                    }
                }
                catch
                {
                    //
                }
                ClassLog.Log("Hash key from block list generated: " + ClassRemoteNodeSync.HashBlockList + " ", 1, 1);
                _inGenerateBlockKey = false;

            }
        }
    }
}