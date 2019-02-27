using System;
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
using Xiropht_RemoteNode.Filter;
using Xiropht_RemoteNode.Log;
using Xiropht_RemoteNode.RemoteNode;
using Xiropht_RemoteNode.Utils;

namespace Xiropht_RemoteNode
{
    public class Program
    {
        /// <summary>
        /// Remote node object of sync.
        /// </summary>
        public static ClassRemoteNodeObject RemoteNodeObjectCoinMaxSupply; // Sync the node for get coin max supply information.
        public static ClassRemoteNodeObject RemoteNodeObjectCoinCirculating; // Sync the node for get coin circulating information.
        public static ClassRemoteNodeObject RemoteNodeObjectTotalBlockMined; // Sync the node for get total block mined information.
        public static ClassRemoteNodeObject RemoteNodeObjectTotalPendingTransaction; // Sync the node for get total pending transaction information.
        public static ClassRemoteNodeObject RemoteNodeObjectCurrentDifficulty; // Sync the node for get current mining difficulty information.
        public static ClassRemoteNodeObject RemoteNodeObjectCurrentRate; // Sync the node for get current mining hashrate information.
        public static ClassRemoteNodeObject RemoteNodeObjectToBePublic; // Sync the node for get the public node list information and ask to be public if not.
        public static ClassRemoteNodeObject RemoteNodeObjectTransaction; // Sync the node for get each transaction data information.
        public static ClassRemoteNodeObject RemoteNodeObjectTotalFee; // Sync the node for get current amount of fee information.
        public static ClassRemoteNodeObject RemoteNodeObjectBlock; // Sync the node for get each block data information.
        public static ClassRemoteNodeObject RemoteNodeObjectTotalTransaction; // Sync the node for get total number of transaction information.

        /// <summary>
        /// Current wallet address used by the remote node.
        /// </summary>
        public static string RemoteNodeWalletAddress; 

        /// <summary>
        /// Threading.
        /// </summary>
        private static Thread _threadCommandLine;

        /// <summary>
        /// About log settings.
        /// </summary>
        public static int LogLevel;
        public static bool EnableWriteLog;

        /// <summary>
        /// Certificate generated for communicate with the network.
        /// </summary>
        public static string Certificate;

        /// <summary>
        /// Force to convert every users to the same cultureinfo.
        /// </summary>
        public static CultureInfo GlobalCultureInfo = new CultureInfo("fr-FR");

        /// <summary>
        /// Return the program status.
        /// </summary>
        public static bool Closed;

        /// <summary>
        /// About setting file.
        /// </summary>
        private static string ConfigFilePath = "\\config.ini";

        /// <summary>
        /// About api http setting.
        /// </summary>
        private static bool EnableApiHttp;

