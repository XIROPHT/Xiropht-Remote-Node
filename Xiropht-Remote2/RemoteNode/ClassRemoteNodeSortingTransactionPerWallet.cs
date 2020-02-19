using System;
using System.Globalization;
using Xiropht_RemoteNode.Data;
using Xiropht_RemoteNode.Log;
using Xiropht_RemoteNode.Object;

namespace Xiropht_RemoteNode.RemoteNode
{

    public class ClassRemoteNodeTransactionPerWalletType
    {
        public const string TypeSend = "SEND";
        public const string TypeRecv = "RECV";
        public const string TypeBlockchain = "m";
        public const string TypeDevFee = "f";
        public const string TypeRemoteFee = "r";
    }

    public class ClassRemoteNodeSortingTransactionPerWallet
    {
        /// <summary>
        /// Add a new transaction on the sorted list of transaction per wallet.
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="idTransaction"></param>
        public static bool AddNewTransactionSortedPerWallet(string transaction, long idTransaction)
        {
            try
            {
                if (ClassRemoteNodeSync.ListTransactionPerWallet == null)
                {
                    ClassRemoteNodeSync.ListTransactionPerWallet = new BigDictionaryTransactionSortedPerWallet();
                }
                var dataTransactionSplit = transaction.Split(new[] { "-" }, StringSplitOptions.None);
                float idWalletSender;

                if (dataTransactionSplit[0] != ClassRemoteNodeTransactionPerWalletType.TypeBlockchain && dataTransactionSplit[0] != ClassRemoteNodeTransactionPerWalletType.TypeRemoteFee && dataTransactionSplit[0] != ClassRemoteNodeTransactionPerWalletType.TypeDevFee)
                {
                    idWalletSender = float.Parse(dataTransactionSplit[0].Replace(".", ","), NumberStyles.Any, Program.GlobalCultureInfo);
                }
                else
                {
                    if (dataTransactionSplit[3] == "")
                    {
                        Console.WriteLine("Id sender for block transaction id: " + ClassRemoteNodeSync.ListTransactionPerWallet.Count + " is missing.");
                        idWalletSender = -1;
                    }
                    else
                    {
                        idWalletSender = -1; // Blockchain.
                    }
                }

                float idWalletReceiver;
                if (dataTransactionSplit[3] == "")
                {
                    ClassLog.Log("Transaction ID: " + ClassRemoteNodeSync.ListTransactionPerWallet.Count + " is corrupted, data: " + transaction, 0, 3);
                }
                else
                {
                    idWalletReceiver = float.Parse(dataTransactionSplit[3].Replace(".", ","), NumberStyles.Any, Program.GlobalCultureInfo); // Receiver ID.


                    string hashTransaction = dataTransactionSplit[5]; // Transaction hash.
                    if (ClassRemoteNodeSync.ListOfTransactionHash.ContainsKey(hashTransaction) == -1)
                    {
                        if (ClassRemoteNodeSync.ListOfTransactionHash.InsertTransactionHash(idTransaction, hashTransaction))
                        {


                            bool testTx;


                            #region test data of tx

                            try
                            {
                                decimal timestamp = decimal.Parse(dataTransactionSplit[4]); // timestamp CEST.
                                string timestampRecv = dataTransactionSplit[6];

                                var splitTransactionInformation = dataTransactionSplit[7].Split(new[] {"#"},
                                    StringSplitOptions.None);

                                string blockHeight = splitTransactionInformation[0]; // Block height;


                                // Real crypted fee, amount sender.
                                string realFeeAmountSend = splitTransactionInformation[1];

                                // Real crypted fee, amount receiver.
                                string realFeeAmountRecv = splitTransactionInformation[2];


                                testTx = true;
                            }
                            catch
                            {
                                testTx = false;
                            }

                            #endregion

                            if (testTx)
                            {
                                if (idWalletSender != -1)
                                {
                                    var tupleTxSender = new Tuple<string, string>(hashTransaction, ClassRemoteNodeTransactionPerWalletType.TypeSend);
                                    ClassRemoteNodeSync.ListTransactionPerWallet.InsertTransactionSorted(idWalletSender,
                                        tupleTxSender);
                                }

                                if (idWalletReceiver != -1)
                                {
                                    var tupleTxReceiver = new Tuple<string, string>(hashTransaction, ClassRemoteNodeTransactionPerWalletType.TypeRecv);
                                    ClassRemoteNodeSync.ListTransactionPerWallet.InsertTransactionSorted(
                                        idWalletReceiver, tupleTxReceiver);
                                }
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
