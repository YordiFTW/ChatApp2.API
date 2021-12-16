using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp2.Domain.Models
{
   public class UserAddtoGroup
    {
        public int chatId { get; set; }
        public string userName { get; set; }
        public bool isAccepted { get; set; }
    }
}
