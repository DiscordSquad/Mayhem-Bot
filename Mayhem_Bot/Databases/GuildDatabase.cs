using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Mayhem_Bot.Databases
{
    public static class GuildDatabase
    {
        public static ConcurrentDictionary<string, classes.GuildUser_Database> GuildSettingsList = new ConcurrentDictionary<string,classes.GuildUser_Database>();
        public enum Counters
        {
            Messages,
            Images,
            Links,
            Commands,
            YoutubeLinks,
        }
        public static void GivePlayerCookie(SocketUser user, ulong GuildId)
        {
            classes.GuildUser_Database gudb = getUserFromGuild(GuildId, user);
            if (gudb != null)
            {
                if (gudb.CanReceiveCookie())
                {
                    gudb.GivePlayerCookie();
                    Core.DatabaseHandler.GuildDatabase_ChangesMade = true;
                }
            }
        }
        public static void IncreaseCount(SocketUser user, ulong GuildId, Counters counter, string command = "")
        {
            classes.GuildUser_Database gudb = getUserFromGuild(GuildId, user);
            if (gudb != null)
            {
                switch (counter)
                {
                    case Counters.Messages:
                        gudb.IncreaseMessage();
                        break;
                    case Counters.Images:
                        gudb.IncreaseImages();
                        break;
                    case Counters.Links:
                        gudb.IncreaseLinks();
                        break;
                    case Counters.Commands:
                        gudb.IncreaseCommands(command);
                        break;
                    case Counters.YoutubeLinks:
                        gudb.IncreaseYoutubeLinks();
                        break;
                }
            }
            Core.DatabaseHandler.GuildDatabase_ChangesMade = true;
        }
        private static classes.GuildUser_Database ImportUserToDB(ulong GuildID, SocketUser user)
        {
            classes.GuildUser_Database newUser = new classes.GuildUser_Database(user, GuildID);
            GuildSettingsList.TryAdd($"{GuildID}|{user.Id}", newUser);
            return newUser;
        }
        private static classes.GuildUser_Database getUserFromGuild(ulong GuildId, SocketUser User)
        {
           if(!GuildSettingsList.TryGetValue($"{GuildId}|{User.Id}", out classes.GuildUser_Database gdb))
            {
                return ImportUserToDB(GuildId, User);
            }
            else
            {
                return gdb;
            }
        }
    }
}
