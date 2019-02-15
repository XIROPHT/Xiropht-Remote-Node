using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Xiropht_Connector_All.Setting;
using Xiropht_Connector_All.Utils;
using Xiropht_RemoteNode.Api;
using Xiropht_RemoteNode.Command;
using Xiropht_RemoteNode.Data;
using Xiropht_RemoteNode.RemoteNode;
using Xiropht_RemoteNode.Utils;

namespace Xiropht_RemoteNode
{
    public class Program
    {
        public static List<ClassRemoteNodeObject>
            RemoteNodeObjectCoinMaxSupply; // Sync the node for get coin max supply information.

        public static List<ClassRemoteNodeObject>
            RemoteNodeObjectCoinCirculating; // Sync the node for get coin circulating information.

        public static List<ClassRemoteNodeObject>
            RemoteNodeObjectTotalBlockMined; // Sync the node for get total block mined information.

        public static List<ClassRemoteNodeObject>
            RemoteNodeObjectTotalPendingTransaction; // Sync the node for get total pending transaction information.

        public static List<ClassRemoteNodeObject>
            RemoteNodeObjectCurrentDifficulty; // Sync the node for get current mining difficulty information.

        public static List<ClassRemoteNodeObject>
            RemoteNodeObjectCurrentRate; // Sync the node for get current mining hashrate information.

        public static ClassRemoteNodeObject
            RemoteNodeObjectToBePublic; // Sync the node for get the public node list information and ask to be public if not.

        public static ClassRemoteNodeObject
            RemoteNodeObjectTransaction; // Sync the node for get each transaction data information.

        public static List<ClassRemoteNodeObject>
            RemoteNodeObjectTotalFee; // Sync the node for get current amount of fee information.

        public static ClassRemoteNodeObject RemoteNodeObjectBlock; // Sync the node for get each block data information.

        public static List<ClassRemoteNodeObject>
            RemoteNodeObjectTotalTransaction; // Sync the node for get total number of transaction information.

        public static string RemoteNodeWalletAddress; // Wallet Address of the owner.
        private static Thread _threadCommandLine;
        public static int LogLevel;
        public static string Certificate;
        public static CultureInfo GlobalCultureInfo = new CultureInfo("fr-FR");
        public static bool Closed;
        public static int TotalConnectionSync;

        private static string ConfigFilePath = "\\config.ini";
        private static bool EnableApiHttp;


