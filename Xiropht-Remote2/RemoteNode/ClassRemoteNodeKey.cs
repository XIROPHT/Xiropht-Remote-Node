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
            _threadUpdateTrustedKey = new Thread(delegate ()
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
                                    .Split(new[] { "#" }, StringSplitOptions.None)[6]);
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
            

           
                ClassRemoteNodeSync.HashTransactionList = Utils.Utils.ConvertStringtoMD5(string.Join(String.Empty, ClassRemoteNodeSync.ListOfTransaction.Values));

                ClassLog.Log(
                    "Hash key from transaction list generated: " + ClassRemoteNodeSync.HashTransactionList + " ", 1, 1);
           
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
                        ClassRemoteNodeSync.HashBlockList = Utils.Utils.ConvertStringtoMD5(string.Join(String.Empty, ClassRemoteNodeSync.ListOfBlock.Values));
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