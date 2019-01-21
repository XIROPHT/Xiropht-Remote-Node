using System;
using System.IO;
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


        public static bool InSaveTransactionDatabase;
        public static bool InSaveBlockDatabase;

        public static int TotalTransactionSaved;
        public static int TotalBlockSaved;
        public static string DataTransactionSaved;
        public static string DataBlockSaved;


        private static Thread _threadAutoSaveTransaction;

        private static Thread _threadAutoSaveBlock;

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
        }


        /// <summary>
        ///     Get Current Path of the program.
        /// </summary>
        /// <returns></returns>
        private static string GetCurrentPath()
        {
            var path = Directory.GetCurrentDirectory();
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

                using (FileStream fs = File.Open(GetCurrentPath() + GetBlockchainTransactionPath() + BlockchainTransactonDatabase, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (BufferedStream bs = new BufferedStream(fs))
                using (StreamReader sr = new StreamReader(bs))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        counter++;
                        ClassRemoteNodeSync.ListOfTransaction.Add(ClassRemoteNodeSync.ListOfTransaction.Count, line);
                        try
                        {
                            ClassRemoteNodeSortingTransactionPerWallet.AddNewTransactionSortedPerWallet(line);
                        }
                        catch
                        {
                            ClassRemoteNodeSync.ListOfTransaction.Clear();
                            return false;
                        }
                    }
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


                using (FileStream fs = File.Open(GetCurrentPath() + GetBlockchainBlockPath() + BlockchainBlockDatabase, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BufferedStream bs = new BufferedStream(fs))
                    {
                        using (StreamReader sr = new StreamReader(bs))
                        {
                            string line;
                            while ((line = sr.ReadLine()) != null)
                            {
                                counter++;
                                ClassRemoteNodeSync.ListOfBlock.Add(ClassRemoteNodeSync.ListOfBlock.Count, line);
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
        /// Force to save transaction.
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
                    _threadAutoSaveTransaction = new Thread(delegate ()
                    {
                        try
                        {

                            if (!File.Exists(GetCurrentPath() + GetBlockchainBlockPath() +
                                            BlockchainTransactonDatabase))
                            {
                                File.Create(GetCurrentPath() + GetBlockchainBlockPath() +
                                            BlockchainTransactonDatabase).Close();
                            }


                            InSaveTransactionDatabase = true;

                            if (ClassRemoteNodeSync.ListOfTransaction != null)
                                if (ClassRemoteNodeSync.ListOfTransaction.Count > 0)
                                {

                                    if (TotalTransactionSaved != ClassRemoteNodeSync.ListOfTransaction.Count)
                                    {
                                        var sw = new StreamWriter(File.OpenWrite(GetCurrentPath() + GetBlockchainTransactionPath() +
                                                                             BlockchainTransactonDatabase));
                                        for (var i = TotalTransactionSaved; i < ClassRemoteNodeSync.ListOfTransaction.Count; i++)
                                        {
                                            if (i < ClassRemoteNodeSync.ListOfTransaction.Count)
                                            {
                                                if (!string.IsNullOrEmpty(ClassRemoteNodeSync.ListOfTransaction[i]))
                                                {



                                                    sw.Write(ClassRemoteNodeSync.ListOfTransaction[i] + "\n");
                                                    sw.Flush();

                                                }
                                            }
                                        }
                                        sw.Close();

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
                        InSaveTransactionDatabase = false;
                            TotalTransactionSaved = 0;

                        }
                    });
                    _threadAutoSaveTransaction.Start();
                }
                else
                {
                    try
                    {


                        File.Create(GetCurrentPath() + GetBlockchainBlockPath() +
                                    BlockchainTransactonDatabase).Close();


                        if (!InSaveTransactionDatabase)
                        {
                            InSaveTransactionDatabase = true;

                            if (ClassRemoteNodeSync.ListOfTransaction != null)
                                if (ClassRemoteNodeSync.ListOfTransaction.Count > 0)
                                {

                                    var sw = new StreamWriter(File.OpenWrite(GetCurrentPath() + GetBlockchainTransactionPath() +
                                                                                 BlockchainTransactonDatabase));
                                    for (var i = 0; i < ClassRemoteNodeSync.ListOfTransaction.Count; i++)
                                    {
                                        if (i < ClassRemoteNodeSync.ListOfTransaction.Count)
                                        {
                                            if (!string.IsNullOrEmpty(ClassRemoteNodeSync.ListOfTransaction[i]))
                                            {



                                                sw.Write(ClassRemoteNodeSync.ListOfTransaction[i] + "\n");
                                                sw.Flush();

                                            }
                                        }
                                    }
                                    sw.Close();

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
                        InSaveTransactionDatabase = false;
                        TotalTransactionSaved = 0;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Force to save block.
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
                    _threadAutoSaveBlock = new Thread(delegate ()
                    {
                        try
                        {

                            if (!File.Exists(GetCurrentPath() + GetBlockchainBlockPath() +
                                            BlockchainBlockDatabase))
                            {
                                File.Create(GetCurrentPath() + GetBlockchainBlockPath() +
                                            BlockchainBlockDatabase).Close();
                            }


                            InSaveBlockDatabase = true;
                            if (ClassRemoteNodeSync.ListOfBlock != null)
                                if (ClassRemoteNodeSync.ListOfBlock.Count > 0)
                                {

                                    if (TotalBlockSaved != ClassRemoteNodeSync.ListOfBlock.Count)
                                    {
                                        var sw = new StreamWriter(File.OpenWrite(GetCurrentPath() + GetBlockchainBlockPath() +
                                                                                 BlockchainBlockDatabase));
                                        for (var i = TotalBlockSaved; i < ClassRemoteNodeSync.ListOfBlock.Count; i++)
                                        {
                                            if (i < ClassRemoteNodeSync.ListOfBlock.Count)
                                            {
                                                if (!string.IsNullOrEmpty(ClassRemoteNodeSync.ListOfBlock[i]))
                                                {



                                                    sw.Write(ClassRemoteNodeSync.ListOfBlock[i] + "\n");
                                                    sw.Flush();

                                                }
                                            }
                                        }
                                        sw.Close();

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
                            InSaveBlockDatabase = false;
                            TotalBlockSaved = 0;
                        }
                    });
                    _threadAutoSaveBlock.Start();
                }
                else
                {
                    try
                    {

                        File.Create(GetCurrentPath() + GetBlockchainBlockPath() +
                                    BlockchainBlockDatabase).Close();



                        InSaveBlockDatabase = true;
                        if (ClassRemoteNodeSync.ListOfBlock != null)
                        {
                            if (ClassRemoteNodeSync.ListOfBlock.Count > 0)
                            {


                                var sw = new StreamWriter(File.OpenWrite(GetCurrentPath() + GetBlockchainBlockPath() +
                                                                         BlockchainBlockDatabase));
                                for (var i = 0; i < ClassRemoteNodeSync.ListOfBlock.Count; i++)
                                {
                                    if (i < ClassRemoteNodeSync.ListOfBlock.Count)
                                    {
                                        if (!string.IsNullOrEmpty(ClassRemoteNodeSync.ListOfBlock[i]))
                                        {



                                            sw.Write(ClassRemoteNodeSync.ListOfBlock[i] + "\n");
                                            sw.Flush();

                                        }
                                    }
                                }
                                sw.Close();

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
                        InSaveBlockDatabase = false;
                    }
                }
            }
            return true;
        }
    }
}