        public static void Main(string[] args)
        {
            Thread.CurrentThread.Name = Path.GetFileName(Environment.GetCommandLineArgs()[0]);

            ClassRemoteNodeSave.InitializePath();
            if (ClassRemoteNodeSave.LoadBlockchainTransaction())
            {
                ClassRemoteNodeSave.LoadBlockchainBlock();
            }
            else
            {
                Console.WriteLine("Blockchain database corrupted, clean up..");
                ClassRemoteNodeSync.ListOfTransaction.Clear();
                ClassRemoteNodeSync.ListOfBlock.Clear();
                ClassRemoteNodeSync.ListTransactionPerWallet.Clear();
                Thread.Sleep(2000);
            }
            Console.WriteLine("Remote node Xiropht - " + Assembly.GetExecutingAssembly().GetName().Version + "b");


            if (File.Exists(ClassUtilsNode.ConvertPath(Directory.GetCurrentDirectory() + ConfigFilePath)))
            {
                ReadConfigFile();
            }
            else
            {
                Console.WriteLine(
                    "Welcome, please write your wallet address, in a near future public remote nodes will get reward: ");
                RemoteNodeWalletAddress = Console.ReadLine();

                Console.WriteLine("Do you want load your node as a Public Remote Node? [Y/N]");
                var answer = Console.ReadLine();
                if (answer == "Y" || answer == "y")
                {
                    Console.WriteLine("Be carefull, you need to open the default port " +
                                      ClassConnectorSetting.RemoteNodePort + " of your remote node in your router.");
                    Console.WriteLine(
                        "Your port need to be opened for everyone and not only for Seed Nodes, for proceed test of your sync.");
                    Console.WriteLine("If everything is alright, your remote node will be listed in the public list.");
                    Console.WriteLine(
                        "If informations of your sync are not right, your remote node will be not listed.");
                    Console.WriteLine(
                        "Checking by Seed Nodes of your Remote Node work everytime for be sure your node is legit and can be rewarded.");
                    Console.WriteLine("");
                    Console.WriteLine("Are you sure to enable this mode? [Y/N]");
                    answer = Console.ReadLine();
                    if (answer == "Y" || answer == "y")
                    {
                        Console.WriteLine("Enabling public remote node system..");
                        ClassRemoteNodeSync.WantToBePublicNode = true;
                    }
                }

                Console.WriteLine("Do you to enable the HTTP API ? [Y/N]");
                answer = Console.ReadLine();
                if (answer == "Y" || answer == "y")
                {
                    EnableApiHttp = true;
                    Console.WriteLine("Do you want to enable HTTPS mode ? [Y/N]");
                    answer = Console.ReadLine();
                    if (answer == "Y" || answer == "y")
                    {
                        ClassApiHttp.UseSSL = true;
                        Console.WriteLine("Write your ssl certificate path, including the file name: ");
                        ClassApiHttp.SSLPath = Console.ReadLine();
                    }
                }
                SaveConfigFile();

            }

            TotalConnectionSync = 1;


            Certificate = ClassUtils.GenerateCertificate();
            Console.WriteLine("Initialize Remote Node Sync Objects..");

            RemoteNodeObjectToBePublic = new ClassRemoteNodeObject(SyncEnumerationObject.ObjectToBePublic);
            RemoteNodeObjectCoinMaxSupply = new List<ClassRemoteNodeObject>();
            RemoteNodeObjectCoinCirculating = new List<ClassRemoteNodeObject>();
            RemoteNodeObjectTotalBlockMined = new List<ClassRemoteNodeObject>();
            RemoteNodeObjectTotalPendingTransaction = new List<ClassRemoteNodeObject>();
            RemoteNodeObjectCurrentDifficulty = new List<ClassRemoteNodeObject>();
            RemoteNodeObjectCurrentRate = new List<ClassRemoteNodeObject>();
            RemoteNodeObjectTotalFee = new List<ClassRemoteNodeObject>();
            RemoteNodeObjectTotalTransaction = new List<ClassRemoteNodeObject>();
            RemoteNodeObjectTransaction = new ClassRemoteNodeObject(SyncEnumerationObject.ObjectTransaction, 0);
            for (int i = 0; i < TotalConnectionSync; i++)
            {
                RemoteNodeObjectCoinMaxSupply.Add(new ClassRemoteNodeObject(SyncEnumerationObject.ObjectCoinSupply, i));
                RemoteNodeObjectCoinCirculating.Add(new ClassRemoteNodeObject(SyncEnumerationObject.ObjectCoinCirculating, i));
                RemoteNodeObjectTotalBlockMined.Add(new ClassRemoteNodeObject(SyncEnumerationObject.ObjectBlockMined, i));
                RemoteNodeObjectTotalPendingTransaction.Add(new ClassRemoteNodeObject(SyncEnumerationObject.ObjectPendingTransaction, i));
                RemoteNodeObjectCurrentDifficulty.Add(new ClassRemoteNodeObject(SyncEnumerationObject.ObjectCurrentDifficulty, i));
                RemoteNodeObjectCurrentRate.Add(new ClassRemoteNodeObject(SyncEnumerationObject.ObjectCurrentRate, i));
                RemoteNodeObjectTotalFee.Add(new ClassRemoteNodeObject(SyncEnumerationObject.ObjectTotalFee, i));
                RemoteNodeObjectTotalTransaction.Add(new ClassRemoteNodeObject(SyncEnumerationObject.ObjectTotalTransaction, i));
            }
            RemoteNodeObjectBlock = new ClassRemoteNodeObject(SyncEnumerationObject.ObjectBlock);
            ClassCheckRemoteNodeSync.AutoCheckBlockchainNetwork();

            Task.Run(async delegate ()
                {
                    ClassCheckRemoteNodeSync.AutoCheckBlockchainNetwork();
                    if (!ClassCheckRemoteNodeSync.BlockchainNetworkStatus)
                    {
                        while (!ClassCheckRemoteNodeSync.BlockchainNetworkStatus)
                        {
                            Console.WriteLine("Blockchain network is not available. Check again after 1 seconds.");
                            await Task.Delay(1000);
                        }
                    }
                    var initializeConnection = false;
                    while (!initializeConnection)
                    {
                        Console.WriteLine("Start Remote Node Sync Objects Connection..");

                        for (int i = 0; i < RemoteNodeObjectCoinMaxSupply.Count; i++)
                        {

                            if (await RemoteNodeObjectCoinMaxSupply[i].StartConnectionAsync())
                            {
                                await Task.Delay(10);
                            }
                            if (await RemoteNodeObjectCoinCirculating[i].StartConnectionAsync())
                            {
                                await Task.Delay(10);
                            }
                            if (await RemoteNodeObjectTotalBlockMined[i].StartConnectionAsync())
                            {
                                await Task.Delay(10);
                            }
                            if (await RemoteNodeObjectTotalPendingTransaction[i].StartConnectionAsync())
                            {
                                await Task.Delay(10);
                            }
                            if (await RemoteNodeObjectCurrentDifficulty[i].StartConnectionAsync())
                            {
                                await Task.Delay(10);
                            }
                            if (await RemoteNodeObjectCurrentRate[i].StartConnectionAsync())
                            {
                                await Task.Delay(10);
                            }

                            if (await RemoteNodeObjectTotalFee[i].StartConnectionAsync())
                            {
                                await Task.Delay(10);
                            }
                            if (await RemoteNodeObjectTotalTransaction[i].StartConnectionAsync())
                            {
                                await Task.Delay(10);
                            }

                        }

                        await Task.Delay(100);
                        if (await RemoteNodeObjectTransaction.StartConnectionAsync())
                        {
                            await Task.Delay(10);

                            if (await RemoteNodeObjectBlock.StartConnectionAsync())
                            {
                                if (ClassRemoteNodeSync.WantToBePublicNode)
                                {
                                    if (await RemoteNodeObjectToBePublic.StartConnectionAsync())
                                    {
                                        initializeConnection = true;
                                    }
                                }
                                else
                                {
                                    initializeConnection = true;
                                }
                            }
                        }

                    }

                    if (initializeConnection)
                    {
                        Console.WriteLine("Remote node objects successfully connected.");
                    }
                    else
                    {
                        Console.WriteLine("Remote node objects can't connect to the network, retry in 10 seconds..");
                        ClassCheckRemoteNodeSync.DisableCheckRemoteNodeSync();
                        RemoteNodeObjectBlock.StopConnection();
                        RemoteNodeObjectToBePublic.StopConnection();
                        RemoteNodeObjectTransaction.StopConnection();
                        for (int i = 0; i < TotalConnectionSync; i++)
                        {
                            if (i < TotalConnectionSync)
                            {
                                RemoteNodeObjectCoinCirculating[i].StopConnection();
                                RemoteNodeObjectCoinMaxSupply[i].StopConnection();
                                RemoteNodeObjectCurrentDifficulty[i].StopConnection();
                                RemoteNodeObjectCurrentRate[i].StopConnection();
                                RemoteNodeObjectTotalBlockMined[i].StopConnection();
                                RemoteNodeObjectTotalFee[i].StopConnection();
                                RemoteNodeObjectTotalPendingTransaction[i].StopConnection();
                                RemoteNodeObjectTotalTransaction[i].StopConnection();

                            }
                        }
                        await Task.Delay(10000);
                    }



                    Console.WriteLine("Enable Check Remote Node Objects connection..");
                    ClassCheckRemoteNodeSync.EnableCheckRemoteNodeSync();
                    Console.WriteLine("Enable System of Generating Trusted Key's of Remote Node..");
                    ClassRemoteNodeKey.StartUpdateHashTransactionList();
                    ClassRemoteNodeKey.StartUpdateHashBlockList();
                    ClassRemoteNodeKey.StartUpdateTrustedKey();

                    Console.WriteLine("Enable API..");
                    ClassApi.StartApiRemoteNode();
                    if (EnableApiHttp)
                    {
                        Console.WriteLine("Enable API HTTP..");
                        ClassApiHttp.StartApiHttpServer();
                    }
                }).ConfigureAwait(true);


            _threadCommandLine = new Thread(delegate ()
            {
                while (!ClassApi.ApiReceiveConnectionStatus)
                {
                    Thread.Sleep(100);
                }
                Console.WriteLine(
                    "Remote node successfully started, you can run command: help for get the list of commands.");
                var exit = false;
                while (!exit)
                {
                    try
                    {
                        if (!ClassCommandLine.CommandLine(Console.ReadLine()))
                        {
                            exit = true;
                        }
                    }
                    catch
                    {

                    }
                }
            });
            _threadCommandLine.Start();

        }

