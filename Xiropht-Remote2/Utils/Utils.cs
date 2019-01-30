using System;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Xiropht_RemoteNode.Utils
{
    public class Utils
    {
        public static bool SocketIsConnected(TcpClient socket)
        {
            if (socket?.Client != null)
                try
                {
                    return !(socket.Client.Poll(100, SelectMode.SelectRead) && socket.Available == 0);
                }
                catch (SocketException)
                {
                    return false;
                }

            return false;
        }



        public static string ConvertStringToSha512(string str)
        {
            using (var hash = SHA512.Create())
            {
                var bytes = Encoding.UTF8.GetBytes(str);
                var hashedInputBytes = hash.ComputeHash(bytes);

                var hashedInputStringBuilder = new StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));

                str = hashedInputStringBuilder.ToString();
                hashedInputStringBuilder.Clear();
                GC.SuppressFinalize(bytes);
                GC.SuppressFinalize(hashedInputBytes);
                GC.SuppressFinalize(hashedInputStringBuilder);
                return str;
            }
        }


    }
}