using Mayhem_Bot.Core;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Mayhem_Bot.Databases
{
    public static class ServerSettings
    {
        //TKey = Serveruid -> TValue = bool - represents the server settings of a discord guild
        /// <summary>
        /// ConcurrentDictionary which contains guild settings
        /// </summary>
        public static ConcurrentDictionary<ulong, Dictionary<Settings, bool>> SettingList = new ConcurrentDictionary<ulong, Dictionary<Settings, bool>>();

        //enum of settings a server can have
        /// <summary>
        /// Represents the ServerSettings
        /// </summary>
        public enum Settings
        {
            SendErrorMessage,
            SendPrivateMessage
        }
        public static bool GetSettingsValue(Settings setting, ulong guid)
        {
            //Get the SettingList from the Serverlist
            bool found = SettingList.TryGetValue(guid, out Dictionary<Settings, bool> gSettings);
            //Create new server settings if the server is not listed
            if (!found) { CreateNewServerSettingList(guid); found = SettingList.TryGetValue(guid, out gSettings);}
            //Get the specific setting from the SettingList
            if (found){ gSettings.TryGetValue(setting, out bool retVal); return retVal;} else { return found; }
            //return Setting
        }
        public static void SetSettingsValue(Settings setting, bool Value, ulong guid)
        {
            //Get the SettingList from the Serverlist
            SettingList.TryGetValue(guid, out Dictionary<Settings, bool> gSettings);
            //Update setting
            gSettings[setting] = Value;
            //Save settinglist
            SettingList[guid] = gSettings;
            //Saves the server setting database
            SaveServerSettingsDatabase();
        }

        public static void DeleteServerSettingList(ulong guid)
        {
            //removes the guild from the list
            SettingList.TryRemove(guid, out Dictionary<Settings, bool> list);
            //Saves the server setting database
            SaveServerSettingsDatabase();
        }
        public static void CreateNewServerSettingList(ulong guid)
        {
            //If the bot joins a new server - create a default server setting list
            Dictionary<Settings, bool> settings = new Dictionary<Settings, bool>();
            //default setting values
            settings.Add(Settings.SendErrorMessage, false);
            settings.Add(Settings.SendPrivateMessage, true);
            /*
             * 
             * 
             * 
             * 
             * 
             * 
             */
            //Adds the setting for the server to the global list
            SettingList.TryAdd(guid, settings);
            //Saves the server setting database
            SaveServerSettingsDatabase();
        }
        
        public static void SaveServerSettingsDatabase()
        {
            //determins that we have made some changes we need to save in the next interval
            DatabaseHandler.Database_ChangesMade = true;
        }
    }
}
