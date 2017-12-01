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
            AutoResetEvent _autoEvent = new AutoResetEvent(false);

            //TIMER FOR DATABASE FILE HANDLING - Due to writing and reading process it wouldn´t be recommendet to write data a thousand times at once... So in this case
            //we create a filehandler which save the database all 10 seconds if needed.

            //Timer _tm = new Timer(DatabaseHandler.SaveDatabase(), _autoEvent, 1000, 10000);
            DatabaseHandler.LoadDatabase();            
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
            if (!(message.HasCharPrefix(CoreProgram.prefix, ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;
            //Create CommandContext
            var context = new SocketCommandContext(_client, message);
            //Execute the Command
            var result = await _commands.ExecuteAsync(context, argPos, _services);
            //If error occures - direct them to the ErrorHandler
            if (!result.IsSuccess) { await _errorHandler.HandleCommandError(context, result, "HandleCommands"); }
        }
    }
}
