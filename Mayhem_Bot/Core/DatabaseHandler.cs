using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Mayhem_Bot.Databases.ServerSettings;
using System.Reflection;
using Discord;
using Mayhem_Bot.Databases;

namespace Mayhem_Bot.Core
{
    public static class DatabaseHandler
    {
        //Represents a GuildDatabase which contains all nececessary lists/configs
        public class ServerDatabase
        {
            //Place all Setting/Cache lists right here
            public DateTime LastUpdate { get; set; }
            public ConcurrentDictionary<ulong, Dictionary<Settings, bool>> ServerDatabaseDictionary = new ConcurrentDictionary<ulong, Dictionary<Settings, bool>>();
        }
        public class GuildUserDatabase
        {
            public DateTime LastUpdate { get; set; }
            public ConcurrentDictionary<string, classes.GuildUser_Database> GuildUserDatabaseDictionary = new ConcurrentDictionary<string, classes.GuildUser_Database>();
        }
        //identifier to check if we have made some changes
        public static bool Database_ChangesMade { get; set; }
        public static bool GuildDatabase_ChangesMade { get; set; }
        public static async Task PrepareSaveDatabases()
        {
            //Check which database has changes
            if (Database_ChangesMade){await SaveDatabase(); await CoreProgram._errorHandler._client_Log(new LogMessage(LogSeverity.Debug, "PrepareSaveDatabase", "ServerDatabase has been saved!")); }
            if (GuildDatabase_ChangesMade){await SaveGuildDatabase(); await CoreProgram._errorHandler._client_Log(new LogMessage(LogSeverity.Debug, "PrepareSaveDatabase", "GuildDatabase has been saved!")); }  
        }


        public static async Task SaveDatabase()
        {

            //create a new GuildDatabase object
            ServerDatabase sdb = new ServerDatabase();
            //implement all setting/cache lists in here!

            sdb.ServerDatabaseDictionary = ServerSettings.SettingList;
            sdb.LastUpdate = DateTime.Now;

            //string serialization with json
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(sdb);
            //leave this path alone cause it reflects the current working dictionary
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"/ServerDatabase.json";
            //Check if the file exist
            if (!File.Exists(path))
            {
                await CreateDatabaseFile(path);
            }
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                //Write data async to the file
                await sw.WriteAsync(json);
                //don´t forget to close writers, too
                sw.Close();
            }
            //set changesMade to false for new changes
            Database_ChangesMade = false;
        }
        public static async Task SaveGuildDatabase()
        {

            //create a new GuildDatabase object
            GuildUserDatabase gdb = new GuildUserDatabase();
            //implement all setting/cache lists in here!

            gdb.GuildUserDatabaseDictionary = Databases.GuildDatabase.GuildSettingsList;
            gdb.LastUpdate = DateTime.Now;
       
            //string serialization with json
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(gdb);
            //leave this path alone cause it reflects the current working dictionary
            string path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"/GuildUserDatabase.json";
            //Check if the file exist
            if (!File.Exists(path))
            {
                await CreateDatabaseFile(path);
            }
            using (StreamWriter sw = new StreamWriter(path, false))
            {
                //Write data async to the file
                await sw.WriteAsync(json);
                //don´t forget to close writers, too
                sw.Close();
            }
            //set changesMade to false for new changes
            GuildDatabase_ChangesMade = false;
        }



        public static async Task LoadDatabase()
        {
            await CoreProgram._errorHandler._client_Log(new LogMessage(LogSeverity.Debug, "LoadDatabase", "Loading databases"));
            string[] path = new string[2] { Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"/ServerDatabase.json", Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"/GuildUserDatabase.json" };
            int pathCount = 0;
            foreach (string _path in path)
            {
                //Check if the file exist
                if (!File.Exists(_path))
                {
                    await CoreProgram._errorHandler._client_Log(new LogMessage(LogSeverity.Warning, "LoadDatabase", "File not found... create file!"));
                    await CreateDatabaseFile(_path);
                }
                using (StreamReader reader = new StreamReader(_path))
                {
                    //leave this path alone cause it reflects the current working dictionary
                    string json = await reader.ReadToEndAsync();
                    if (pathCount == 0)
                    {
                        //create a new ServerDatabase from the json deserializer
                        ServerDatabase sdb = Newtonsoft.Json.JsonConvert.DeserializeObject<ServerDatabase>(json);
                        //set all setting/cache lists in here!
                        if (sdb != null) { ServerSettings.SettingList = sdb.ServerDatabaseDictionary; await CoreProgram._errorHandler._client_Log(new LogMessage(LogSeverity.Debug, "LoadDatabase", "Loaded: server_database...")); }
                        else { await CoreProgram._errorHandler._client_Log(new LogMessage(LogSeverity.Critical, "LoadDatabase", "Could not load ServerDatabase", new NullReferenceException())); }
                    }
                    if (pathCount == 1)
                    {
                        //create a new GuildDatabase from the json deserializer
                        GuildUserDatabase gdb = Newtonsoft.Json.JsonConvert.DeserializeObject<GuildUserDatabase>(json);
                        //set all setting/cache lists in here!
                        if (gdb != null) { Databases.GuildDatabase.GuildSettingsList = gdb.GuildUserDatabaseDictionary; await CoreProgram._errorHandler._client_Log(new LogMessage(LogSeverity.Debug, "LoadDatabase", "Loaded: guild_database...")); }
                        else { await CoreProgram._errorHandler._client_Log(new LogMessage(LogSeverity.Critical, "LoadDatabase", "Could not load GuildUserDatabase", new NullReferenceException())); }
                    }
                }
                pathCount++;
            }
        }
        private static async Task CreateDatabaseFile(string path)
        {
            try
            {
                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
                {
                    fs.Close();
                    await CoreProgram._errorHandler._client_Log(new LogMessage(LogSeverity.Warning, "CreateDatabaseFile", "File successfully created"));
                }
            }
            catch (Exception ex)
            {
                await CoreProgram._errorHandler._client_Log(new LogMessage(LogSeverity.Critical, "CreateDatabaseFile", "Critical error detected. Program will stop!", ex));
                Thread.Sleep(10000);
                System.Environment.Exit(0);
            }
            
        }
    }

}
