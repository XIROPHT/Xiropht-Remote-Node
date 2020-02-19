using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Xiropht_RemoteNode.Log;

namespace Xiropht_RemoteNode.Api
{
    public class ClassApiBanObject
    {
        public string Ip;
        public int TotalInvalidPacket;
        public long LastInvalidPacketDate;
        public long LastBanDate;
        public bool Banned;

        public ClassApiBanObject(string ip)
        {
            Ip = ip;
            TotalInvalidPacket = 0;
            LastBanDate = 0;
        }
    }

    public class ClassFilterSystemEnumeration
    {
        public const string FilterSystemIptables = "iptables";
        public const string FilterSystemPacketFilter = "packetfilter";
    }

    public class ClassApiBan
    {

        public static Dictionary<string, ClassApiBanObject> ListFilterObjects = new Dictionary<string, ClassApiBanObject>();
        public const int MaxInvalidPacket = 100;
        public const int MaxPacketPerSecond = 10000;
        private const int KeepAliveInvalidPacket = 5; // Keep alive total of invalid packet accumulated.
        public const int BanDelay = 60; // Ban delay of 60 seconds.
        private const int AutoCheckObjectInterval = 1 * 1000; // Interval of auto check object.
        private static Thread ThreadFilterObject;
        public static string FilterSystem;
        public static string FilterChainName;


