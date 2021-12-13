using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp2.Domain.Models;

namespace ChatApp2.Bussiness.Services
{
    public interface IChatDataService
    {
        Message ProfanityChecker(Message message);
        Chat TogglePrivateChat(Chat chat);

        Chat Create(ChatCreate chatCreate);
    }
}
