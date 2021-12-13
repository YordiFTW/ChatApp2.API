using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp2.Domain.Models
{
    public class ChatCreate
    {
        public string Chatname { get; set; }
        public bool Hidden { get; set; }

        public int Maxusers { get; set; }

        public string Password { get; set; }


    }
}
