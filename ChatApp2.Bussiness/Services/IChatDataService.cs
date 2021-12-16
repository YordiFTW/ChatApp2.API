using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp2.Domain.Models;
using PagedList;

namespace ChatApp2.Bussiness.Services
{
    public interface IChatDataService
    {
        Message ProfanityChecker(Message message);
        Chat TogglePrivateChat(Chat chat);

        Chat Create(ChatCreate chatCreate);

        Chat GetChatById(int chatId);
        void Delete(int chatId);

        void ToggleVisibility(int chatId);

        IEnumerable<Chat> GetAllChats();

        List<Message> GetAllMessagesByChat(int chatId);

        IPagedList<Message> GetAllMessagesByPaging(int page);

        Message CreateMessage(MessageCreate messageCreate);

        List<Message> SearchMessagesBySearchTerm(string searchTerm);

        IEnumerable<Chat> GetCurrentUserPrivateChats();
    }
}
