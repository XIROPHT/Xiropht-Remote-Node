using System;
using System.Collections.Generic;
using System.Text;
using Xiropht_RemoteNode.Log;

namespace Xiropht_RemoteNode.Filter
{
    public class ClassApiBanObject
    {
        public string Ip;
        public long BanDate;
        public int TotalInvalidPacket;
        public bool Banned;

        public ClassApiBanObject(string ipTmp)
        {
            Ip = ipTmp;
            BanDate = 0;
        }
    }

    public static class ClassApiBan
    {
        public const int MaxPacketPerSecond = 30;
        public const int MaxInvalidPacket = 10;
        public const int BanTimeInSecond = 60;
        public static Dictionary<string, ClassApiBanObject> ListBanApiIp = new Dictionary<string, ClassApiBanObject>();

        /// <summary>
        /// Increment total invalid packet.
        /// </summary>
        /// <param name="ip"></param>
        public static void InsertInvalidPacket(string ip)
        {
            ip = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(ip));
            ClassLog.Log("Insert Invalid Packet for IP: " + ip, 7, 3);
            if (ListBanApiIp.ContainsKey(ip)) // Anti flood.
            {
                ListBanApiIp[ip].TotalInvalidPacket++;
                if (ListBanApiIp[ip].TotalInvalidPacket >= MaxInvalidPacket)
                {
                    ListBanApiIp[ip].BanDate = DateTimeOffset.Now.ToUnixTimeSeconds() + BanTimeInSecond;
                    if (Program.EnableFilteringSystem)
                    {
                        if (!ListBanApiIp[ip].Banned)
                        {
                            ClassFilter.InsertFilterBan(ip);
                        }
                    }
                    ListBanApiIp[ip].Banned = true;
                }
            }
            else
            {
                ListBanApiIp.Add(ip, new ClassApiBanObject(ip));
                if (ListBanApiIp.ContainsKey(ip)) // Anti flood.
                {
                    ListBanApiIp[ip].TotalInvalidPacket++;
                    if (ListBanApiIp[ip].TotalInvalidPacket >= MaxInvalidPacket)
                    {
                        ListBanApiIp[ip].BanDate = DateTimeOffset.Now.ToUnixTimeSeconds() + BanTimeInSecond;
                        if (Program.EnableFilteringSystem)
                        {
                            if (!ListBanApiIp[ip].Banned)
                            {
                                ClassFilter.InsertFilterBan(ip);
                            }
                        }
                        ListBanApiIp[ip].Banned = true;
                    }
                }
            }
        }


        /// <summary>
        /// Check if ip is banned, return ban object id if the ip is not banned.
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool CheckBanIp(string ip)
        {
            ip = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(ip));

            if (ip == "127.0.0.1")
            {
                return false;
            }
            ClassLog.Log("Check Ban IP: " + ip + "", 7, 2);
            try
            {
                if (ListBanApiIp.ContainsKey(ip))
                {
                    if (ListBanApiIp[ip].BanDate > DateTimeOffset.Now.ToUnixTimeSeconds())
                    {
                        return true;
                    }
                    else
                    {
                        if (ListBanApiIp[ip].Banned)
                        {
                            ListBanApiIp[ip].Banned = false;
                            ListBanApiIp[ip].TotalInvalidPacket = 0;
                        }
                        return false;
                    }
                }
                else
                {
                    ClassLog.Log("Check Ban IP insert new object for IP: " + ip + "", 6, 2);
                    ListBanApiIp.Add(ip, new ClassApiBanObject(ip));
                }
            }
            catch
            {

            }
            return false;
        }
    }
}
