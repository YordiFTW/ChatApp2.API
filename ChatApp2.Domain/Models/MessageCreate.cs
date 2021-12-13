using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp2.Domain.Models
{
   public class MessageCreate
    {
        public string Content { get; set; }
        public int ChatId { get; set; }
    }
}
