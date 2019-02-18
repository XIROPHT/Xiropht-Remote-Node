using System;
using System.Diagnostics;
using System.Threading;
using Xiropht_RemoteNode.Api;
using Xiropht_RemoteNode.Log;

namespace Xiropht_RemoteNode.Filter
{
    public class ClassFilterSystemEnumeration
    {
        public const string FilterSystemIptables = "iptables";
        public const string FilterSystemPacketFilter = "packetfilter";
    }

    public class ClassFilter
    {
        public static string FilterSystem;
        public static string FilterChainName;
        private static Thread ThreadCheckFilterSystem;
        private const int CheckFilterSystemInterval = 1 * 1000;
     
        /// <summary>
        /// Insert rule of ban.
        /// </summary>
        /// <param name="ip"></param>
        public static void InsertFilterBan(string ip)
        {
            if (string.IsNullOrEmpty(FilterChainName))
            {
                ClassLog.Log("Cannot insert a rule of ban, please check your config and insert a chain name.", 7, 3);
            }
            else
            {
                switch(FilterSystem)
                {
                    case ClassFilterSystemEnumeration.FilterSystemIptables:
                        Process.Start("/bin/bash", "-c \"iptables -A "+FilterChainName+" -p tcp -s " + ip + " -j DROP\""); // Add iptables rules.
                        break;
                    case ClassFilterSystemEnumeration.FilterSystemPacketFilter:
                        Process.Start("pfctl", "-t " + FilterChainName + " -T add " + ip + ""); // Add iptables rules.
                        break;
                    default:
                        ClassLog.Log("Cannot insert a rule of ban, please check your config and set the filter system name.", 7, 3);
                        break;
                }
            }
        }

        /// <summary>
        /// Remove rule of ban.
        /// </summary>
        /// <param name="ip"></param>
        public static void RemoteFilterBan(string ip)
        {
            if (string.IsNullOrEmpty(FilterChainName))
            {
                ClassLog.Log("Cannot remove a rule of ban, please check your config and insert a chain name.", 7, 3);
            }
            else
            {
                switch (FilterSystem)
                {
                    case ClassFilterSystemEnumeration.FilterSystemIptables:
                        Process.Start("/bin/bash", "-c \"iptables -D " + FilterChainName + " -p tcp -s " + ip + " -j DROP\""); // Add iptables rules.
                        break;
                    case ClassFilterSystemEnumeration.FilterSystemPacketFilter:
                        Process.Start("pfctl", "-t " + FilterChainName + " -T del " + ip + ""); // Add iptables rules.
                        break;
                    default:
                        ClassLog.Log("Cannot remove a rule of ban, please check your config and set the filter system name.", 7, 3);
                        break;
                }
            }
        }

        /// <summary>
        /// Check automaticaly list of banned ip.
        /// </summary>
        public static void EnableFilterSystem()
        {
            ThreadCheckFilterSystem = new Thread(delegate ()
            {
                while(!Program.Closed)
                {
                    foreach(var banObject in ClassApiBan.ListBanApiIp)
                    {
                        if (banObject.Value.Banned)
                        {
                            if (banObject.Value.BanDate < DateTimeOffset.Now.ToUnixTimeSeconds())
                            {
                                ClassLog.Log("Unban ip: " + banObject.Key + "", 7, 1);
                                banObject.Value.Banned = false;
                                banObject.Value.TotalInvalidPacket = 0;
                                RemoteFilterBan(banObject.Key);
                            }
                        }
                    }
                    Thread.Sleep(CheckFilterSystemInterval);
                }
            });
            ThreadCheckFilterSystem.Start();
        }
    }
}
