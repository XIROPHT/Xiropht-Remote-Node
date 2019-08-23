using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Xiropht_RemoteNode.Data;

namespace Xiropht_RemoteNode.RemoteNode
{
    public class ClassRemoteNodeSave
    {
        private static readonly string BlockchainTransactonDatabase = "transaction.xirdb";
        private static readonly string BlockchainBlockDatabase = "block.xirdb";
        private static readonly string BlockchainDirectory = "\\Blockchain\\";
        private static readonly string BlockchainTransactionDirectory = "\\Blockchain\\Transaction\\";
        private static readonly string BlockchainBlockDirectory = "\\Blockchain\\Block\\";
        private static readonly string BlockchainWalletCacheDirectory = "\\Blockchain\\Wallet\\";
        private static readonly string BlockchainWalletCacheDatabase = "wallet-cache.xirdb";

        private static StreamWriter BlockchainTransactionWriter;
        private static StreamWriter BlockchainBlockWriter;
        private static StreamWriter BlockchainWalletCacheWriter;

        public static bool InSaveTransactionDatabase;
        public static bool InSaveBlockDatabase;
        public static bool InSaveWalletCacheDatabase;

        public static long TotalTransactionSaved;
        public static int TotalBlockSaved;
        public static int TotalWalletCacheSaved;


        private static Thread _threadAutoSaveTransaction;

        private static Thread _threadAutoSaveBlock;

        private static Thread _threadAutoSaveWalletCache;

        /// <summary>
        ///     Initialize Path make them if they not exist.
        /// </summary>
        public static void InitializePath()
        {
            if (!Directory.Exists(GetCurrentPath() + GetBlockchainPath()))
                Directory.CreateDirectory(GetCurrentPath() + GetBlockchainPath());
            if (!Directory.Exists(GetCurrentPath() + GetBlockchainBlockPath()))
                Directory.CreateDirectory(GetCurrentPath() + GetBlockchainBlockPath());
            if (!Directory.Exists(GetCurrentPath() + GetBlockchainTransactionPath()))
                Directory.CreateDirectory(GetCurrentPath() + GetBlockchainTransactionPath());
            if (!Directory.Exists(GetCurrentPath() + GetBlockchainWalletCachePath()))
                Directory.CreateDirectory(GetCurrentPath() + GetBlockchainWalletCachePath());
        }


        /// <summary>
        ///     Get Current Path of the program.
        /// </summary>
        /// <returns></returns>
        private static string GetCurrentPath()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            if (Environment.OSVersion.Platform == PlatformID.Unix) path = path.Replace("\\", "/");
            return path;
        }

        /// <summary>
        ///     Get Blockchain Path.
        /// </summary>
        /// <returns></returns>
        private static string GetBlockchainPath()
        {
            var path = BlockchainDirectory;
            if (Environment.OSVersion.Platform == PlatformID.Unix) path = path.Replace("\\", "/");
            return path;
        }

        /// <summary>
        ///     Get blockchain path of transaction database.
        /// </summary>
        /// <returns></returns>
        private static string GetBlockchainTransactionPath()
        {
            var path = BlockchainTransactionDirectory;
            if (Environment.OSVersion.Platform == PlatformID.Unix) path = path.Replace("\\", "/");
            return path;
        }

        /// <summary>
        ///     Get blockchain path of wallet cache database.
        /// </summary>
        /// <returns></returns>
        private static string GetBlockchainWalletCachePath()
        {
            var path = BlockchainWalletCacheDirectory;
            if (Environment.OSVersion.Platform == PlatformID.Unix) path = path.Replace("\\", "/");
            return path;
        }

        /// <summary>
        ///     Get blockchain path of block database.
        /// </summary>
        /// <returns></returns>
        private static string GetBlockchainBlockPath()
        {
            var path = BlockchainBlockDirectory;
            if (Environment.OSVersion.Platform == PlatformID.Unix) path = path.Replace("\\", "/");
            return path;
        }

