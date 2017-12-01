using Discord.WebSocket;
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
            return Task.CompletedTask;
        }
        public Task LeftGuild(SocketGuild arg)
        {
            //deletes the server settings when this bot leaves a server
            ServerSettings.DeleteServerSettingList(arg.Id);
            return Task.CompletedTask;
        }
    }
}
