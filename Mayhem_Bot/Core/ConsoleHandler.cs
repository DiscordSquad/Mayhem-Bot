using Mayhem_Bot.classes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mayhem_Bot.Core
{
    public class ConsoleHandler
    {  

        public class Analysis
        {
   
            public void getCookies(ulong UserId)
            {

            }
        }
        public static void EnterConsoleMode()
        {
            CoreProgram._errorHandler.DebugMode = false;
            EnterConsoleCommand();
        }
        public static void EnterConsoleCommand()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("Command: ");
        }
        public static void ExecuteCommand(string Command)
        {
           if(Command == " ")
            {
                CoreProgram._errorHandler.DebugMode = false;
            }
           if(Command == "y")
            {
                Console.WriteLine("Exiting Console Mode");
                Console.ForegroundColor = ConsoleColor.White;
                CoreProgram._errorHandler.ResumeDebug();

                
            }

        }
    }
}
