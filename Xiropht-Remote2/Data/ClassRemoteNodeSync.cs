using System.Collections.Generic;
using System.Threading;
using Xiropht_RemoteNode.RemoteNode;

namespace Xiropht_RemoteNode.Data
{
    public class ClassRemoteNodeSync
    {
        /// <summary>
        /// Object of sync.
        /// </summary>
        public static Dictionary<int, string> ListOfTransaction = new Dictionary<int, string>(); // List of transaction of the blockchain.
        public static Dictionary<int, string> ListOfBlock = new Dictionary<int, string>(); // List of block mined of the blockchain.
        public static string CoinMaxSupply; // Max Supply of the coin in the blockchain.
        public static string CoinCirculating; // Current amount of coin circulating in the blockchain.
        public static string CurrentTotalFee; // Current amount of fee in the blockchain.
        public static string CurrentHashrate; // Current Mining Power Calculation.
        public static string CurrentDifficulty; // Current Mining Difficulty.
        public static string CurrentBlockLeft; // Current total of blocks left in the blockchain.
        public static string TotalPendingTransaction; // Number of total transactions with status pending in the blockchain. 
        public static string TotalBlockMined; // Number of total of blocks mined in the blockchain.
        public static string TotalTransaction; // Number of total transaction in the blockchain.

        /// <summary>
        /// Object for API (Wallet, pools and more).
        /// </summary>
        public static Dictionary<float, List<string>> ListTransactionPerWallet = new Dictionary<float, List<string>>();
        public static string HashTransactionList; // A key from each transaction(s).
        public static string HashBlockList; // A key from each block(s).
        public static string TrustedKey; // A key generated from information and hash 


        /// <summary>
        /// Status of public system.
        /// </summary>
        public static bool WantToBePublicNode; // This information enable the request for be a public node , the request is send until you are in the list.
        public static bool ImPublicNode; // This information show if your node is listed on the list of public nodes or not.
        public static string MyOwnIP; // This information save your public ip for check if your are in the list of nodes.
        public static List<string> ListOfPublicNodes = new List<string>();
        public static List<string> ListCollectionTransaction = new List<string>();

        public static void CollectionTransaction()
        {
            var threadCollectionTransaction = new Thread(delegate ()
            {
                while (!Program.Closed)
                {
                    for (int i = 0; i < ListCollectionTransaction.Count; i++)
                    {
                        if (i < ListCollectionTransaction.Count)
                        {
                            if (!ListOfTransaction.ContainsValue(ListCollectionTransaction[i]))
                            {
                                if (!ListOfTransaction.ContainsKey(ListOfTransaction.Count))
                                {
                                    ListOfTransaction.Add(ListOfTransaction.Count, ListCollectionTransaction[i]);
                                }
                            }
                        }
                    }
                    ClassRemoteNodeKey.StartUpdateHashTransactionList();
                    if (!ClassRemoteNodeSave.InSaveTransactionDatabase)
                    {
                        ClassRemoteNodeSave.SaveTransaction(false);
                    }
                    Thread.Sleep(100);
                    if (ListOfTransaction.Count.ToString() == TotalTransaction)
                    {
                        ListCollectionTransaction.Clear();
                    }
                }
            });
            threadCollectionTransaction.Start();
        }
    }
}
