using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Mayhem_Bot.Databases;
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
            //Sets the default ConsoleColor for the Console
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
                    c = MESSAGE_COLOR;
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
            return Task.CompletedTask;
        }
        /// <summary>
        /// Directs the CommandError to the ErrorHandler and send a message if the settings are set to true
        /// </summary>
        /// <param name="Context">SocketCommandContext</param>
        /// <param name="result">Result from the SocketCommandContext</param>
        /// <param name="Source">The method which has been balled</param>
        /// <returns></returns>
        public async Task HandleCommandError(SocketCommandContext Context, IResult result, string Source)
        {
            string message = "";
            //Whitelist case - Just filter the error we want to track
            switch (result.Error.Value.ToString())
            {
                case "UnknownCommand":
                    message = $"I am not able to recognize this command, {Context.User.Mention}.";
                    break;
            }
            //Is definitely a private channel, no settings needed
            if (Context.IsPrivate)
            {
                //Create a new channel
                var channel = await Context.User.GetOrCreateDMChannelAsync() as IDMChannel;
                //Send message to user
                await channel.SendMessageAsync(message, false);
                //write to log
                await _client_Log(new LogMessage(LogSeverity.Error, $"{Source}", $"[{Context.Channel.Name}|{Context.User.Username}] - ['{Context.Message.Content}'] Exception: {result.Error}"));
                return;
            }
            else //message is guild channel message - go ahead
            {
                //Check if we are allowd to send public channel response messages
                if (ServerSettings.GetSettingsValue(ServerSettings.Settings.SendErrorMessage, Context.Guild.Id)){await MessageHandler.SendEmbededMessageAsync(MessageHandler.MessageType.Error, Context.Channel, result.Error.Value.ToString(), message); }
                else if (ServerSettings.GetSettingsValue(ServerSettings.Settings.SendPrivateMessage, Context.Guild.Id))  //Check if we are allowed to send private response messages
                {
                    var channel = await Context.User.GetOrCreateDMChannelAsync();
                    await MessageHandler.SendEmbededDMMessageAsync(MessageHandler.MessageType.Error, channel, result.Error.Value.ToString(), message);
                }
                //Write to log
                await _client_Log(new LogMessage(LogSeverity.Error, $"{Source}", $"[{Context.Guild.Name}|{Context.Channel.Name}|{Context.User.Username}] - ['{Context.Message.Content}'] Exception: {result.Error}"));
            }
          
          
        }
    }
}
