using System;

namespace Xiropht_Remote2.Log
{
    public class ClassLog
    {
        public static void Log(string message, int logLevel, int colorId)
        {
            if (logLevel == Program.LogLevel)
            {
                switch (colorId)
                {
                    case 0:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                    case 1:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case 2:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case 3:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                }

                Console.WriteLine("<Xiropht>[Log] - " + DateTime.Now + " | " + message);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}