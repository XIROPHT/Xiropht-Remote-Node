using System;
using System.Threading;
using Xiropht_RemoteNode.Data;
using Xiropht_RemoteNode.Log;

namespace Xiropht_RemoteNode.RemoteNode
{
    public class ClassRemoteNodeKey
    {
        private static Thread _threadUpdateTrustedKey;
        private static Thread _threadUpdateHashTransactionList;
        private static Thread _threadUpdateHashBlockList;

        private static readonly int ThreadUpdateTrustedKeyInterval = 2 * 1000;

        public static int LastTransactionIdRead;
        public static int LastBlockIdRead;
        public static string DataTransactionRead;
        public static string DataBlockRead;

        public static void StartUpdateTrustedKey()
        {
            _threadUpdateTrustedKey = new Thread(delegate()
            {
                while (!Program.Closed)
                {
                    try
                    {
                        if (ClassRemoteNodeSync.ListOfBlock.Count > 0)
                            ClassRemoteNodeSync.TrustedKey = Utils.Utils.ConvertStringtoMD5(
                                ClassRemoteNodeSync.CoinCirculating + ClassRemoteNodeSync.CoinMaxSupply +
                                ClassRemoteNodeSync.CurrentDifficulty + ClassRemoteNodeSync.CurrentHashrate +
                                ClassRemoteNodeSync.TotalBlockMined + ClassRemoteNodeSync.CurrentTotalFee +
                                ClassRemoteNodeSync.TotalPendingTransaction + ClassRemoteNodeSync
                                    .ListOfBlock[ClassRemoteNodeSync.ListOfBlock.Count - 1]
                                    .Split(new[] {"#"}, StringSplitOptions.None)[6]);
                        else
                            ClassRemoteNodeSync.TrustedKey = Utils.Utils.ConvertStringtoMD5(
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
            if (_threadUpdateHashTransactionList != null &&
                (_threadUpdateHashTransactionList.IsAlive || _threadUpdateHashTransactionList != null))
            {
                _threadUpdateHashTransactionList.Abort();
                GC.SuppressFinalize(_threadUpdateHashTransactionList);
            }

            _threadUpdateHashTransactionList = new Thread(delegate()
            {
                if (DataTransactionRead == null || string.IsNullOrWhiteSpace(DataTransactionRead))
                    DataTransactionRead = string.Empty;
                if (ClassRemoteNodeSync.ListOfTransaction.Count > 0 &&
                    LastTransactionIdRead != ClassRemoteNodeSync.ListOfTransaction.Count)
                {
                    /*for (var i = LastTransactionIdRead; i < ClassRemoteNodeSync.ListOfTransaction.Count; i++)
                        if (i < ClassRemoteNodeSync.ListOfTransaction.Count)
                            DataTransactionRead += ClassRemoteNodeSync.ListOfTransaction[i];
                    LastTransactionIdRead = ClassRemoteNodeSync.ListOfTransaction.Count;
                    ClassRemoteNodeSync.HashTransactionList = Utils.Utils.ConvertStringtoMD5(DataTransactionRead);*/


                    ClassRemoteNodeSync.HashTransactionList = Utils.Utils.ConvertStringtoMD5(string.Join(String.Empty, ClassRemoteNodeSync.ListOfTransaction.Values));
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

            _threadUpdateHashBlockList = new Thread(delegate()
            {
                if (DataBlockRead == null || string.IsNullOrWhiteSpace(DataBlockRead)) DataBlockRead = string.Empty;
                if (ClassRemoteNodeSync.ListOfBlock.Count > 0 &&
                    LastBlockIdRead != ClassRemoteNodeSync.ListOfBlock.Count)
                {
                    /*for (var i = LastBlockIdRead; i < ClassRemoteNodeSync.ListOfBlock.Count; i++)
                        if (i < ClassRemoteNodeSync.ListOfBlock.Count)
                            DataBlockRead += ClassRemoteNodeSync.ListOfBlock[i];

                    LastBlockIdRead = ClassRemoteNodeSync.ListOfBlock.Count;
                    ClassRemoteNodeSync.HashBlockList = Utils.Utils.ConvertStringtoMD5(DataBlockRead);*/

                    ClassRemoteNodeSync.HashBlockList = Utils.Utils.ConvertStringtoMD5(string.Join(String.Empty, ClassRemoteNodeSync.ListOfBlock.Values));

                }

                ClassLog.Log("Hash key from block list generated: " + ClassRemoteNodeSync.HashBlockList + " ", 1, 1);
            });
            _threadUpdateHashBlockList.Start();
        }
    }
}