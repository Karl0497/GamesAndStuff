using Common.Context.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Context
{
    [DbModel]
    public class DiscordUser : BasicModel<DiscordUser>
    {
        public DiscordUser(int id)
        {
            Id = id;
        }

        public DiscordUser()
        {

        }

        public ulong DiscordId { get; set; }
        public string DisplayName { get; set; }

        [ForeignKey(nameof(AccountBalance))]
        [OnDeleteCascade]
        public AccountBalance Balance { get; set; }

        public string Mention 
        {
            get
            {
                return $"<@!{DiscordId}>";
            }
        }
        
        
    }
}
