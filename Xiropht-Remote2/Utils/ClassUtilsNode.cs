using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;

namespace Xiropht_RemoteNode.Utils
{
    public class ClassUtilsNode
    {
        private static RNGCryptoServiceProvider Generator = new RNGCryptoServiceProvider();


        public static string ConvertPath(string path)
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix) path = path.Replace("\\", "/");
            return path;
        }

        /// <summary>
        ///     Get a random number in integer size.
        /// </summary>
        /// <param name="minimumValue"></param>
        /// <param name="maximumValue"></param>
        /// <returns></returns>
        public static int GetRandomBetween(int minimumValue, int maximumValue)
        {

            var randomNumber = new byte[1];

            Generator.GetBytes(randomNumber);

            var asciiValueOfRandomCharacter = Convert.ToDouble(randomNumber[0]);

            var multiplier = Math.Max(0, asciiValueOfRandomCharacter / 255d - 0.00000000001d);

            var range = maximumValue - minimumValue + 1;

            var randomValueInRange = Math.Floor(multiplier * range);

            return (int)(minimumValue + randomValueInRange);

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

        public static bool SocketIsConnected(TcpClient socket)
        {
            if (socket?.Client != null)
                try
                {
                    if (isClientConnected(socket))
                    {
                        return true;
                    }

                    return !(socket.Client.Poll(1, SelectMode.SelectWrite) && socket.Client.Poll(1, SelectMode.SelectRead) && socket.Available == 0);


                }
                catch
                {
                    return false;
                }

            return false;
        }

        public static bool isClientConnected(TcpClient ClientSocket)
        {
            try
            {
                var stateOfConnection = GetState(ClientSocket);


                if (stateOfConnection != TcpState.Closed && stateOfConnection != TcpState.CloseWait && stateOfConnection != TcpState.Closing)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }

        }

        public static TcpState GetState(TcpClient tcpClient)
        {
            var foo = IPGlobalProperties.GetIPGlobalProperties()
              .GetActiveTcpConnections()
              .SingleOrDefault(x => x.LocalEndPoint.Equals(tcpClient.Client.LocalEndPoint)
                                 && x.RemoteEndPoint.Equals(tcpClient.Client.RemoteEndPoint)
              );

            return foo != null ? foo.State : TcpState.Unknown;
        }


        public static string GetStringBetween(string STR, string FirstString, string LastString)
        {
            string FinalString;
            int Pos1 = STR.IndexOf(FirstString) + FirstString.Length;
            int Pos2 = STR.IndexOf(LastString);
            FinalString = STR.Substring(Pos1, Pos2 - Pos1);
            return FinalString;
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