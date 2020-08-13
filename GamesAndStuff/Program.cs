using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using KpBot;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace GamesAndStuff
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IWebHost webHost = CreateWebHostBuilder(args).Build();

            MainBot KpBot = new MainBot(webHost.Services);
            Task t1 = KpBot.KeepAlive();
            Task t2 = KpBot.MainAsync();
            webHost.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseDefaultServiceProvider(options =>
                        options.ValidateScopes = false);
    }
}