        private static void SaveConfigFile()
        {
            Console.WriteLine("Save config file..");
            File.Create(ClassUtilsNode.ConvertPath(Directory.GetCurrentDirectory() + ConfigFilePath)).Close();
            using (StreamWriter writer = new StreamWriter(ClassUtilsNode.ConvertPath(Directory.GetCurrentDirectory() + ConfigFilePath)) { AutoFlush = true })
            {
                writer.WriteLine("WALLET_ADDRESS=" + RemoteNodeWalletAddress);
                if (ClassRemoteNodeSync.WantToBePublicNode)
                {
                    writer.WriteLine("ENABLE_PUBLIC_MODE=Y");
                }
                else
                {
                    writer.WriteLine("ENABLE_PUBLIC_MODE=N");
                }
                if (EnableApiHttp)
                {
                    writer.WriteLine("ENABLE_API_HTTP=Y");
                }
                else
                {
                    writer.WriteLine("ENABLE_API_HTTP=N");
                }
                if (ClassApiHttp.UseSSL)
                {
                    writer.WriteLine("ENABLE_HTTPS_API_MODE=Y");
                }
                else
                {
                    writer.WriteLine("ENABLE_HTTPS_API_MODE=N");
                }
                if (!string.IsNullOrEmpty(ClassApiHttp.SSLPath))
                {
                    writer.WriteLine("HTTPS_CERTIFICATE_PATH=" + ClassApiHttp.SSLPath);
                }
                else
                {
                    writer.WriteLine("HTTPS_CERTIFICATE_PATH=");
                }
                writer.WriteLine("LOG_LEVEL=" + LogLevel);
            }
            Console.WriteLine("Config file saved.");
        }

