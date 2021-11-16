using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp2.Domain.Enums;

namespace ChatApp2.Domain.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }

        public bool Private { get; set; }

        public bool Deleted { get; set; }

        public ConversationType ChatType { get; set; }


        //public List<Comment> Comments2 {get; set;}

        public ICollection<Message> Messages { get; set; }
    }
}
