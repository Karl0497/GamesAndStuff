using Common.Context;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace KpBot.Services
{
    public class DiscordUserService :  BaseService
    {
        public DiscordUserService(IServiceProvider services) : base(services)
        {

        }


        /// <summary>
        /// Delete Discord users
        /// </summary>
        /// <param name="users"> List of users - must be loaded</param>
        /// <returns></returns>
        public async Task DeleteDiscordUsers(IList<DiscordUser> users)
        {
            // Filter users already in the db
            List<ulong> allUsersDiscordId = _context.DiscordUsers.AsEnumerable().Select(x => x.DiscordId).ToList();
            users = users.Where(x => allUsersDiscordId.Contains(x.DiscordId)).ToList();

            if (users.Count == 0)
            {
                return;
            }

            _context.RemoveRangeCascade(users);
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Create Discord users. Each user must have DiscordID
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public async Task CreateDiscordUsers(IList<SocketGuildUser> users)
        {
            await AddDiscordUsers(ConvertGuildUsersToDiscordUsers(users));
        }

        public async Task AddDiscordUsers(IList<DiscordUser> users)
        {
            // Filter users already in the db
            List<ulong> allUsersDiscordId = _context.DiscordUsers.AsEnumerable().Select(x => x.DiscordId).ToList();
            users = users.Where(x => !allUsersDiscordId.Contains(x.DiscordId)).ToList();

            if (users.Count == 0)
            {
                return;
            }

            if (users.Any(x => x.DiscordId == 0 || x.DisplayName == null))
            {
                throw new Exception("One or more users do not have DiscordId or DisplayName");
            }

            foreach (var user in users)
            { 
                if (user.Balance == null)
                {
                    user.Balance = new AccountBalance();
                }
            }

            _context.DiscordUsers.AddRange(users);
            await _context.SaveChangesAsync();
        }

        public IList<DiscordUser> ConvertGuildUsersToDiscordUsers(IList<SocketGuildUser> users)
        {
            return users.Select(x => new DiscordUser
            {
                DiscordId = x.Id,
                DisplayName = x.Username,
                Balance = new AccountBalance()
            }).ToList();
        }
    }
}
