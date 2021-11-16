using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp2.Domain.Models;
using PagedList;

namespace ChatApp2.Bussiness.Repositories
{
    public interface IChatRepository
    {
        IEnumerable<Chat> GetAllChats();
        List<Message> GetAllMessagesByChat(int chatId);
        Chat GetChatbyId(int chatId);
        Chat GetChatbyName(string chatName);
        Chat AddChat(Chat chat);
        Chat UpdateChat(Chat chat);

        IPagedList<Message> GetAllMessagesByPaging();
        List<Message> SearchMessagesBySearchTerm(string searchTerm);

        Chat TogglePrivateChat(Chat chat);
        void DeleteChat(int chatId);
    }
}
