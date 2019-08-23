using System.Collections.Generic;
using Xiropht_RemoteNode.Object;

namespace Xiropht_RemoteNode.Data
{
    public static class ClassRemoteNodeSync
    {
        /// <summary>
        /// Object of sync.
        /// </summary>
        public static BigDictionaryTransaction ListOfTransaction = new BigDictionaryTransaction(); // List of transaction of the blockchain.
        public static BigDictionaryTransactionHash ListOfTransactionHash = new BigDictionaryTransactionHash(); // List of transaction hash of the blockchain.
        public static Dictionary<long, string> ListOfBlock = new Dictionary<long, string>(); // List of block mined of the blockchain.
        public static DictionaryBlockHash ListOfBlockHash = new DictionaryBlockHash(); // List of block mined hash of the blockchain.
        public static Dictionary<string, string> DictionaryCacheValidWalletAddress = new Dictionary<string, string>();

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
        public static BigDictionaryTransactionSortedPerWallet ListTransactionPerWallet = new BigDictionaryTransactionSortedPerWallet();
        public static string HashTransactionList; // A key from each transaction(s).
        public static string HashBlockList; // A key from each block(s).
        public static string TrustedKey; // A key generated from information and hash 

        /// <summary>
        /// For generate right transaction/block keys
        /// </summary>
        public static string SchemaHashTransaction;
        public static string SchemaHashBlock;

        /// <summary>
        /// Status of public system.
        /// </summary>
        public static bool WantToBePublicNode; // This information enable the request for be a public node , the request is send until you are in the list.
        public static bool ImPublicNode; // This information show if your node is listed on the list of public nodes or not.
        public static string MyOwnIP; // This information save your public ip for check if your are in the list of nodes.
        public static List<string> ListOfPublicNodes = new List<string>();

    }
}