        /// <summary>
        /// About filtering system.
        /// </summary>
        public static bool EnableFilteringSystem;

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
                if (EnableWriteLog)
                {
                    ClassLog.EnableWriteLog();
                }
                if (EnableFilteringSystem)
                {
                    ClassFilter.EnableFilterSystem();
                }
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
                    Console.WriteLine("Do you want to select another port for your HTTP API? [Y/N]");
                    answer = Console.ReadLine();
                    if (answer == "Y" || answer == "y")
                    {
                        Console.WriteLine("Enter your port selected for your HTTP API: (By default: " + ClassConnectorSetting.RemoteNodeHttpPort + ")");
                        string portChoosed = Console.ReadLine();
                        while (!int.TryParse(portChoosed, out ClassApiHttp.PersonalRemoteNodeHttpPort))
                        {
                            Console.WriteLine("Invalid port, please try another one:");
                            portChoosed = Console.ReadLine();
                        }
                    }
                    Console.WriteLine("Do you want to enable HTTPS mode ? [Y/N]");
                    answer = Console.ReadLine();
                    if (answer == "Y" || answer == "y")
                    {
                        ClassApiHttp.UseSSL = true;
                        Console.WriteLine("Write your ssl certificate path, including the file name [Only PFX certificate file is compatible]: ");
                        ClassApiHttp.SSLPath = Console.ReadLine();
                    }
                }
                SaveConfigFile();

            }



            Certificate = ClassUtils.GenerateCertificate();
            Console.WriteLine("Initialize Remote Node Sync Objects..");

            RemoteNodeObjectToBePublic = new ClassRemoteNodeObject(SyncEnumerationObject.ObjectToBePublic);
            RemoteNodeObjectCoinMaxSupply = new ClassRemoteNodeObject(SyncEnumerationObject.ObjectCoinSupply);
            RemoteNodeObjectTransaction = new ClassRemoteNodeObject(SyncEnumerationObject.ObjectTransaction);
            RemoteNodeObjectCoinMaxSupply = new ClassRemoteNodeObject(SyncEnumerationObject.ObjectCoinSupply);
            RemoteNodeObjectCoinCirculating = new ClassRemoteNodeObject(SyncEnumerationObject.ObjectCoinCirculating);
            RemoteNodeObjectTotalBlockMined = new ClassRemoteNodeObject(SyncEnumerationObject.ObjectBlockMined);
            RemoteNodeObjectTotalPendingTransaction = new ClassRemoteNodeObject(SyncEnumerationObject.ObjectPendingTransaction);
            RemoteNodeObjectCurrentDifficulty = new ClassRemoteNodeObject(SyncEnumerationObject.ObjectCurrentDifficulty);
            RemoteNodeObjectCurrentRate = new ClassRemoteNodeObject(SyncEnumerationObject.ObjectCurrentRate);
            RemoteNodeObjectTotalFee = new ClassRemoteNodeObject(SyncEnumerationObject.ObjectTotalFee);
            RemoteNodeObjectTotalTransaction = new ClassRemoteNodeObject(SyncEnumerationObject.ObjectTotalTransaction);
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

                     if (await RemoteNodeObjectCoinMaxSupply.StartConnectionAsync())
                     {
                         await Task.Delay(10);
                     }
                     if (await RemoteNodeObjectCoinCirculating.StartConnectionAsync())
                     {
                         await Task.Delay(10);
                     }
                     if (await RemoteNodeObjectTotalBlockMined.StartConnectionAsync())
                     {
                         await Task.Delay(10);
                     }
                     if (await RemoteNodeObjectTotalPendingTransaction.StartConnectionAsync())
                     {
                         await Task.Delay(10);
                     }
                     if (await RemoteNodeObjectCurrentDifficulty.StartConnectionAsync())
                     {
                         await Task.Delay(10);
                     }
                     if (await RemoteNodeObjectCurrentRate.StartConnectionAsync())
                     {
                         await Task.Delay(10);
                     }

                     if (await RemoteNodeObjectTotalFee.StartConnectionAsync())
                     {
                         await Task.Delay(10);
                     }
                     if (await RemoteNodeObjectTotalTransaction.StartConnectionAsync())
                     {
                         await Task.Delay(10);
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
                     RemoteNodeObjectCoinCirculating.StopConnection();
                     RemoteNodeObjectCoinMaxSupply.StopConnection();
                     RemoteNodeObjectCurrentDifficulty.StopConnection();
                     RemoteNodeObjectCurrentRate.StopConnection();
                     RemoteNodeObjectTotalBlockMined.StopConnection();
                     RemoteNodeObjectTotalFee.StopConnection();
                     RemoteNodeObjectTotalPendingTransaction.StopConnection();
                     RemoteNodeObjectTotalTransaction.StopConnection();
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
                writer.WriteLine("API_HTTP_PORT=" + ClassApiHttp.PersonalRemoteNodeHttpPort);
                writer.WriteLine("LOG_LEVEL=" + LogLevel);
                writer.WriteLine("WRITE_LOG=Y");
                writer.WriteLine("ENABLE_FILTERING_SYSTEM=N");
                writer.WriteLine("CHAIN_FILTERING_SYSTEM=");
                writer.WriteLine("NAME_FILTERING_SYSTEM=");
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
                    else
                    {
                        ClassRemoteNodeSync.WantToBePublicNode = false;
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
                if (line.Contains("API_HTTP_PORT="))
                {
                    int.TryParse(line.Replace("API_HTTP_PORT=", ""), out ClassApiHttp.PersonalRemoteNodeHttpPort);
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
                if (line.Contains("WRITE_LOG="))
                {
                    string option = line.Replace("WRITE_LOG=", "");
                    if (option.ToLower() == "y")
                    {
                        EnableWriteLog = true;
                    }
                }
                if (line.Contains("ENABLE_FILTERING_SYSTEM="))
                {
                    string option = line.Replace("ENABLE_FILTERING_SYSTEM=", "");
                    if (option.ToLower() == "y")
                    {
                        EnableFilteringSystem = true;
                    }
                }
                if (line.Contains("CHAIN_FILTERING_SYSTEM="))
                {
                    ClassFilter.FilterChainName = line.Replace("CHAIN_FILTERING_SYSTEM=", "").ToLower();
                }
                if (line.Contains("NAME_FILTERING_SYSTEM="))
                {
                    ClassFilter.FilterSystem = line.Replace("NAME_FILTERING_SYSTEM=", "").ToLower();
                }

            }
        }
    }
}