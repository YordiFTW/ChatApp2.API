using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp2.Domain.Enums;
using Microsoft.AspNetCore.Identity;

namespace ChatApp2.Domain.Models
{
    public class User : IdentityUser
    {
        

        public AccountType UserType { get; set; } = AccountType.DefaultUser;

        public bool IsAdmin { get; set; }
        public IList<Group> UserChats { get; set; }

        public bool IsLoggedIn { get; set; }

        public bool IsBlocked { get; set; }

        public string LastAction { get; set; }
    }
}