        /// <summary>
        /// Insert a new ip on the filtering system.
        /// </summary>
        /// <param name="ip"></param>
        public static void FilterInsertIp(string ip)
        {
            try
            {
                if (!ListFilterObjects.ContainsKey(ip))
                {
                    ListFilterObjects.Add(ip, new ClassApiBanObject(ip));
                }
                else // Just in case:
                {
                    if (ListFilterObjects[ip].Ip != ip) // Check the filter object ip.
                    {
                        ListFilterObjects[ip].Ip = ip;
                    }
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Return the filtering status of the ip connected.
        /// </summary>
        /// <param name="ip"></param>
        /// <returns>True for accepted, false for banned</returns>
        public static bool FilterCheckIp(string ip)
        {
            try
            {
                if (ListFilterObjects.ContainsKey(ip))
                {
                    if (ListFilterObjects[ip].Banned)
                    {
                        return false; // Banned
                    }
                }
            }
            catch
            {
                return true; // Not sure.
            }
            return true; // not banned.
        }

        /// <summary>
        /// Insert invalid packet 
        /// </summary>
        /// <param name="ip"></param>
        public static void FilterInsertInvalidPacket(string ip, int invalidPacket = 0)
        {
            try
            {
                if (ListFilterObjects.ContainsKey(ip) && ip != "127.0.0.1") // Increment the total invalid packet.
                {
                    if (invalidPacket == 0)
                    {
                        ListFilterObjects[ip].TotalInvalidPacket++;
                    }
                    else
                    {
                        ListFilterObjects[ip].TotalInvalidPacket = invalidPacket;
                    }
                    ListFilterObjects[ip].LastInvalidPacketDate = DateTimeOffset.Now.ToUnixTimeSeconds();
                    if (ListFilterObjects[ip].TotalInvalidPacket >= MaxInvalidPacket) // Ban if the filter object reach the max invalid packet.
                    {
                        ListFilterObjects[ip].LastBanDate = DateTimeOffset.Now.ToUnixTimeSeconds();
                        ListFilterObjects[ip].Banned = true;
                        if (Environment.OSVersion.Platform == PlatformID.Unix) // use iptables on linux.
                        {
                            switch (FilterSystem)
                            {
                                case ClassFilterSystemEnumeration.FilterSystemIptables:
                                    Process.Start("/bin/bash", "-c \"iptables -A " + FilterChainName + " -p tcp -s " + ip + " -j DROP\""); // Add iptables rule.
                                    break;
                                case ClassFilterSystemEnumeration.FilterSystemPacketFilter:
                                    Process.Start("pfctl", "-t " + FilterChainName + " -T add " + ip + ""); // Add packet filter rule.
                                    break;
                                default:
                                    ClassLog.Log("Cannot insert a rule of ban, please check your config and set the filter system name.", 7, 3);
                                    break;
                            }
                        }
                        else
                        {

                            try
                            {
                                using (Process cmd = new Process())
                                {
                                    cmd.StartInfo.FileName = "cmd.exe";
                                    cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                    cmd.StartInfo.Arguments = "/c netsh advfirewall firewall add rule name=\"AutoBAN (" + ip + ")\" protocol=TCP dir=in remoteip=" + ip + " action=block"; // Insert Windows rule.
                                    cmd.Start();
                                }
                            }
                            catch
                            {

                            }
                        }
                        ClassLog.Log("Incoming connection " + ip + " is banned.", 3, 3);
                    }
                }

            }
            catch
            {

            }
        }

        /// <summary>
        /// Automaticaly check each filter object.
        /// </summary>
        public static void FilterAutoCheckObject()
        {
            ThreadFilterObject = new Thread(delegate ()
            {
                while (!Program.Closed)
                {
                    try
                    {
                        if (ListFilterObjects.Count > 0)
                        {
                            foreach (KeyValuePair<string, ClassApiBanObject> filterObject in ListFilterObjects)
                            {
                                if (filterObject.Key != "127.0.0.1")
                                {
                                    if (!filterObject.Value.Banned) // If not banned
                                    {
                                        if (filterObject.Value.LastInvalidPacketDate + KeepAliveInvalidPacket < DateTimeOffset.Now.ToUnixTimeSeconds()) // Clean accumulated total invalid packet.
                                        {
                                            ListFilterObjects[filterObject.Key].LastInvalidPacketDate = 0;
                                            ListFilterObjects[filterObject.Key].TotalInvalidPacket = 0;
                                        }
                                        else // Just in case:
                                        {
                                            if (filterObject.Value.TotalInvalidPacket >= MaxInvalidPacket) // Ban if the filter object reach the max invalid packet.
                                            {
                                                ListFilterObjects[filterObject.Key].LastBanDate = DateTimeOffset.Now.ToUnixTimeSeconds();
                                                ListFilterObjects[filterObject.Key].Banned = true;
                                            }
                                        }
                                    }
                                    else
                                    {

                                        if (filterObject.Value.LastBanDate + BanDelay < DateTimeOffset.Now.ToUnixTimeSeconds()) // Unban.
                                        {

                                            ListFilterObjects[filterObject.Key].LastInvalidPacketDate = 0;
                                            ListFilterObjects[filterObject.Key].TotalInvalidPacket = 0;
                                            ListFilterObjects[filterObject.Key].LastBanDate = 0;
                                            ListFilterObjects[filterObject.Key].Banned = false;
                                            if (Environment.OSVersion.Platform == PlatformID.Unix) // use iptables on linux.
                                            {
                                                switch (FilterSystem)
                                                {
                                                    case ClassFilterSystemEnumeration.FilterSystemIptables:
                                                        Process.Start("/bin/bash", "-c \"iptables -D " + FilterChainName + " -p tcp -s " + filterObject.Key + " -j DROP\""); // Remove iptables rule.
                                                        break;
                                                    case ClassFilterSystemEnumeration.FilterSystemPacketFilter:
                                                        Process.Start("pfctl", "-t " + FilterChainName + " -T del " + filterObject.Key + ""); // Remove PF rule.
                                                        break;
                                                    default:
                                                        ClassLog.Log("Cannot remove a rule of ban, please check your config and set the filter system name.", 7, 3);
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                try
                                                {
                                                    using (Process cmd = new Process())
                                                    {
                                                        cmd.StartInfo.FileName = "cmd.exe";
                                                        cmd.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                                        cmd.StartInfo.Arguments = "/c netsh advfirewall firewall delete rule name=\"AutoBAN (" + filterObject.Value.Ip + ")\""; // Remove Windows rule.
                                                        cmd.Start();
                                                    }
                                                }
                                                catch (Exception error)
                                                {
                                                    ClassLog.Log("Windows - Unban ip " + filterObject.Value.Ip + " exception: " + error.Message, 0, 2);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {

                    }
                    Thread.Sleep(AutoCheckObjectInterval);
                }
            });
            ThreadFilterObject.Start();
        }

    }
}
