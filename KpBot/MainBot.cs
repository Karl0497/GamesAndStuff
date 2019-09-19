using Discord;
using Discord.Commands;
using Discord.WebSocket;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace KpBot
{
    public class MainBot
    {
        private DiscordSocketClient _client;
        private CommandService Commands;
        private ServiceCollection ServiceCollection;
        private IServiceProvider Services;


        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            //  You can assign your bot token to a string, and pass that in to connect.
            //  This is, however, insecure, particularly if you plan to have your code hosted in a public repository.
            ServiceCollection = new ServiceCollection();
            Commands = new CommandService();

            Services = new ServiceCollection()
                .AddSingleton(_client)
                .BuildServiceProvider();
            var token = "NjIzODUwMDEyNDc2MTc4NDM3.XYM-fg.GH3TtWutJC7yJO7sQ4Pw3ZoYY4c";
            await InstallCommands();
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
        public async Task KeepAlive()
        {
            HtmlWeb web;
            HtmlDocument HtmlDoc;
            while (true)
            {
                web = new HtmlWeb();
                await Task.Delay(900000);
                HtmlDoc = await web.LoadFromWebAsync("https://gamesandstuff.azurewebsites.net//");
            }

        }
        public async Task InstallCommands()
        {

            _client.MessageReceived += HandleCommand;
            await Commands.AddModuleAsync<Assembly>(Services);

        }

        public async Task HandleCommand(SocketMessage messageParam)
        {

            // Don't process the command if it was a System Message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            var context = new CommandContext(_client, message);
            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;
            string[] triggers = new string[]{ "what", "when", "how", "who", "where", "why", "?" };
            if (triggers.Any(x => message.Content.ToLower().Split(' ').Contains(x)))
            {
                await context.Channel.SendMessageAsync("ur mum");

            }
        }
    }

 
}
