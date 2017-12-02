using System;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using Mayhem_Bot.Core;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Threading;

namespace Mayhem_Bot
{
    public class CoreProgram
    {
        static public DiscordSocketClient _client;
        static public ErrorHandler _errorHandler;
        static public ListenerHandler _listenerHandler;

        private CommandService _commands;
        private IServiceProvider _services;

        //Needs to remove after push
        private const string token = "Mzg1OTExMDAxMDI5MTQ4Njcy.DQIPbg.XJ1suCOfubEEQzRSbRnU2hMIJ4Q";
        public const char prefix = '!';
        private static void Main(string[] args) => new CoreProgram().MainAsync(args).GetAwaiter().GetResult();
        public async Task MainAsync(string[] args)
        {
            //Initialize a new SocketClient
            _client = new DiscordSocketClient();

            //Initialize a new ErrorHandler and direct errors direct to the handler
            _errorHandler = new ErrorHandler();
            _client.Log += _errorHandler._client_Log;
            //Set Listeners
            _listenerHandler = new ListenerHandler();
            SetListeners();
            //Initialize the CommandHandler
            await InstallCommandHandlerAnsyc();
            //Initialize the Database
            await LoadDatabase();
            //Launch the bot
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            //Block this task until the program is closed
            await Task.Delay(-1);
        }

       

        public async Task LoadDatabase()
        {
            //initialize a timer which saves the database after a period of time
            AutoResetEvent _autoEvent = new AutoResetEvent(false);
            SaveTimer = new Timer(SaveTimerExecute, _autoEvent, 1000, 10000);
            await DatabaseHandler.LoadDatabase();            
        }
       
        public async Task InstallCommandHandlerAnsyc()
        {
            //Adds the services to the ServiceProvider
            this._commands = new CommandService();
            this._services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();
            await InstallCommandsAsync();
        }

        private async Task InstallCommandsAsync()
        {
            //initialize the client MessageReceive event
            _client.MessageReceived += HandleCommands;
            //Discover all commands in this assembly - load
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private void SetListeners()
        {
            _client.JoinedGuild += _listenerHandler.JoinedGuild;
            _client.LeftGuild += _listenerHandler.LeftGuild;
            
        }

      

        private async Task HandleCommands(Discord.WebSocket.SocketMessage arg)
        {
            //Handle Commands right here
            var message = arg as SocketUserMessage;
            if (message == null) return;
            //Track where the command prefix ends and where the command begins
            int argPos = 0;
            //Check if author is a bot
            if (message.Author.IsBot) return;
            //Determine if the message is a command or a mention
            if (!(message.HasCharPrefix(CoreProgram.prefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos)))
            {
                //check if message is a private message
                if (message.Channel.Name == $"@{message.Author.Username}#{message.Author.DiscriminatorValue}")
                {
                    //Case private message
                    string text = "";
                    text = message.Attachments.Count > 0 ? $"[{message.Channel.Name}|{message.Author.Username}] whispered: {message.Content} 'FILE ATTACHED'" : $"[{message.Channel.Name}|{message.Author.Username}] whispered: {message.Content}";
                    //Send message to the logHandler
                    await _errorHandler._client_Log(new LogMessage(LogSeverity.Warning, "DM Chat", text));
                
                }
                else
                {
                    //Case public message
                    string text = "";
                    text = message.Attachments.Count > 0 ? $"[{message.Channel.Name}|{message.Author.Username}] wrote: {message.Content} 'FILE ATTACHED'" : $"[{message.Channel.Name}|{message.Author.Username}] wrote: {message.Content}";
                    //Send message to the logHandler
                    await _errorHandler._client_Log(new LogMessage(LogSeverity.Debug, "Discord Chat",text));
                   
                }
                return;
            };

            //Create CommandContext
            var context = new SocketCommandContext(_client, message);
            //Execute the Command
            var result = await _commands.ExecuteAsync(context, argPos, _services);
            //Send commanmd to the logHandler
            await _errorHandler._client_Log(new LogMessage(LogSeverity.Verbose, "Discord Chat", $"[{message.Channel.Name}|{message.Author.Username}] Command: {message.Content}"));

            //If error occures - direct them to the ErrorHandler
            if (!result.IsSuccess) { await _errorHandler.HandleCommandError(context, result, "HandleCommands"); }
        }


        #region Timer functions
        Timer SaveTimer;
        public async void SaveTimerExecute(Object StateInfo)
        {
            /*
             * After a period of time
             * the database will be save if
             * the ChangesMade property is set to true
             */
            await DatabaseHandler.SaveDatabase();
        }
        #endregion
    }
}
