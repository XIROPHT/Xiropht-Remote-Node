using System;
using Xiropht_RemoteNode.Data;
using Xiropht_RemoteNode.RemoteNode;

namespace Xiropht_RemoteNode.Api
{
    public class ClassApiTransaction : IDisposable
    {
        private Tuple<string, string> TupleTransaction;
        private string Transaction;

        public ClassApiTransaction()
        {
            TupleTransaction = new Tuple<string, string>(string.Empty, string.Empty);
            Transaction = string.Empty;
        }

        #region Disposing Part Implementation 

        private bool _disposed;


        ~ClassApiTransaction()
        {
            Dispose(false);
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                TupleTransaction = null;
                Transaction = null;
                if (disposing)
                {

                }
            }

            _disposed = true;
        }

        #endregion

        /// <summary>
        /// Get a transaction from his id , according to the wallet unique id.
        /// </summary>
        /// <param name="walletId"></param>
        /// <param name="transactionId"></param>
        /// <returns></returns>
        public string GetTransactionFromWalletId(float walletId, int transactionId)
        {
            TupleTransaction =
                ClassRemoteNodeSync.ListTransactionPerWallet.GetTransactionPerId(walletId, transactionId);
            if (TupleTransaction.Item1 != "WRONG")
            {
                long getTransactionId = ClassRemoteNodeSync.ListOfTransactionHash.ContainsKey(TupleTransaction.Item1);

                if (getTransactionId != -1)
                {
                    Transaction = ClassRemoteNodeSync.ListOfTransaction.GetTransaction(getTransactionId).Item1;
                    if (Transaction != "WRONG")
                    {
                        var dataTransactionSplit = Transaction.Split(new[] {"-"}, StringSplitOptions.None);

                        if (TupleTransaction.Item2 == ClassRemoteNodeTransactionPerWalletType.TypeSend)
                        {
                            decimal timestamp = decimal.Parse(dataTransactionSplit[4]); // timestamp CEST.
                            decimal amount = 0; // Amount.
                            decimal fee = 0; // Fee.
                            string timestampRecv = dataTransactionSplit[6];
                            string hashTransaction = dataTransactionSplit[5]; // Transaction hash.

                            var splitTransactionInformation =
                                dataTransactionSplit[7].Split(new[] {"#"}, StringSplitOptions.None);

                            // Real crypted fee, amount sender.
                            string blockHeight = splitTransactionInformation[0];
                            string realFeeAmountSend = splitTransactionInformation[1];
                            string realFeeAmountRecv = splitTransactionInformation[2];
                            return ClassRemoteNodeTransactionPerWalletType.TypeSend + "#" + amount + "#" + fee + "#" +
                                   timestamp + "#" + hashTransaction + "#" + timestampRecv + "#" + blockHeight + "#" +
                                   realFeeAmountSend + "#" + realFeeAmountRecv + "#";


                        }

                        if (TupleTransaction.Item2 == ClassRemoteNodeTransactionPerWalletType.TypeRecv)
                        {
                            decimal timestamp = decimal.Parse(dataTransactionSplit[4]); // timestamp CEST.
                            decimal amount = 0; // Amount.
                            decimal fee = 0; // Fee.
                            string timestampRecv = dataTransactionSplit[6];
                            string hashTransaction = dataTransactionSplit[5]; // Transaction hash.

                            var splitTransactionInformation =
                                dataTransactionSplit[7].Split(new[] {"#"}, StringSplitOptions.None);

                            // Real crypted fee, amount sender.
                            string blockHeight = splitTransactionInformation[0];
                            string realFeeAmountSend = splitTransactionInformation[1];
                            string realFeeAmountRecv = splitTransactionInformation[2];

                            return ClassRemoteNodeTransactionPerWalletType.TypeRecv + "#" + amount + "#" + fee + "#" +
                                   timestamp + "#" + hashTransaction + "#" + timestampRecv + "#" + blockHeight + "#" +
                                   realFeeAmountSend + "#" + realFeeAmountRecv + "#";

                        }
                    }
                }
            }

            return "WRONG";

        }

    }
}
