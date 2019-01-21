using System;
using System.Collections.Generic;
using System.Globalization;
using Xiropht_RemoteNode.Data;
using Xiropht_RemoteNode.Log;

namespace Xiropht_RemoteNode.RemoteNode
{
    public class ClassRemoteNodeSortingTransactionPerWallet
    {

        /// <summary>
        /// Add a new transaction on the sorted list of transaction per wallet.
        /// </summary>
        /// <param name="transaction"></param>
        public static void AddNewTransactionSortedPerWallet(string transaction)
        {
            var dataTransactionSplit = transaction.Split(new[] { "-" }, StringSplitOptions.None);
            float idWalletSender;

            if (dataTransactionSplit[0] != "m")
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
            Decimal amount = 0; // Amount.
            Decimal fee = 0; // Fee.
            float idWalletReceiver;
            if (dataTransactionSplit[3] == "")
            {
                idWalletReceiver = -1;
                ClassLog.Log("Transaction ID: " + ClassRemoteNodeSync.ListTransactionPerWallet.Count + " is corrupted, data: " + transaction, 0, 3);
            }
            else
            {
                idWalletReceiver = float.Parse(dataTransactionSplit[3].Replace(".", ","), NumberStyles.Any, Program.GlobalCultureInfo); // Receiver ID.


                Decimal timestamp = Decimal.Parse(dataTransactionSplit[4]); // timestamp CEST.
                string hashTransaction = dataTransactionSplit[5]; // Transaction hash.
                string timestampRecv = dataTransactionSplit[6];

                var splitTransactionInformation = dataTransactionSplit[7].Split(new[] { "#" },
                    StringSplitOptions.None);

                string blockHeight = splitTransactionInformation[0]; // Block height;


                // Real crypted fee, amount sender.
                string realFeeAmountSend = splitTransactionInformation[1];

                // Real crypted fee, amount receiver.
                string realFeeAmountRecv = splitTransactionInformation[2];

                string dataInformationSend = "SEND#" + amount + "#" + fee + "#" + timestamp + "#" +
                                             hashTransaction + "#" + timestampRecv + "#" + blockHeight + "#" + realFeeAmountSend + "#" +
                                             realFeeAmountRecv + "#";
                string dataInformationRecv = "RECV#" + amount + "#" + fee + "#" + timestamp + "#" +
                                             hashTransaction + "#" + timestampRecv + "#" + blockHeight + "#" + realFeeAmountSend + "#" +
                                             realFeeAmountRecv + "#";

                if (idWalletSender != -1)
                {
                    if (ClassRemoteNodeSync.ListTransactionPerWallet.ContainsKey(idWalletSender))
                    {
                        ClassRemoteNodeSync.ListTransactionPerWallet[idWalletSender].Add(dataInformationSend);
                    }
                    else
                    {
                        ClassRemoteNodeSync.ListTransactionPerWallet.Add(idWalletSender, new List<string>());
                        ClassRemoteNodeSync.ListTransactionPerWallet[idWalletSender].Add(dataInformationSend);
                    }

                }
                if (idWalletReceiver != -1)
                {

                    if (ClassRemoteNodeSync.ListTransactionPerWallet.ContainsKey(idWalletReceiver))
                    {
                        ClassRemoteNodeSync.ListTransactionPerWallet[idWalletReceiver].Add(dataInformationRecv);
                    }
                    else
                    {
                        ClassRemoteNodeSync.ListTransactionPerWallet.Add(idWalletReceiver, new List<string>());
                        ClassRemoteNodeSync.ListTransactionPerWallet[idWalletReceiver].Add(dataInformationRecv);
                    }
                }
            }

        }
    }
}
