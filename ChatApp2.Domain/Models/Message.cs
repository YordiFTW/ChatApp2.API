using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp2.Domain.Enums;

namespace ChatApp2.Domain.Models
{
    public class Message
    {
        public int MessageId { get; set; }

        public string UserName { get; set; }

        public bool Deleted { get; set; }

        public string Date { get; set; }

        public ConversationType Type { get; set; }
        public string Content { get; set; }
        public string GifLink { get; set; }

        public string EmoticonLink { get; set; }

        public int ChatId { get; set; }

        public Chat Chat { get; set; }



    }
}
