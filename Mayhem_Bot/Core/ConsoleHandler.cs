using Mayhem_Bot.classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.RegularExpressions;
using System.Reflection;

namespace Mayhem_Bot.Core
{
    public class ConsoleHandler
    {  
        public class ConsoleCommands
        {
            public static void Execute(string[] args)
            {
                Type t = typeof(ConsoleCommands);
                MethodInfo info = t.GetMethod(args.First().ToLower());
                if (info == null)
                    Help();
                info.Invoke(null ,null);
            }
            public static void Help()
            {
                
            }
            public static void EnterConsoleMode()
            {
                CoreProgram._errorHandler.DebugMode = false;
                EnterConsoleCommand();
            }
            public static void exit()
            {
                Console.WriteLine("Exiting Console Mode");
                Console.ForegroundColor = ConsoleColor.White;
                CoreProgram._errorHandler.ResumeDebug();
            }
        }
        public static class Analysis
        {
            public static void Help()
            {
                
            }
            public static void getLastCookies(ulong GuildId, ulong UserId)
            {

            }
            public static void getCounter(ulong GuildId, ulong UserId, Databases.GuildUserDatabase.Counters counter)
            {
                switch (counter)
                {
                    case Databases.GuildUserDatabase.Counters.Commands:
                        break;
                    case Databases.GuildUserDatabase.Counters.Images:
                        break;
                    case Databases.GuildUserDatabase.Counters.Links:
                        break;
                    case Databases.GuildUserDatabase.Counters.Messages:
                        break;
                    case Databases.GuildUserDatabase.Counters.YoutubeLinks:
                        break;
                }
            }
            public static void getLastCommands(ulong GuildId, ulong UserId)
            {
                
            }
        }
        public static void EnterConsoleCommand()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("Command: ");
        }
        public static void ExecuteCommand(string Command)
        {
            string[] args = ParseCommand(Command); 
            switch (Command.Split(' ').First())
            {
                case "analysis":
                    break;
                case "console":
                    ConsoleCommands.Execute(args);
                    break;
            }
        }
        private static string[] ParseCommand(string Command)
        {
            string command = Command.Split(' ').First();
            string[] args = Command.Split(' ').Skip(1).Where(x => x.First() == '-').ToArray();
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = Regex.Replace(args[i], "-", "");
            }
            return args;
        }
    }
}
