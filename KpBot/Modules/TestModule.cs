
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Common;
using Microsoft.Extensions.Options;
using KpBot.Context;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace KpBot.BasicCommands
{
    public class TestModule : ModuleBase
    {
        private AppSettingsConfiguration _config;

        public TestModule(DiscordContext db, IOptions<AppSettingsConfiguration> config)
        {
            _config = config.Value;
        }

        [Command("ping")]
        [Alias("pong", "hello")]
        public Task PingAsync()
        {
            return ReplyAsync("pong");
        }
    }
}
