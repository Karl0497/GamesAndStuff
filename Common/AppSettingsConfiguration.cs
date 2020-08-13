using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common
{
    public class AppSettingsConfiguration
    {
        public ConnectionStrings ConnectionStrings { get; set; }
        public DiscordTokens DiscordTokens { get; set; }
    }

    public class ConnectionStrings
    {
        public string DiscordContext { get; set; }
    }

    public class DiscordTokens
    {
        public string KpBot { get; set; }
    }
}
