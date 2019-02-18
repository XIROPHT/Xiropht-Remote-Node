using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Xiropht_RemoteNode.Utils;

namespace Xiropht_RemoteNode.Log
{
    public class ClassLog
    {
        private static StreamWriter writerLog = new StreamWriter(ClassUtilsNode.ConvertPath(Directory.GetCurrentDirectory() + "\\remotenode.log")) { AutoFlush = true };
        private static List<string> ListOfLog = new List<string>();
        private static Thread ThreadWriteLog;
        private const int WriteLogInterval = 10 * 1000; // Every 10 seconds in milliseconds.
        private const int MinimumLogLine = 100;

        /// <summary>
        /// Show and write logs.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logLevel"></param>
        /// <param name="colorId"></param>
        public static void Log(string message, int logLevel, int colorId)
        {
            message = "<Xiropht>[Log] - " + DateTime.Now + " | " + message;
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

                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.White;
            }
            if (Program.EnableWriteLog)
            {
                InsertLog(message);
            }
        }

        /// <summary>
        /// Insert log to the list.
        /// </summary>
        /// <param name="message"></param>
        private static void InsertLog(string message)
        {
            try
            {
                ListOfLog.Add(message);
            }
            catch
            {

            }
        }

        /// <summary>
        /// Write logs once the list reach the minimum of content required.
        /// </summary>
        public static void EnableWriteLog()
        {
            ThreadWriteLog = new Thread(delegate ()
            {
                while (!Program.Closed)
                {
                    try
                    {
                        if (ListOfLog.Count >= MinimumLogLine)
                        {
                            for (int i = 0; i < ListOfLog.Count; i++)
                            {
                                if (i < ListOfLog.Count)
                                {
                                    if (ListOfLog[i] != null)
                                    {
                                        writerLog.WriteLine("WriteLog Date: "+ DateTime.Now + " - " + ListOfLog[i]);
                                        ListOfLog[i] = null;
                                    }
                                }
                            }
                            ListOfLog.Clear();
                        }
                    }
                    catch
                    {

                    }
                    Thread.Sleep(WriteLogInterval);
                }
            })
            {
                Priority = ThreadPriority.Lowest
            };
            ThreadWriteLog.Start();
        }
    }
}