using Common.Context;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace KpBot.Services
{
    public class BaseService
    {
        public readonly IServiceProvider _services;
        public readonly CommandService _commands;
        public readonly DiscordSocketClient _client;
        public readonly GamesAndStuffContext _context;

        public BaseService(IServiceProvider services)
        {
            _services = services;
            _commands = services.GetRequiredService<CommandService>();
            _client = services.GetRequiredService<DiscordSocketClient>();
            _context = services.GetRequiredService<GamesAndStuffContext>();
        }
    }
}
