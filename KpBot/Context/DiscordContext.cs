using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KpBot.Context
{
    public class DiscordContext : DbContext
    {
        public DiscordContext(DbContextOptions<DiscordContext> options)
            : base(options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder.UseSqlServer("Server=localhost;Database=GameContext;Trusted_Connection=True;");
        //}

        public DiscordContext()
        {

        }

        public DbSet<Student> Students { get; set; }
    }
}
