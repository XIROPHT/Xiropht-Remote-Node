using System;
#if DEBUG
using System.Diagnostics;
#endif
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xiropht_RemoteNode.Data;
using Xiropht_RemoteNode.Object;
using Xiropht_RemoteNode.Utils;

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

        private static StreamWriter _blockchainTransactionWriter;
        private static StreamWriter _blockchainBlockWriter;
        private static StreamWriter _blockchainWalletCacheWriter;

        public static bool InSaveTransactionDatabase;
        public static bool InSaveBlockDatabase;
        public static bool InSaveWalletCacheDatabase;

        public static long TotalTransactionSaved;
        public static int TotalBlockSaved;
        public static int TotalWalletCacheSaved;


        private static CancellationTokenSource _cancellationTokenSaveTransaction;

        private static CancellationTokenSource _cancellationTokenSaveBlock;

        private static CancellationTokenSource _cancellationTokenSaveWalletCache;

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
                if (Program.RemoteNodeSettingObject.enable_save_sync_raw)
                {
                    Console.WriteLine("Load transaction database file - RAW data syntax enabled..");
                }
                else
                {
                    Console.WriteLine("Load transaction database file - JSON Data syntax enabled..");
                }
                long counter = 0;
                try
                {
                    bool error = false;

                    using (FileStream fs = File.Open(GetCurrentPath() + GetBlockchainTransactionPath() + BlockchainTransactonDatabase, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    using (BufferedStream bs = new BufferedStream(fs))
                    using (StreamReader sr = new StreamReader(bs))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            try
                            {
                                if (Program.RemoteNodeSettingObject.enable_save_sync_raw)
                                {
                                    var splitTransactionLine = line.Split(new[] { "%" }, StringSplitOptions.None);
                                    long transactionId = long.Parse(splitTransactionLine[0]);
                                    if (counter == transactionId)
                                    {
                                        if (ClassRemoteNodeSortingTransactionPerWallet.AddNewTransactionSortedPerWallet(splitTransactionLine[1], transactionId))
                                        {
                                            ClassRemoteNodeSync.ListOfTransaction.InsertTransaction(transactionId, splitTransactionLine[1]);
                                        }
                                    }
                                    else
                                    {
                                        error = true;
                                        break;
                                    }
                                }
                                else
                                {
                                    var transactionObject = JsonConvert.DeserializeObject<ClassTransactionObject>(line);
                                    if (counter == transactionObject.transaction_id)
                                    {
                                        string transactionRaw = ClassTransactionUtility.BuildTransactionRaw(transactionObject);
                                        if (ClassRemoteNodeSortingTransactionPerWallet.AddNewTransactionSortedPerWallet(transactionRaw, transactionObject.transaction_id))
                                        {
                                            ClassRemoteNodeSync.ListOfTransaction.InsertTransaction(transactionObject.transaction_id, transactionRaw);
                                        }
                                    }
                                    else
                                    {
                                        error = true;
                                        break;
                                    }
                                }
                            }
                            catch
                            {
                                error = true;
                                break;
                            }
                            counter++;
                        }
                    }
                    if (error)
                    {
                        Console.WriteLine("Transaction database seems corrupted, start to clear transaction database and resync..");
                        ClassRemoteNodeSync.ListOfTransaction.Clear();
                        ClassRemoteNodeSync.ListOfTransactionHash.Clear();
                        ClassRemoteNodeSync.ListTransactionPerWallet.Clear();
                        ClearTransactionSyncSave();
                        ClassUtilsNode.ClearGc();
                        return true;
                    }
                }
                catch
                {
                    ClassUtilsNode.ClearGc();
                    return false;
                }

                TotalTransactionSaved = counter;
                Console.WriteLine(counter + " transaction successfully loaded and included on memory..");
            }
            else
            {
                File.Create(GetCurrentPath() + GetBlockchainTransactionPath() + BlockchainTransactonDatabase).Close();
            }

            ClassUtilsNode.ClearGc();
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

                bool error = false;

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
                                try
                                {
                                    counter++;
                                    var splitLineBlock = line.Split(new[] { "#" }, StringSplitOptions.None);
                                    long blockId = long.Parse(splitLineBlock[0]);
                                    if (blockId == counter)
                                    {
                                        if (!ClassRemoteNodeSync.ListOfBlock.ContainsKey(blockId - 1))
                                        {
                                            string blockHash = splitLineBlock[1];
                                            if (ClassRemoteNodeSync.ListOfBlockHash.GetBlockIdFromHash(blockHash) == -1)
                                            {
                                                ClassRemoteNodeSync.ListOfBlock.Add(blockId - 1, line);
                                                ClassRemoteNodeSync.ListOfBlockHash.InsertBlockHash(blockHash, blockId - 1);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        error = true;
                                        break;
                                    }
                                }
                                catch
                                {
                                    error = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                if (error)
                {
                    Console.WriteLine("Block database seems corrupted, start to clear block database and resync..");
                    ClassRemoteNodeSync.ListOfBlock.Clear();
                    ClassRemoteNodeSync.ListOfBlockHash.Clear();
                    ClearBlockSyncSave();
                    ClassUtilsNode.ClearGc();
                    return;
                }

                TotalBlockSaved = counter;
                Console.WriteLine(counter + " block successfully loaded and included on memory..");
                ClassUtilsNode.ClearGc();
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
                ClassUtilsNode.ClearGc();
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
                if (auto)
                {
                    if (_cancellationTokenSaveTransaction != null)
                    {
                        try
                        {
                            if (!_cancellationTokenSaveTransaction.IsCancellationRequested)
                            {
                                _cancellationTokenSaveTransaction.Cancel();
                            }
                        }
                        catch
                        {
                            // Ignored.
                        }
                    }
                    _cancellationTokenSaveTransaction = new CancellationTokenSource();
                    try
                    {
                        Task.Factory.StartNew(async delegate
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

                                    if (_blockchainTransactionWriter == null)
                                    {
                                        _blockchainTransactionWriter = new StreamWriter(GetCurrentPath() + GetBlockchainTransactionPath() + BlockchainTransactonDatabase, true, Encoding.UTF8, 8192) { AutoFlush = true };
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
                                                        var transactionObject = ClassRemoteNodeSync.ListOfTransaction.GetTransaction(i);
                                                        if (Program.RemoteNodeSettingObject.enable_save_sync_raw)
                                                        {
                                                            _blockchainTransactionWriter.WriteLine(transactionObject.Item2+"%"+transactionObject.Item1);
                                                        }
                                                        else
                                                        {
                                                            _blockchainTransactionWriter.WriteLine(JsonConvert.SerializeObject(ClassTransactionUtility.BuildTransactionObjectFromRaw(transactionObject.Item2, transactionObject.Item1), Formatting.None));
                                                        }
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
                                    Debug.WriteLine("Can't save transaction(s) to database file: " + error.Message);
#endif
                                    ClearTransactionSyncSave();
                                }

                                await Task.Delay(1000);
                            }
                        }, _cancellationTokenSaveTransaction.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current).ConfigureAwait(false);
                    }
                    catch
                    {
                        // Catch the exception once the task is cancelled.
                    }
                }
                else
                {
                    if (_cancellationTokenSaveTransaction != null)
                    {
                        try
                        {
                            if (!_cancellationTokenSaveTransaction.IsCancellationRequested)
                            {
                                _cancellationTokenSaveTransaction.Cancel();
                            }
                        }
                        catch
                        {
                            // Ignored.
                        }
                    }
                    try
                    {
                        if (_blockchainTransactionWriter != null)
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
                                    using (var sw = new StreamWriter(GetCurrentPath() + GetBlockchainTransactionPath() + BlockchainTransactonDatabase, true, Encoding.UTF8, 8192) { AutoFlush = true })
                                    {
                                        for (var i = 0; i < ClassRemoteNodeSync.ListOfTransaction.Count; i++)
                                        {
                                            if (ClassRemoteNodeSync.ListOfTransaction.ContainsKey(i))
                                            {
                                                var transactionObject = ClassRemoteNodeSync.ListOfTransaction.GetTransaction(i);
                                                if (Program.RemoteNodeSettingObject.enable_save_sync_raw)
                                                {
                                                    sw.WriteLine(transactionObject.Item2 + "%" + transactionObject.Item1);
                                                }
                                                else
                                                {
                                                    sw.WriteLine(JsonConvert.SerializeObject(ClassTransactionUtility.BuildTransactionObjectFromRaw(transactionObject.Item2, transactionObject.Item1), Formatting.None));
                                                }
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
                        Debug.WriteLine("Can't save transaction(s) to database file: " + error.Message);
#endif
                        ClearTransactionSyncSave();
                    }
                }
            }

            return true;
        }

        /// <summary>
        ///  Force to save block.
        /// </summary>
        public static bool SaveBlock(bool auto = true)
        {
            if (!InSaveBlockDatabase)
            {

                if (auto)
                {
                    if (_cancellationTokenSaveBlock != null)
                    {
                        try
                        {
                            if (!_cancellationTokenSaveBlock.IsCancellationRequested)
                            {
                                _cancellationTokenSaveBlock.Cancel();
                            }
                        }
                        catch
                        {
                            // Ignored.
                        }
                    }
                    _cancellationTokenSaveBlock = new CancellationTokenSource();

                    try
                    {
                        Task.Factory.StartNew(async delegate
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

                                    if (_blockchainBlockWriter == null)
                                    {
                                        _blockchainBlockWriter =
                                            new StreamWriter(
                                                GetCurrentPath() + GetBlockchainBlockPath() + BlockchainBlockDatabase, true,
                                                Encoding.UTF8, 8192)
                                            { AutoFlush = true };
                                    }


                                    InSaveBlockDatabase = true;
                                    if (ClassRemoteNodeSync.ListOfBlock != null)
                                        if (ClassRemoteNodeSync.ListOfBlock.Count > 0)
                                        {
                                            if (TotalBlockSaved != ClassRemoteNodeSync.ListOfBlock.Count)
                                            {
                                                for (var i = TotalBlockSaved; i < ClassRemoteNodeSync.ListOfBlock.Count; i++)
                                                {
                                                    if (ClassRemoteNodeSync.ListOfBlock.ContainsKey(i))
                                                    {
                                                        _blockchainBlockWriter.WriteLine(ClassRemoteNodeSync.ListOfBlock[i]);
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

                                await Task.Delay(1000);
                            }
                        }, _cancellationTokenSaveBlock.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current).ConfigureAwait(false);
                    }
                    catch
                    {
                        // Catch the exception once the task is cancelled.
                    }
                }
                else
                {
                    if (_cancellationTokenSaveBlock != null)
                    {
                        try
                        {
                            if (!_cancellationTokenSaveBlock.IsCancellationRequested)
                            {
                                _cancellationTokenSaveBlock.Cancel();
                            }
                        }
                        catch
                        {
                            // Ignored.
                        }
                    }
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
                                { AutoFlush = true })
                                {
                                    for (var i = 0; i < ClassRemoteNodeSync.ListOfBlock.Count; i++)
                                    {
                                        if (ClassRemoteNodeSync.ListOfBlock.ContainsKey(i))
                                        {
                                            sw.WriteLine(ClassRemoteNodeSync.ListOfBlock[i]);
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
                        Debug.WriteLine("Can't save block(s) to database file: " + error.Message);
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

                if (auto)
                {
                    if (_cancellationTokenSaveWalletCache != null)
                    {
                        try
                        {
                            if (!_cancellationTokenSaveWalletCache.IsCancellationRequested)
                            {
                                _cancellationTokenSaveWalletCache.Cancel();
                            }
                        }
                        catch
                        {
                            // Ignored.
                        }
                    }
                    _cancellationTokenSaveWalletCache = new CancellationTokenSource();

                    try
                    {
                        Task.Factory.StartNew(async delegate
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

                                    if (_blockchainWalletCacheWriter == null)
                                    {
                                        _blockchainWalletCacheWriter =
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

                                                    _blockchainWalletCacheWriter.WriteLine(walletCache[i].Key);

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
                                    Debug.WriteLine("Can't save wallet cache to database file: " + error.Message);
#endif
                                    ClearWalletCacheSave();

                                }

                                await Task.Delay(1000);
                            }
                        }, _cancellationTokenSaveWalletCache.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current).ConfigureAwait(false);
                    }
                    catch
                    {
                        // Catch the exception once the task is cancelled.
                    }
                }
                else
                {
                    if (_cancellationTokenSaveWalletCache != null)
                    {
                        try
                        {
                            if (!_cancellationTokenSaveWalletCache.IsCancellationRequested)
                            {
                                _cancellationTokenSaveWalletCache.Cancel();
                            }
                        }
                        catch
                        {
                            // Ignored.
                        }
                    }
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
                                using (var sw = new StreamWriter(GetCurrentPath() + GetBlockchainWalletCachePath() + BlockchainWalletCacheDatabase, true, Encoding.UTF8, 8192)
                                { AutoFlush = true })
                                {
                                    foreach (var wallet in walletCache)
                                    {
                                        sw.WriteLine(wallet.Key);
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
                        Debug.WriteLine("Can't save wallet cache to database file: " + error.Message);
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
                if (_blockchainBlockWriter != null)
                {
                    _blockchainBlockWriter?.Close();
                    _blockchainBlockWriter?.Dispose();
                    _blockchainBlockWriter = null;
                }

                TotalBlockSaved = 0;
                if (File.Exists(GetCurrentPath() + GetBlockchainBlockPath() + BlockchainBlockDatabase))
                {
                    File.Delete(GetCurrentPath() + GetBlockchainBlockPath() + BlockchainBlockDatabase);
                }
                File.Create(GetCurrentPath() + GetBlockchainBlockPath() + BlockchainBlockDatabase).Close();
                InSaveBlockDatabase = false;
            }
            catch
            {
                // Ignored.
            }
        }

        /// <summary>
        ///     Force to clear transactions saved.
        /// </summary>
        public static void ClearTransactionSyncSave()
        {
            try
            {
                if (_blockchainTransactionWriter != null)
                {
                    _blockchainTransactionWriter?.Close();
                    _blockchainTransactionWriter?.Dispose();
                    _blockchainTransactionWriter = null;
                }

                TotalTransactionSaved = 0;
                if (File.Exists(GetCurrentPath() + GetBlockchainTransactionPath() + BlockchainTransactonDatabase))
                {
                    File.Delete(GetCurrentPath() + GetBlockchainTransactionPath() + BlockchainTransactonDatabase);
                }
                File.Create(GetCurrentPath() + GetBlockchainTransactionPath() + BlockchainTransactonDatabase).Close();
                InSaveTransactionDatabase = false;
            }
            catch
            {
                // Ignored.
            }
        }

        /// <summary>
        /// Force to clear wallet cache saved.
        /// </summary>
        public static void ClearWalletCacheSave()
        {
            try
            {
                if (_blockchainWalletCacheWriter != null)
                {
                    _blockchainWalletCacheWriter?.Close();
                    _blockchainWalletCacheWriter?.Dispose();
                    _blockchainWalletCacheWriter = null;
                }
                TotalWalletCacheSaved = 0;
                if (File.Exists(GetCurrentPath() + GetBlockchainWalletCachePath() + BlockchainWalletCacheDatabase))
                {
                    File.Delete(GetCurrentPath() + GetBlockchainWalletCachePath() + BlockchainWalletCacheDatabase);
                }
                File.Create(GetCurrentPath() + GetBlockchainWalletCachePath() + BlockchainWalletCacheDatabase).Close();
                InSaveWalletCacheDatabase = false;
            }
            catch
            {
                // Ignored.
            }
        }
    }
}