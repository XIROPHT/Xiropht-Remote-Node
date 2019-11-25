using System;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Xiropht_RemoteNode.Utils
{
    public class ClassUtilsNode
    {

        public static string ConvertPath(string path)
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix) path = path.Replace("\\", "/");
            return path;
        }

        /// <summary>
        /// Remove special characters
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RemoveSpecialCharacters(string str)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in str)
            {
                if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z') || c == '.' || c == '_')
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Check the status of the socket.
        /// </summary>
        /// <param name="socket"></param>
        /// <returns></returns>
        public static bool SocketIsConnected(TcpClient socket)
        {
            if (socket?.Client != null)
                try
                {

                    return !(socket.Client.Poll(100, SelectMode.SelectRead) && socket.Available == 0);


                }
                catch
                {
                    return false;
                }

            return false;
        }



        /// <summary>
        /// Get a string between two string delimiters.
        /// </summary>
        /// <param name="str"></param>
        /// <param name="firstString"></param>
        /// <param name="lastString"></param>
        /// <returns></returns>
        public static string GetStringBetween(string str, string firstString, string lastString)
        {
            string FinalString;
            int Pos1 = str.IndexOf(firstString) + firstString.Length;
            int Pos2 = str.IndexOf(lastString);
            FinalString = str.Substring(Pos1, Pos2 - Pos1);
            return FinalString;
        }

        /// <summary>
        /// Convert a string into SHA256
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Clear memory stored on the Garbage Collector.
        /// </summary>
        public static void ClearGc()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }
    }
}