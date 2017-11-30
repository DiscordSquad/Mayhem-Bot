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
        public Task _client_Log(LogMessage arg)
        {
            return default(Task);
        }
        public async Task SendErrorToChannel(SocketCommandContext Context)
        {
            
        }
    }
}
