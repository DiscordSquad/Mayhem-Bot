using Discord.WebSocket;
using Mayhem_Bot.Databases;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mayhem_Bot.Core
{
    public class ListenerHandler
    {
        public Task JoinedGuild(SocketGuild arg)
        {
            //Creates new server settings when this bot joins a server
            ServerSettings.CreateNewServerSettingList(arg.Id);
            CoreProgram._errorHandler._client_Log(new Discord.LogMessage(Discord.LogSeverity.Info, "JoinedGuild", $"Bot has joined the Guild '{arg.Name}'"));
            return Task.CompletedTask;
        }
        public Task LeftGuild(SocketGuild arg)
        {
            //deletes the server settings when this bot leaves a server
            ServerSettings.DeleteServerSettingList(arg.Id);
            CoreProgram._errorHandler._client_Log(new Discord.LogMessage(Discord.LogSeverity.Info, "LeftGuild", $"Bot has left the Guild '{arg.Name}'"));
            return Task.CompletedTask;
        }
        public Task UserJoined(SocketGuildUser arg)
        {
            //Import the joined user to the GuildUserDatabase
            GuildUserDatabase.ImportUserToDB(arg.Guild.Id, arg);
            CoreProgram._errorHandler._client_Log(new Discord.LogMessage(Discord.LogSeverity.Info, "UserJoined", $"The User '{arg.Nickname}' has joined the Guild '{arg.Guild.Name}'"));
            return Task.CompletedTask;
        }
        public Task UserLeft(SocketGuildUser arg)
        {
            //Delete the joined user to the GuildUserDatabase
            GuildUserDatabase.DelteUserFromDB(arg.Guild.Id, arg);
            CoreProgram._errorHandler._client_Log(new Discord.LogMessage(Discord.LogSeverity.Info, "UserLeft", $"The User '{arg.Nickname}' has left the Guild '{arg.Guild.Name}'"));
            return Task.CompletedTask;
        }
    }
}
