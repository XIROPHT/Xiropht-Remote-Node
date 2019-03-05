using System;
using System.Threading;
using Xiropht_RemoteNode.Data;
using Xiropht_RemoteNode.Log;

namespace Xiropht_RemoteNode.RemoteNode
{
    public class ClassRemoteNodeKey
    {
        private static Thread _threadUpdateTrustedKey;
        private static Thread _threadUpdateHashBlockList;
        private static Thread _threadUpdateHashTransactionList;

        private static readonly int ThreadUpdateTrustedKeyInterval = 500;

        public static int LastTransactionIdRead;
        public static int LastBlockIdRead;
        public static string DataTransactionRead;
        public static string DataBlockRead;

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

            if (_threadUpdateHashTransactionList != null && (_threadUpdateHashTransactionList.IsAlive || _threadUpdateHashTransactionList != null))
            {
                _threadUpdateHashTransactionList.Abort();
                GC.SuppressFinalize(_threadUpdateHashTransactionList);
            }

            _threadUpdateHashTransactionList = new Thread(delegate ()
            {

                try
                {

                    //  ClassRemoteNodeSync.HashTransactionList = Utils.ClassUtilsNode.ConvertStringToSha512(string.Join(string.Empty, ClassRemoteNodeSync.ListOfTransaction.Values()));
                    string transactionBlock = string.Empty;
                    string schema = ClassRemoteNodeSync.SchemaHashTransaction;

                    if (!string.IsNullOrEmpty(schema))
                    {
                        var splitSchema = schema.Split(new[] { ";" }, StringSplitOptions.None);
                        foreach (var transaction in splitSchema)
                        {
                            if (transaction != null)
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
                        }
                        if (!string.IsNullOrEmpty(transactionBlock))
                        {
                            ClassRemoteNodeSync.HashTransactionList = Utils.ClassUtilsNode.ConvertStringToSha512(transactionBlock);
                        }
                    }
                }
                catch
                {

                }
                ClassLog.Log(
                    "Hash key from transaction list generated: " + ClassRemoteNodeSync.HashTransactionList + " ", 1, 1);

            });
            _threadUpdateHashTransactionList.Start();
        }

        public static void StartUpdateHashBlockList()
        {
            if (_threadUpdateHashBlockList != null &&
                (_threadUpdateHashBlockList.IsAlive || _threadUpdateHashBlockList != null))
            {
                _threadUpdateHashBlockList.Abort();
                GC.SuppressFinalize(_threadUpdateHashBlockList);
            }

            _threadUpdateHashBlockList = new Thread(delegate ()
            {

                try
                {
                    if (LastBlockIdRead != ClassRemoteNodeSync.ListOfBlock.Count)
                    {
                        LastBlockIdRead = ClassRemoteNodeSync.ListOfBlock.Count;
                        //ClassRemoteNodeSync.HashBlockList = Utils.ClassUtilsNode.ConvertStringToSha512(string.Join(String.Empty, ClassRemoteNodeSync.ListOfBlock.Values));

                        string blockBLock = string.Empty;
                        string schema = ClassRemoteNodeSync.SchemaHashBlock;

                        if (!string.IsNullOrEmpty(schema))
                        {
                            var splitSchema = schema.Split(new[] { ";" }, StringSplitOptions.None);
                            foreach (var block in splitSchema)
                            {
                                if (block != null)
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
                            }
                            if (!string.IsNullOrEmpty(blockBLock))
                            {
                                ClassRemoteNodeSync.HashBlockList = Utils.ClassUtilsNode.ConvertStringToSha512(blockBLock);
                            }
                        }
                    }
                }
                catch
                {

                }
                ClassLog.Log("Hash key from block list generated: " + ClassRemoteNodeSync.HashBlockList + " ", 1, 1);
            });
            _threadUpdateHashBlockList.Start();
        }
    }
}