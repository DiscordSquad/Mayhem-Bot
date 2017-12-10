using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Text.RegularExpressions;

namespace Mayhem_Bot.Databases
{
    public static class GuildUserDatabase
    {
        //first string is the primary identifier for a user in a specific guild 'GuildId|UserId'
        public static ConcurrentDictionary<string, classes.GuildUser_Database> GuildSettingsList = new ConcurrentDictionary<string,classes.GuildUser_Database>();
        /// <summary>
        /// Counter types from a user
        /// </summary>
        public enum Counters
        {
            Messages,
            Images,
            Links,
            Commands,
            YoutubeLinks,
        }
        /// <summary>
        /// Gives the player a cookie.
        /// <para>A cookie can be sent all 5 minutes</para>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="GuildId"></param>
        public static void GivePlayerCookie(SocketUser Sender, SocketUser Receiver, ulong GuildId)
        {
            classes.GuildUser_Database gudb = getUserFromGuild(GuildId, Receiver);
            if (gudb != null)
            {
                //Check if player can receiver cookie
                //dueTime = 5Minutes
                if (gudb.CanReceiveCookie())
                {
                    gudb.GivePlayerCookie(Sender.Mention);
                    Core.DatabaseHandler.GuildDatabase_ChangesMade = true;
                }
            }
        }
        /// <summary>
        /// Gives the player a cookie.
        /// <para>A cookie can be sent all 5 minutes</para>
        /// </summary>
        /// <param name="user"></param>
        /// <param name="GuildId"></param>
        public static void GivePlayerCookie(SocketGuildUser Sender, SocketGuildUser Receiver, ulong GuildId)
        {
            classes.GuildUser_Database gudb = getUserFromGuild(GuildId, Receiver);
            if (gudb != null)
            {
                //Check if player can receiver cookie
                //dueTime = 5Minutes
                if (gudb.CanReceiveCookie())
                {
                    gudb.GivePlayerCookie(Sender.Mention);
                    Core.DatabaseHandler.GuildDatabase_ChangesMade = true;
                }
            }
        }

        /// <summary>
        /// Increase the specific statistic counter +1
        /// </summary>
        /// <param name="user"></param>
        /// <param name="GuildId"></param>
        /// <param name="counter"></param>
        /// <param name="command"></param>
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
        /// <summary>
        /// Increase the specific statistic counter +1
        /// </summary>
        /// <param name="user"></param>
        /// <param name="GuildId"></param>
        /// <param name="counter"></param>
        /// <param name="command"></param>
        public static void IncreaseCount(SocketGuildUser user, ulong GuildId, Counters counter, string command = "")
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

        /// <summary>
        /// Imports a SocketUser to the GuildUserDatabase
        /// </summary>
        /// <param name="GuildID"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static classes.GuildUser_Database ImportUserToDB(ulong GuildID, SocketUser user)
        {
            classes.GuildUser_Database newUser = new classes.GuildUser_Database(user, GuildID, user.Id);
            GuildSettingsList.TryAdd($"{GuildID}|{user.Id}", newUser);
            return newUser;
        }
        /// <summary>
        /// Imports a SocketGuildUser to the GuildUserDatabase
        /// </summary>
        /// <param name="GuildID"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static classes.GuildUser_Database ImportUserToDB(ulong GuildID, SocketGuildUser user)
        {
            classes.GuildUser_Database newUser = new classes.GuildUser_Database(user, GuildID, user.Id);
            GuildSettingsList.TryAdd($"{GuildID}|{user.Id}", newUser);
            return newUser;
        }

        /// <summary>
        /// Removes the SocketUser from the GuildUserDatabase
        /// </summary>
        /// <param name="GuildID"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool DelteUserFromDB(ulong GuildID, SocketUser user)
        {
            return GuildSettingsList.TryRemove($"{GuildID}|{user.Id}", out classes.GuildUser_Database outUser);
        }
        /// <summary>
        /// Removes the SocketGuildUser from the GuildUserDatabase
        /// </summary>
        /// <param name="GuildID"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool DelteUserFromDB(ulong GuildID, SocketGuildUser user)
        {
            return GuildSettingsList.TryRemove($"{GuildID}|{user.Id}", out classes.GuildUser_Database outUser);
        }

        /// <summary>
        /// Returns a Guild_UserDatabse object from the specific key
        /// </summary>
        /// <param name="GuildId"></param>
        /// <param name="User"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Returns a Guild_UserDatabse object from the specific key
        /// </summary>
        /// <param name="GuildId"></param>
        /// <param name="User"></param>
        /// <returns></returns>
        private static classes.GuildUser_Database getUserFromGuild(ulong GuildId, SocketGuildUser User)
        {
            if (!GuildSettingsList.TryGetValue($"{GuildId}|{User.Id}", out classes.GuildUser_Database gdb))
            {
                return ImportUserToDB(GuildId, User);
            }
            else
            {
                return gdb;
            }
        }

        public static Counters CheckMessageType(string Content)
        {
            Counters type = Counters.Messages;
 
            //Regex Patterns
            string image_regex = @"(https?:) ?//?[^'"+@"<>]+?\.(jpg|jpeg|gif|png)";
            string url_regex = @"^(https?://)?[\w\-\._]+\.[a-zA-Z]{2,6}/?$";
            string youtube_regex = @"(https?://(www\.)?youtube\.com/.*v=\w+.*)|(https?://youtu\.be/\w+.*)|(.*src=.https?://(www\.)?youtube\.com/v/\w+.*)|(.*src=.https?://(www\.)?youtube\.com/embed/\w+.*)";

            //check if string is image
            Match imageMatch = Regex.Match(Content, image_regex);
            if (imageMatch.Success) { return type = Counters.Images; }
            //check if string is youtube url
            Match youtubeMatch = Regex.Match(Content, youtube_regex);
            if (youtubeMatch.Success) { return type = Counters.YoutubeLinks; }
            //check if string is url
            Match urlMatch = Regex.Match(Content, url_regex);
            if (urlMatch.Success) { return type = Counters.Links; }
          
            return type;
        }
    }
}