        /// <summary>
        ///     Load transaction(s) database file.
        /// </summary>
        public static bool LoadBlockchainTransaction()
        {
            if (File.Exists(GetCurrentPath() + GetBlockchainTransactionPath() + BlockchainTransactonDatabase))
            {
                Console.WriteLine("Load transaction database file..");

                var counter = 0;
                try
                {
                    using (FileStream fs =
                        File.Open(GetCurrentPath() + GetBlockchainTransactionPath() + BlockchainTransactonDatabase,
                            FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (BufferedStream bs = new BufferedStream(fs))
                    using (StreamReader sr = new StreamReader(bs))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            counter++;
                            try
                            {
                                if (line.Contains("%"))
                                {
                                    var splitTransaction = line.Split(new[] {"%"}, StringSplitOptions.None);
                                    long idTransaction = long.Parse(splitTransaction[0]);
                                    if (ClassRemoteNodeSortingTransactionPerWallet.AddNewTransactionSortedPerWallet(
                                        splitTransaction[1], idTransaction))
                                    {
                                        ClassRemoteNodeSync.ListOfTransaction.InsertTransaction(idTransaction,
                                            splitTransaction[1]);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Transaction: " + line + " on line: " + counter +
                                                          " is probably duplicate or corrupted. Ignored.");
                                    }
                                }
                                else
                                {
                                    long totalTransaction = ClassRemoteNodeSync.ListOfTransaction.Count;
                                    if (ClassRemoteNodeSortingTransactionPerWallet.AddNewTransactionSortedPerWallet(
                                        line, totalTransaction))
                                    {
                                        ClassRemoteNodeSync.ListOfTransaction.InsertTransaction(totalTransaction, line);
                                    }
                                    else
                                    {
                                        Console.WriteLine("Transaction: " + line + " on line: " + counter +
                                                          " is probably duplicate or corrupted. Ignored.");
                                    }
                                }
                            }
                            catch
                            {
                                Console.WriteLine(
                                    "Transaction database seems corrupted, clearing transaction database and resync..");
                                ClassRemoteNodeSync.ListOfTransaction.Clear();
                                ClassRemoteNodeSync.ListOfTransactionHash.Clear();
                                ClassRemoteNodeSync.ListTransactionPerWallet.Clear();
                                return true;
                            }
                        }
                    }
                }
                catch
                {
                    return false;
                }

                TotalTransactionSaved = counter;
                Console.WriteLine(counter + " transaction successfully loaded and included on memory..");
            }
            else
            {
                File.Create(GetCurrentPath() + GetBlockchainTransactionPath() + BlockchainTransactonDatabase).Close();
            }

