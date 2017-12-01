using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Mayhem_Bot.Core
{
    public static class ServerSettings
    {
        //TKey = Serveruid -> TValue = bool - represents the server settings of a discord guild
        public static ConcurrentDictionary<ulong, Dictionary<Settings, bool>> SettingList = new ConcurrentDictionary<ulong, Dictionary<Settings, bool>>();

        //enum of settings a server can have
        public enum Settings
        {
            SendErrorMessage
        }
        public static bool GetSettingsValue(Settings setting, ulong guid)
        {
            //Get the SettingList from the Serverlist
            SettingList.TryGetValue(guid, out Dictionary<Settings, bool> gSettings);
            //Get the specific setting from the SettingList
            gSettings.TryGetValue(setting, out bool retVal);
            //return Setting
            return retVal;
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

        public static void CreateNewServerSettingList(ulong guid)
        {
            //If the bot joins a new server - create a default server setting list
            Dictionary<Settings, bool> settings = new Dictionary<Settings, bool>();

            //default setting values
            settings.Add(Settings.SendErrorMessage, true);


            //Adds the setting for the server to the global list
            SettingList.TryAdd(guid, settings);
            //Saves the server setting database
            SaveServerSettingsDatabase();
        }
        
        public static void SaveServerSettingsDatabase()
        {
            DatabaseHandler.ChangesMade = true;
        }
        
        //Property sets
        private static bool? SendErrorMessage { get; set; }
    }
}