        /// <summary>
        /// Read config file.
        /// </summary>
        private static void ReadConfigFile()
        {
            StreamReader reader = new StreamReader(ClassUtilsNode.ConvertPath(Directory.GetCurrentDirectory() + ConfigFilePath));

            string line = string.Empty;

            while ((line = reader.ReadLine()) != null)
            {
                if (line.Contains("WALLET_ADDRESS="))
                {
                    RemoteNodeWalletAddress = line.Replace("WALLET_ADDRESS=", "");
                }
                if (line.Contains("ENABLE_PUBLIC_MODE="))
                {
                    string option = line.Replace("ENABLE_PUBLIC_MODE=", "");
                    if (option.ToLower() == "y")
                    {
                        ClassRemoteNodeSync.WantToBePublicNode = true;
                    }
                }
                if (line.Contains("ENABLE_API_HTTP="))
                {
                    string option = line.Replace("ENABLE_API_HTTP=", "");
                    if (option.ToLower() == "y")
                    {
                        EnableApiHttp = true;
                    }
                }
                if (line.Contains("ENABLE_HTTPS_API_MODE="))
                {
                    string option = line.Replace("ENABLE_HTTPS_API_MODE=", "");
                    if (option.ToLower() == "y")
                    {
                        ClassApiHttp.UseSSL = true;
                    }
                }
                if (line.Contains("HTTPS_CERTIFICATE_PATH="))
                {
                    ClassApiHttp.SSLPath = line.Replace("HTTPS_CERTIFICATE_PATH=", "");
                }
                if (line.Contains("LOG_LEVEL="))
                {
                    int.TryParse(line.Replace("LOG_LEVEL=", ""), out LogLevel);
                }
            }
        }
    }
}