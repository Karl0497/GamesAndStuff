
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Common;
using Microsoft.Extensions.Options;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Common.Context;
using Microsoft.EntityFrameworkCore;

namespace KpBot.BasicCommands
{
    public class TestModule : ModuleBase
    {
        private GamesAndStuffContext _db;

        public TestModule(GamesAndStuffContext db)
        {
            _db = db;
        }

        [Command("ping")]
        [Alias("pong", "hello")]
        public async Task PingAsync()
        {
            var testUser = _db.DiscordUsers.Include(x => x.Balance).ThenInclude(x=>x.Jobs).FirstOrDefault(x => x.DiscordId == 681791074376876044);
            testUser.Balance = new AccountBalance();
            var job = new ScheduledJob();
            job.Owner = testUser.Balance;
            _db.ScheduledJobs.Add(job);
            await _db.SaveChangesAsync();
            await ReplyAsync("pong");
        }


        [Command("delete")]
        public async Task Delete()
        {
            var testUser = _db.DiscordUsers.FirstOrDefault(x => x.DiscordId == 681791074376876044);
            _db.RemoveCascade(testUser);
            await _db.SaveChangesAsync();
            await ReplyAsync("pong");
        }
    }
}
