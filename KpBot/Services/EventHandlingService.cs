using Common.Context;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KpBot.Services
{
    public class EventHandlingService : BaseService
    {
        private DiscordUserService _userService;
        public EventHandlingService(IServiceProvider services) : base(services)
        {
            _userService = services.GetRequiredService<DiscordUserService>();
        }

        public void InitializeAsync()
        {
            _client.Ready += OnReady;
            _client.UserJoined += OnUserJoined;
        }     

        public async Task OnReady()
        {
            IList<SocketGuildUser> usersToSave = new List<SocketGuildUser>();
            IList<DiscordUser> usersToDelete = new List<DiscordUser>();

            IEnumerable<SocketGuildUser> users = _client.Guilds.SelectMany(x => x.Users);
            List<DiscordUser> savedUsers = _context.DiscordUsers.ToList();

            usersToDelete = savedUsers.Where(x => !users.Select(y => y.Id).Contains(x.DiscordId)).ToList();
          
            await _userService.CreateDiscordUsers(usersToSave);
            await _userService.DeleteDiscordUsers(usersToDelete); 
        }

        public async Task OnUserJoined(SocketGuildUser user)
        {
            
        }
    }
}
