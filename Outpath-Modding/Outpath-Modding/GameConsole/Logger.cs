using System.IO;
using UnityEngine;
using Outpath_Modding.Loader;
using System.Collections.Generic;

namespace Outpath_Modding.GameConsole
{
    public static class Logger
    {
        public static List<TempLog> LogText = new List<TempLog>();

        public static void Message(string text)
        {
            StreamReader sr = new(Path.Combine(Paths.Log, "LogMessage.txt"));
            string line = sr.ReadLine();
            sr.Close();
            StreamWriter sw = new(Path.Combine(Paths.Log, "LogMessage.txt"));

            sw.WriteLine(line + "\n" + text);
            sw.Close();

            LogText.Add(new(text, 0));

            if (ConsoleManager.Console != null) ConsoleManager.Console.SendLog(text);
        }

        public static void Message(string text, Color color)
        {
            StreamReader sr = new(Path.Combine(Paths.Log, "LogMessage.txt"));
            string line = sr.ReadLine();
            sr.Close();
            StreamWriter sw = new(Path.Combine(Paths.Log, "LogMessage.txt"));

            sw.WriteLine(line + "\n" + text);
            sw.Close();

            LogText.Add(new(text, 1, color));

            if (ConsoleManager.Console != null) ConsoleManager.Console.SendLog(text, color);
        }

        public static void Debug(string text)
        {
            StreamReader sr = new(Path.Combine(Paths.Log, "LogDebug.txt"));
            string line = sr.ReadLine();
            sr.Close();
            StreamWriter sw = new(Path.Combine(Paths.Log, "LogDebug.txt"));

            sw.WriteLine(line + "\n" + text);
            sw.Close();

            LogText.Add(new(text, 2));

            if (ConsoleManager.Console != null) ConsoleManager.Console.SendDebug(text);
        }

        public static void Info(string text)
        {
            StreamReader sr = new(Path.Combine(Paths.Log, "LogInfo.txt"));
            string line = sr.ReadLine();
            sr.Close();
            StreamWriter sw = new(Path.Combine(Paths.Log, "LogInfo.txt"));

            sw.WriteLine(line + "\n" + text);
            sw.Close();

            LogText.Add(new(text, 3));

            if (ConsoleManager.Console != null) ConsoleManager.Console.SendInfo(text);
        }

        public static void Warn(string text)
        {
            StreamReader sr = new(Path.Combine(Paths.Log, "LogWarn.txt"));
            string line = sr.ReadLine();
            sr.Close();
            StreamWriter sw = new(Path.Combine(Paths.Log, "LogWarn.txt"));

            sw.WriteLine(line + "\n" + text);
            sw.Close();

            LogText.Add(new(text, 4));

            if (ConsoleManager.Console != null) ConsoleManager.Console.SendWarn(text);
        }

        public static void Error(string text)
        {
            StreamReader sr = new(Path.Combine(Paths.Log, "LogError.txt"));
            string line = sr.ReadLine();
            sr.Close();
            StreamWriter sw = new(Path.Combine(Paths.Log, "LogError.txt"));

            sw.WriteLine(line + "\n" + text);
            sw.Close();

            LogText.Add(new(text, 5));

            if (ConsoleManager.Console != null) ConsoleManager.Console.SendError(text);
        }

        [System.Serializable]
        public class TempLog
        {
            public string Context;

            public Color Color = Color.clear;

            public int Type;

            public TempLog(string context, int type)
            {
                Context = context;
                Type = type;
            }

            public TempLog(string context, int type, Color color)
            {
                Context = context;
                Type = type;
                Color = color;
            }
        }
    }
}
