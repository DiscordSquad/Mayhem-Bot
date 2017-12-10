using Discord;
using Discord.WebSocket;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mayhem_Bot.classes
{
    public class GuildUser_Database
    {
        /*
         * Due to issues with the defaultValue from json conver
         * Set JsonProperty´s defaultvaluehandling to populate
         * With this attribute - we can receive all values from the deserialized object
         */

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string UserName { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string Discriminator { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string GuildName { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public ulong UserId { get; private set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public ulong GuildId { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int Count_Messages { get; private set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int Count_Images { get; private set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int Count_Links { get; private set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int Count_Commands { get; private set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int Count_Youtube_Links { get; private set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public int Count_Cookies { get; private set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public DateTime LastCookieReceived { get; private set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string[] LastCommands = new string[5];
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public string[] LastCookies = new string[5];
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate)]
        public DateTime LastUserUpdate { get; set; }

        public GuildUser_Database()
        {

        }
        public GuildUser_Database(SocketUser user, ulong guildId, ulong userId)
        {
            this.UserName = user.Username;
            this.Discriminator = user.Discriminator;
            this.GuildName = CoreProgram._client.GetGuild(guildId).Name;
            this.UserId = userId;
            this.GuildId = guildId;
            this.Count_Commands = 0;
            this.Count_Cookies = 0;
            this.Count_Images = 0;
            this.Count_Links = 0;
            this.Count_Messages = 0;
            this.Count_Youtube_Links = 0;
            this.LastUserUpdate = DateTime.Now;
        }
        public GuildUser_Database(SocketGuildUser user, ulong guildId, ulong userId)
        {
            this.UserName = user.Username;
            this.Discriminator = user.Discriminator;
            this.GuildName = CoreProgram._client.GetGuild(guildId).Name;
            this.UserId = userId;
            this.GuildId = guildId;
            this.Count_Commands = 0;
            this.Count_Cookies = 0;
            this.Count_Images = 0;
            this.Count_Links = 0;
            this.Count_Messages = 0;
            this.Count_Youtube_Links = 0;
            this.LastUserUpdate = DateTime.Now;
        }
        public bool CanReceiveCookie()
        {
            LastUserUpdate = DateTime.Now;
            return (DateTime.Now >= LastCookieReceived.AddMinutes(5)) ? true : false;

        }
        public void GivePlayerCookie(string MentionUserName)
        {
            LastUserUpdate = DateTime.Now;
            Count_Cookies++;
            //Set the first available space for the last cookie
            //The last 5 cookies will be saved

            //Swap positions from 0-5 and delete the last one
            LastCookies.SetValue(LastCookies[3], 4);
            LastCookies.SetValue(LastCookies[2], 3);
            LastCookies.SetValue(LastCookies[1], 2);
            LastCookies.SetValue(LastCookies[0], 1);
            //insert recent cookie
            LastCookies.SetValue(MentionUserName, 0);
            LastCookieReceived = DateTime.Now;
        }
        public void IncreaseMessage()
        {
            LastUserUpdate = DateTime.Now;
            Count_Messages++;
        }
        public void IncreaseImages()
        {
            LastUserUpdate = DateTime.Now;
            Count_Images++;
        }
        public void IncreaseLinks()
        {
            LastUserUpdate = DateTime.Now;
            Count_Links++;
        }
        public void IncreaseCommands(string Command)
        {
            LastUserUpdate = DateTime.Now;
            Count_Commands++;
            //Set the first available space for the last command
            //The last 5 commands will be saved

                //Swap positions from 0-5 and delete the last one
                LastCommands.SetValue(LastCommands[3], 4);
                LastCommands.SetValue(LastCommands[2], 3);
                LastCommands.SetValue(LastCommands[1], 2);
                LastCommands.SetValue(LastCommands[0], 1);
                //insert recent command
                LastCommands.SetValue(Command, 0);
        }
        public void IncreaseYoutubeLinks()
        {
            LastUserUpdate = DateTime.Now;
            Count_Youtube_Links++;
        }
    }
}
