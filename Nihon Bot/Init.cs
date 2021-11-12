using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Nihon_Bot
{
    class Init
    {
        public CommandService Commands;
        public DiscordSocketClient Client;
        public IServiceProvider Services;

        public string Prefix = "?"; /* Bot Prefix */

        static void Main(string[] args) => new Init().Entry().GetAwaiter().GetResult();

        private void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
            => BotCommands.ErrorEmbed(e.ExceptionObject.ToString()); /* Handle Exceptions */

        public async Task Entry()
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler; /* Exception Event */

            Client = new DiscordSocketClient();
            Commands = new CommandService();

            Console.Title = $"Nihon Bot | Version 1.0.0";
            Console.SetWindowSize(80, 20);

            string Watermark = @"
  
  ███╗░░██╗██╗██╗░░██╗░█████╗░███╗░░██╗
  ████╗░██║██║██║░░██║██╔══██╗████╗░██║
  ██╔██╗██║██║███████║██║░░██║██╔██╗██║
  ██║╚████║██║██╔══██║██║░░██║██║╚████║
  ██║░╚███║██║██║░░██║╚█████╔╝██║░╚███║
  ╚═╝░░╚══╝╚═╝╚═╝░░╚═╝░╚════╝░╚═╝░░╚══╝";

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{Watermark}\n");
            Console.ForegroundColor = ConsoleColor.White;

            string Token = "Token Here"; /* Discord Bot Token Here */

            Client.Log += Log;
            await RegisterCommandsAsync();
            await Client.LoginAsync(TokenType.Bot, Token);
            await Client.StartAsync();

            await Client.SetGameAsync("Game Status Here", null, ActivityType.Playing);

            await Task.Delay(-1);

            Client = new DiscordSocketClient();
            Commands = new CommandService();
            Services = new ServiceCollection()
                .AddSingleton(Client)
                .AddSingleton(Commands)
                .BuildServiceProvider();

            await RegisterCommandsAsync();
            await Client.LoginAsync(TokenType.Bot, Token);
            await Client.StartAsync();
        }

        public Task Log(LogMessage Message)
        {
            Console.WriteLine($"  {Message}");
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            Client.MessageReceived += HandleCommandAsync;
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), Services);
        }

        Commands BotCommands = new();
        private async Task HandleCommandAsync(SocketMessage Argument)
        {
            var Message = Argument as SocketUserMessage;
            var Context = new SocketCommandContext(Client, Message);
            if (Message.Author.IsBot) return;

            int ArgumentPosition = 0;
            if (Message.HasStringPrefix(Prefix, ref ArgumentPosition))
            {
                var Result = await Commands.ExecuteAsync(Context, ArgumentPosition, Services);
                if (!Result.IsSuccess) await Message.Channel.SendMessageAsync(null, false, BotCommands.ErrorEmbed(Result.ErrorReason).Build()); /* Catches Discord Bot Error And Makes Into Embed And Sends */
                if (Result.Error.Equals(CommandError.UnmetPrecondition)) await Message.Channel.SendMessageAsync(Result.ErrorReason); /* Does Just A Simple Message Rather */
            }
        }
    }
}
