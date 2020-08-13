using Discord;
using Discord.Commands;
using Discord.WebSocket;
using HtmlAgilityPack;
using KpBot.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using WindowsInput;
using Microsoft.Extensions.Configuration;
using KpBot.Context;
using Microsoft.EntityFrameworkCore;
using Common;
using Microsoft.Extensions.Options;

namespace KpBot
{
    public static class MainBotServiceInjection
    {
        public static void KpBotConfigureServices(this IServiceCollection serviceCollection)
        {
            serviceCollection
                   .AddSingleton<DiscordSocketClient>()
                   .AddSingleton<CommandService>()
                   .AddSingleton<CommandHandlingService>()
                   .AddSingleton<HttpClient>();
        }
    }

    public class MainBot
    {
        private IServiceProvider _services;

        public MainBot(IServiceProvider services)
        {
            _services = services;
        }

        public async Task MainAsync()
        {
            // You should dispose a service provider created using ASP.NET
            // when you are finished using it, at the end of your app's lifetime.
            // If you use another dependency injection framework, you should inspect
            // its documentation for the best way to do this.

            DiscordSocketClient client = _services.GetRequiredService<DiscordSocketClient>();
            AppSettingsConfiguration config = _services.GetRequiredService<IOptions<AppSettingsConfiguration>>().Value;
            // Tokens should be considered secret data and never hard-coded.
            // We can read from the environment variable to avoid hardcoding.
            await client.LoginAsync(TokenType.Bot, config.DiscordTokens.KpBot);
            await client.StartAsync();

            // Here we initialize the logic required to register our commands.
            await _services.GetRequiredService<CommandHandlingService>().InitializeAsync();

            await Task.Delay(Timeout.Infinite);

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
    }
}
