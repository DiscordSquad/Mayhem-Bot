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
        public string UserName { get; set; }
        public string Discriminator { get; set; }
        public string GuildName { get; set; }
        public ulong UserId { get; private set; }
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
        public DateTime LastCookieReceived { get; private set; }
        public string[] LastCommands = new string[5];
        public GuildUser_Database()
        {

        }
        public GuildUser_Database(SocketUser user, ulong guildId)
        {
            this.UserName = user.Username;
            this.Discriminator = user.Discriminator;
            this.GuildName = CoreProgram._client.GetGuild(guildId).Name;
            this.UserId = user.Id;
            this.GuildId = guildId;
            this.Count_Commands = 0;
            this.Count_Cookies = 0;
            this.Count_Images = 0;
            this.Count_Links = 0;
            this.Count_Messages = 0;
            this.Count_Youtube_Links = 0;
        }
        public bool CanReceiveCookie()
        {
            DateTime now = DateTime.Now;
            int minutes = 0;
            minutes = now.Minute - LastCookieReceived.Minute;
            return minutes >= 5 ? true : false;
        }
        public void GivePlayerCookie()
        {
            Count_Cookies++;
        }
        public void IncreaseMessage()
        {
            Count_Messages++;
        }
        public void IncreaseImages()
        {
            Count_Images++;
        }
        public void IncreaseLinks()
        {
            Count_Links++;
        }
        public void IncreaseCommands(string Command)
        {
            Count_Commands++;
            //Set the first available space for the last command
            //The last 5 commands will be saved

            //Check if we have space left
            if (String.IsNullOrEmpty(LastCommands[4]))
            {
                for (int i = 0; i < LastCommands.Length; i++)
                {
                    //Find the last space which is empty and insert command
                    if (String.IsNullOrEmpty(LastCommands[i]))
                    {
                        LastCommands.SetValue(Command, i);
                        break;
                    }
                }
            }
            else
            {
                //Swap positions from 0-5 and delete the last one
                LastCommands.SetValue(LastCommands[3], 4);
                LastCommands.SetValue(LastCommands[2], 3);
                LastCommands.SetValue(LastCommands[1], 2);
                LastCommands.SetValue(LastCommands[0], 1);
                //insert recent command
                LastCommands.SetValue(Command, 0);
            }
        }
        public void IncreaseYoutubeLinks()
        {
            Count_Youtube_Links++;
        }
    }
}
