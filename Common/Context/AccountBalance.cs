using Common.Context.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Common.Context
{
    [DbModel]
    public class AccountBalance : BasicModel<AccountBalance>
    {
        public AccountBalance(int id)
        {
            Id = id;
        }

        public AccountBalance()
        {

        }

        public decimal Amount { get; set; }

        [OnDeleteCascade]
        public DiscordUser Owner { get; set; }
    }
}