            return true;
        }

        /// <summary>
        ///     Load block(s) database file.
        /// </summary>
        public static void LoadBlockchainBlock()
        {
            if (File.Exists(GetCurrentPath() + GetBlockchainBlockPath() + BlockchainBlockDatabase))
            {
                Console.WriteLine("Load block database file..");


                var counter = 0;


                using (FileStream fs = File.Open(GetCurrentPath() + GetBlockchainBlockPath() + BlockchainBlockDatabase,
                    FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BufferedStream bs = new BufferedStream(fs))
                    {
                        using (StreamReader sr = new StreamReader(bs))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                counter++;
                                var splitLineBlock = line.Split(new[] { "#" }, StringSplitOptions.None);
                                string blockHash = splitLineBlock[1];
                                long blockId = long.Parse(splitLineBlock[0]);
                                if (!ClassRemoteNodeSync.ListOfBlock.ContainsKey(blockId - 1))
                                {
                                    if (ClassRemoteNodeSync.ListOfBlockHash.GetBlockIdFromHash(blockHash) == -1)
                                    {
                                        ClassRemoteNodeSync.ListOfBlock.Add(blockId - 1, line);
                                        ClassRemoteNodeSync.ListOfBlockHash.InsertBlockHash(blockHash, blockId - 1);
                                    }
                                }
                            }
                        }
                    }
                }

                TotalBlockSaved = counter;
                Console.WriteLine(counter + " block successfully loaded and included on memory..");
            }
            else
            {
                File.Create(GetCurrentPath() + GetBlockchainBlockPath() + BlockchainBlockDatabase).Close();
            }
        }

        /// <summary>
        ///     Load wallet cache database file.
        /// </summary>
        public static void LoadBlockchainWalletCache()
        {
            if (File.Exists(GetCurrentPath() + GetBlockchainWalletCachePath() + BlockchainWalletCacheDatabase))
            {
                Console.WriteLine("Load block database file..");


                var counter = 0;


                using (FileStream fs = File.Open(GetCurrentPath() + GetBlockchainWalletCachePath() + BlockchainWalletCacheDatabase, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BufferedStream bs = new BufferedStream(fs))
                    {
                        using (StreamReader sr = new StreamReader(bs))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                counter++;
                                if (ClassRemoteNodeSync.DictionaryCacheValidWalletAddress.Count < int.MaxValue - 1)
                                {
                                    if (!ClassRemoteNodeSync.DictionaryCacheValidWalletAddress.ContainsKey(line))
                                    {
                                        ClassRemoteNodeSync.DictionaryCacheValidWalletAddress.Add(line, line);
                                    }
                                }
                            }
                        }
                    }
                }

                TotalWalletCacheSaved = counter;
                Console.WriteLine(counter + " wallet cache successfully loaded and included on memory..");
            }
            else
            {
                File.Create(GetCurrentPath() + GetBlockchainWalletCachePath() + BlockchainWalletCacheDatabase).Close();
            }
        }

        /// <summary>
        ///     Force to save transaction.
        /// </summary>
        public static bool SaveTransaction(bool auto = true)
        {
            if (!InSaveTransactionDatabase)
            {
                if (_threadAutoSaveTransaction != null &&
                    (_threadAutoSaveTransaction.IsAlive || _threadAutoSaveTransaction != null))
                {
                    _threadAutoSaveTransaction.Abort();
                    GC.SuppressFinalize(_threadAutoSaveTransaction);
                }

                if (auto)
                {
                    _threadAutoSaveTransaction = new Thread(delegate()
                    {
                        while (!Program.Closed)
                        {
                            try
                            {
                                if (!File.Exists(GetCurrentPath() + GetBlockchainTransactionPath() +
                                                 BlockchainTransactonDatabase))
                                {
                                    File.Create(GetCurrentPath() + GetBlockchainTransactionPath() +
                                                BlockchainTransactonDatabase).Close();
                                }

                                if (BlockchainTransactionWriter == null)
                                {
                                    BlockchainTransactionWriter = new StreamWriter(
                                            GetCurrentPath() + GetBlockchainTransactionPath() +
                                            BlockchainTransactonDatabase, true, Encoding.UTF8, 8192)
                                        {AutoFlush = true};
                                }

                                InSaveTransactionDatabase = true;

                                if (ClassRemoteNodeSync.ListOfTransaction != null)
                                    if (ClassRemoteNodeSync.ListOfTransaction.Count > 0)
                                    {
                                        if (TotalTransactionSaved != ClassRemoteNodeSync.ListOfTransaction.Count)
                                        {
                                            for (var i = TotalTransactionSaved;
                                                i < ClassRemoteNodeSync.ListOfTransaction.Count;
                                                i++)
                                            {
                                                if (ClassRemoteNodeSync.ListOfTransaction.ContainsKey(i))
                                                {
                                                    var transactionObject = ClassRemoteNodeSync.ListOfTransaction
                                                        .GetTransaction(i);

                                                    BlockchainTransactionWriter.Write(
                                                        transactionObject.Item2 + "%" + transactionObject.Item1 + "\n");
                                                }
                                            }

                                            TotalTransactionSaved = ClassRemoteNodeSync.ListOfTransaction.Count;
                                        }
                                    }

                                InSaveTransactionDatabase = false;
                            }
                            catch (Exception error)
                            {
#if DEBUG
                            Console.WriteLine("Can't save transaction(s) to database file: " + error.Message);
#endif
                                ClearTransactionSyncSave();
                            }

                            Thread.Sleep(1000);
                        }
                    });
                    _threadAutoSaveTransaction.Start();
                }
                else
                {
                    try
                    {
                        if (BlockchainTransactionWriter != null)
                        {
                            ClearTransactionSyncSave();
                        }

                        File.Create(GetCurrentPath() + GetBlockchainTransactionPath() +
                                    BlockchainTransactonDatabase).Close();


                        if (!InSaveTransactionDatabase)
                        {
                            InSaveTransactionDatabase = true;

                            if (ClassRemoteNodeSync.ListOfTransaction != null)
                                if (ClassRemoteNodeSync.ListOfTransaction.Count > 0)
                                {
                                    using (var sw = new StreamWriter(GetCurrentPath() + GetBlockchainTransactionPath() +
                                                                     BlockchainTransactonDatabase, true, Encoding.UTF8,
                                        8192) {AutoFlush = true})
                                    {
                                        for (var i = 0; i < ClassRemoteNodeSync.ListOfTransaction.Count; i++)
                                        {
                                            if (ClassRemoteNodeSync.ListOfTransaction.ContainsKey(i))
                                            {
                                                var transactionObject =
                                                    ClassRemoteNodeSync.ListOfTransaction.GetTransaction(i);

                                                sw.Write(transactionObject.Item2 + "%" + transactionObject.Item1 +
                                                         "\n");
                                            }
                                        }
                                    }

                                    TotalTransactionSaved = ClassRemoteNodeSync.ListOfTransaction.Count;
                                }

                            InSaveTransactionDatabase = false;
                        }
                    }
                    catch (Exception error)
                    {
#if DEBUG
                        Console.WriteLine("Can't save transaction(s) to database file: " + error.Message);
#endif
                        ClearTransactionSyncSave();
                    }
                }
            }

            return true;
        }

        /// <summary>
        ///     Force to save block.
        /// </summary>
        public static bool SaveBlock(bool auto = true)
        {
            if (!InSaveBlockDatabase)
            {
                if (_threadAutoSaveBlock != null && (_threadAutoSaveBlock.IsAlive || _threadAutoSaveBlock != null))
                {
                    _threadAutoSaveBlock.Abort();
                    GC.SuppressFinalize(_threadAutoSaveBlock);
                }

                if (auto)
                {
                    _threadAutoSaveBlock = new Thread(delegate()
                    {
                        while (!Program.Closed)
                        {
                            try
                            {
                                if (!File.Exists(GetCurrentPath() + GetBlockchainBlockPath() +
                                                 BlockchainBlockDatabase))
                                {
                                    File.Create(GetCurrentPath() + GetBlockchainBlockPath() +
                                                BlockchainBlockDatabase).Close();
                                }

                                if (BlockchainBlockWriter == null)
                                {
                                    BlockchainBlockWriter =
                                        new StreamWriter(
                                            GetCurrentPath() + GetBlockchainBlockPath() + BlockchainBlockDatabase, true,
                                            Encoding.UTF8, 8192) {AutoFlush = true};
                                }


                                InSaveBlockDatabase = true;
                                if (ClassRemoteNodeSync.ListOfBlock != null)
                                    if (ClassRemoteNodeSync.ListOfBlock.Count > 0)
                                    {
                                        if (TotalBlockSaved != ClassRemoteNodeSync.ListOfBlock.Count)
                                        {
                                            for (var i = TotalBlockSaved;
                                                i < ClassRemoteNodeSync.ListOfBlock.Count;
                                                i++)
                                            {
                                                if (ClassRemoteNodeSync.ListOfBlock.ContainsKey(i))
                                                {
                                                    BlockchainBlockWriter.Write(
                                                        ClassRemoteNodeSync.ListOfBlock[i] + "\n");
                                                }
                                            }

                                            TotalBlockSaved = ClassRemoteNodeSync.ListOfBlock.Count;
                                        }
                                    }

                                InSaveBlockDatabase = false;
                            }
                            catch (Exception error)
                            {
#if DEBUG
                            Console.WriteLine("Can't save block(s) to database file: " + error.Message);
#endif
                                ClearBlockSyncSave();
                            }

                            Thread.Sleep(1000);
                        }
                    });
                    _threadAutoSaveBlock.Start();
                }
                else
                {
                    try
                    {
                        ClearBlockSyncSave();

                        File.Create(GetCurrentPath() + GetBlockchainBlockPath() +
                                    BlockchainBlockDatabase).Close();


                        InSaveBlockDatabase = true;
                        if (ClassRemoteNodeSync.ListOfBlock != null)
                        {
                            if (ClassRemoteNodeSync.ListOfBlock.Count > 0)
                            {
                                using (var sw = new StreamWriter(GetCurrentPath() + GetBlockchainBlockPath() +
                                                                 BlockchainBlockDatabase, true, Encoding.UTF8, 8192)
                                    {AutoFlush = true})
                                {
                                    for (var i = 0; i < ClassRemoteNodeSync.ListOfBlock.Count; i++)
                                    {
                                        if (ClassRemoteNodeSync.ListOfBlock.ContainsKey(i))
                                        {
                                            sw.Write(ClassRemoteNodeSync.ListOfBlock[i] + "\n");
                                        }
                                    }
                                }

                                TotalBlockSaved = ClassRemoteNodeSync.ListOfBlock.Count;
                            }
                        }

                        InSaveBlockDatabase = false;
                    }
                    catch (Exception error)
                    {
#if DEBUG
                        Console.WriteLine("Can't save block(s) to database file: " + error.Message);
#endif
                        ClearBlockSyncSave();
                    }
                }
            }

            return true;
        }


        /// <summary>
        /// Force to save wallet cache.
        /// </summary>
        public static bool SaveWalletCache(bool auto = true)
        {

            if (!InSaveWalletCacheDatabase)
            {
                if (_threadAutoSaveWalletCache != null &&
                    (_threadAutoSaveWalletCache.IsAlive || _threadAutoSaveWalletCache != null))
                {
                    _threadAutoSaveWalletCache.Abort();
                    GC.SuppressFinalize(_threadAutoSaveWalletCache);
                }

                if (auto)
                {
                    _threadAutoSaveWalletCache = new Thread(delegate ()
                    {
                        while (!Program.Closed)
                        {
                            try
                            {
                                if (!File.Exists(GetCurrentPath() + GetBlockchainWalletCachePath() +
                                                 BlockchainWalletCacheDatabase))
                                {
                                    File.Create(GetCurrentPath() + GetBlockchainWalletCachePath() +
                                                BlockchainWalletCacheDatabase).Close();
                                }

                                if (BlockchainWalletCacheWriter == null)
                                {
                                    BlockchainWalletCacheWriter =
                                        new StreamWriter(
                                                GetCurrentPath() + GetBlockchainWalletCachePath() +
                                                BlockchainWalletCacheDatabase, true, Encoding.UTF8, 8192)
                                        { AutoFlush = true };
                                }


                                InSaveWalletCacheDatabase = true;
                                if (ClassRemoteNodeSync.DictionaryCacheValidWalletAddress != null)
                                {
                                    if (ClassRemoteNodeSync.DictionaryCacheValidWalletAddress.Count > 0)
                                    {

                                        if (TotalWalletCacheSaved != ClassRemoteNodeSync
                                                .DictionaryCacheValidWalletAddress.Count)
                                        {
                                            var walletCache = ClassRemoteNodeSync.DictionaryCacheValidWalletAddress
                                                .ToArray();
                                            for (var i = TotalWalletCacheSaved;
                                                i < walletCache.Length;
                                                i++)
                                            {

                                                BlockchainWalletCacheWriter.Write(
                                                    walletCache[i].Key + "\n");

                                            }

                                            TotalWalletCacheSaved = walletCache.Length;
                                            Array.Clear(walletCache, 0, walletCache.Length);
                                        }
                                    }
                                }

                                InSaveWalletCacheDatabase = false;

                            }
                            catch (Exception error)
                            {
#if DEBUG
                            Console.WriteLine("Can't save wallet cache to database file: " + error.Message);
#endif
                                ClearWalletCacheSave();

                            }

                            Thread.Sleep(1000);
                        }
                    });
                    _threadAutoSaveWalletCache.Start();
                }
                else
                {
                    try
                    {

                        ClearWalletCacheSave();

                        File.Create(GetCurrentPath() + GetBlockchainWalletCachePath() +
                                    BlockchainWalletCacheDatabase).Close();



                        InSaveWalletCacheDatabase = true;
                        if (ClassRemoteNodeSync.DictionaryCacheValidWalletAddress != null)
                        {
                            if (ClassRemoteNodeSync.DictionaryCacheValidWalletAddress.Count > 0)
                            {

                                var walletCache = ClassRemoteNodeSync.DictionaryCacheValidWalletAddress.ToArray();
                                using (var sw = new StreamWriter(GetCurrentPath() + GetBlockchainWalletCachePath() +
                                                                 BlockchainWalletCacheDatabase, true, Encoding.UTF8,
                                        8192)
                                { AutoFlush = true })
                                {
                                    foreach (var wallet in walletCache)
                                    {
                                        sw.Write(wallet.Key + "\n");
                                    }
                                }

                                TotalWalletCacheSaved = walletCache.Length;
                                Array.Clear(walletCache, 0, walletCache.Length);
                            }
                        }

                        InSaveWalletCacheDatabase = false;

                    }
                    catch (Exception error)
                    {
#if DEBUG
                        Console.WriteLine("Can't save wallet cache to database file: " + error.Message);
#endif
                        ClearWalletCacheSave();
                    }
                }
            }

            return true;
        }

        /// <summary>
        ///     Force to clear blocks saved.
        /// </summary>
        public static void ClearBlockSyncSave()
        {
            try
            {
                if (BlockchainBlockWriter != null)
                {
                    BlockchainBlockWriter?.Close();
                    BlockchainBlockWriter?.Dispose();
                    BlockchainBlockWriter = null;
                }

                TotalBlockSaved = 0;
                File.Create(GetCurrentPath() + GetBlockchainBlockPath() + BlockchainBlockDatabase).Close();
                InSaveBlockDatabase = false;
            }
            catch
            {
            }
        }


        /// <summary>
        ///     Force to clear transactions saved.
        /// </summary>
        public static void ClearTransactionSyncSave()
        {
            try
            {
                if (BlockchainTransactionWriter != null)
                {
                    BlockchainTransactionWriter?.Close();
                    BlockchainTransactionWriter?.Dispose();
                    BlockchainTransactionWriter = null;
                }

                TotalTransactionSaved = 0;
                File.Create(GetCurrentPath() + GetBlockchainTransactionPath() + BlockchainTransactonDatabase).Close();
                InSaveTransactionDatabase = false;
            }
            catch
            {
            }
        }

        /// <summary>
        /// Force to clear wallet cache saved.
        /// </summary>
        public static void ClearWalletCacheSave()
        {
            try
            {
                if (BlockchainWalletCacheWriter != null)
                {
                    BlockchainWalletCacheWriter?.Close();
                    BlockchainWalletCacheWriter?.Dispose();
                    BlockchainWalletCacheWriter = null;
                }
                TotalWalletCacheSaved = 0;
                File.Create(GetCurrentPath() + GetBlockchainWalletCachePath() + BlockchainWalletCacheDatabase).Close();
                InSaveWalletCacheDatabase = false;
            }
            catch
            {

            }
        }
    }
}