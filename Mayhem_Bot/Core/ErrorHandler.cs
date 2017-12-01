using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mayhem_Bot.Core
{
    public class ErrorHandler
    {
        const ConsoleColor CRITICAL_COLOR = ConsoleColor.Red;
        const ConsoleColor ERROR_COLOR = ConsoleColor.DarkRed;
        const ConsoleColor WARNING_COLOR = ConsoleColor.Yellow;
        const ConsoleColor MESSAGE_COLOR = ConsoleColor.Green;
        const ConsoleColor DEBUG_COLOR = ConsoleColor.Blue;
        const ConsoleColor INFO_COLOR = ConsoleColor.Cyan;

        public Task _client_Log(LogMessage arg)
        {
            ConsoleColor c = ConsoleColor.White;
            string message = $"[LOGHANDLER][{DateTime.Now.ToString()}] || [Severity: {arg.Severity.ToString()}] ";
            switch (arg.Severity)
            {
                case LogSeverity.Critical:
                    c = CRITICAL_COLOR;
                    message += $"Exception: {arg.Exception} - Message: {arg.Message} - Source: {arg.Source}";
                    break;
                case LogSeverity.Error:
                    c = ERROR_COLOR;
                    if (arg.Exception == null){ message += $"Message: {arg.Message} - Source: {arg.Source}";}
                    else {message += $"Exception: {arg.Exception} - Message: {arg.Message} - Source: {arg.Source}";}
                    break;
                case LogSeverity.Warning:
                    c = WARNING_COLOR;
                    message += $"Message: {arg.Message} - Source: {arg.Source}";
                    break;
                case LogSeverity.Info:
                    c = INFO_COLOR;
                    message += $"Message: {arg.Message} - Source: {arg.Source}";
                    break;
                case LogSeverity.Verbose:
                    c = ConsoleColor.White;
                    message += $"{arg.Message} - Source: {arg.Source}";
                    break;
                case LogSeverity.Debug:
                    c = DEBUG_COLOR;
                    message += $"Message: {arg.Message} - Source: {arg.Source}";
                    break;
                default:
                    break;
            }
            Console.ForegroundColor = c;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
            return default(Task);
        }

        public async Task HandleCommandError(SocketCommandContext Context, IResult result, string Source)
        {
            //Needs some sort of refactor cause of some nullreferences ?!?!?
            //await debugging is like a piece of shit

            string message = "";
            switch (result.Error.Value.ToString())
            {
                case "UnknownCommand":
                    message = "This is an unknown Command";
                    break;
            }
            if (ServerSettings.GetSettingsValue(ServerSettings.Settings.SendErrorMessage, Context.Guild.Id))
            {
                await Context.Channel.SendMessageAsync(message, false);
            }
            await _client_Log(new LogMessage(LogSeverity.Error, $"{Source}", $"[{Context.Guild.Name}|{Context.Channel.Name}|{Context.User.Username}] - ['{Context.Message.Content}'] Exception: {result.Error}"));
        }
    }
}
