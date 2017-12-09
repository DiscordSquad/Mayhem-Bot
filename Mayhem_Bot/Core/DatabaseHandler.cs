using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Mayhem_Bot.Core.ServerSettings;
using System.Reflection;

namespace Mayhem_Bot.Core
{
    public static class DatabaseHandler
    {
        //Represents a GuildDatabase which contains all nececessary lists/configs
        public class GuildDatabase
        {
            //Place all Setting/Cache lists right here
            public ConcurrentDictionary<ulong, Dictionary<Settings, bool>> SettingsList = new ConcurrentDictionary<ulong, Dictionary<Settings, bool>>();
        }

        //identifier to check if we have made some changes
        public static bool ChangesMade { get; set; }
        public static async Task SaveDatabase()
        {
            //If changes were made - write them into the database.json
            if (ChangesMade)
            {
                //create a new GuildDatabase object
                GuildDatabase gdb = new GuildDatabase();
                //implement all setting/cache lists in here!

                gdb.SettingsList = ServerSettings.SettingList;
                /*
                 * 
                 * 
                 * 
                 */

                //string serialization with json
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(gdb);
                //leave this path alone cause it reflects the current working dictionary
                string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"/database.json"; 
                //Check if the file exist
                if (!File.Exists(path))
                {
                    CreateDatabaseFile(path);
                }
                using (StreamWriter sw = new StreamWriter(path, false))
                {
                    //Write data async to the file
                    await sw.WriteAsync(json);
                    //don´t forget to close writers, too
                    sw.Close();
                }
                //set changesMade to false for new changes
                ChangesMade = false;
            }
            else { return; } //No changes detected
        }
        public static async Task LoadDatabase()
        {

            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"/database.json"; 
            //Check if the file exist
            if(!File.Exists(path)){
                CreateDatabaseFile(path);
            }
            using (StreamReader reader = new StreamReader(path))
            {
                //leave this path alone cause it reflects the current working dictionary
                string json = await reader.ReadToEndAsync();
                //create a new GuildDatabase from the json deserializer
                GuildDatabase gdb = Newtonsoft.Json.JsonConvert.DeserializeObject<GuildDatabase>(json);
                if (gdb == null) { gdb = new GuildDatabase(); }
                //set all setting/cache lists in here!
                ServerSettings.SettingList = gdb.SettingsList;
                /*
                 * 
                 * 
                 * 
                 * 
                 */
            }
        }
        private static void CreateDatabaseFile(string path)
        {
            using(FileStream fs = new FileStream(path,FileMode.Create, FileAccess.ReadWrite))
            {
                fs.Close();
            }
        }
    }
    
}
