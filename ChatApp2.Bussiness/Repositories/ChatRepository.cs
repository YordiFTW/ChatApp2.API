using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp2.Domain.DbContexts;
using ChatApp2.Domain.Models;
using PagedList;

namespace ChatApp2.Bussiness.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly ChatAppDbContext _mBDbContext;


        public ChatRepository(ChatAppDbContext mBDbContext)
        {
            _mBDbContext = mBDbContext;

        }

        public Chat AddChat(Chat chat)
        {
            var addChat = _mBDbContext.Chats.Add(chat);
            _mBDbContext.SaveChanges();
            return addChat.Entity;
        }

        public void DeleteChat(int chatId)
        {
            var Chat = _mBDbContext.Chats.FirstOrDefault(e => e.Id == chatId);
            if (Chat == null) return;

            _mBDbContext.Chats.Remove(Chat);
            _mBDbContext.SaveChanges();
        }

        public IEnumerable<Chat> GetAllChats()
        {
            return _mBDbContext.Chats;
        }

        public List<Message> GetAllMessagesByChat(int chatId)
        {
            List<Message> list = new List<Message>();

            foreach (var item in (_mBDbContext.Messages.Where(x => x.Chat.Id == chatId)))
            {
                list.Add(item);
            }
            return list;

        }

        public List<Message> GetAllMessages()
        {

            return _mBDbContext.Messages.ToList();

        }
        public IPagedList<Message> GetAllMessagesByPaging()
        {
            return _mBDbContext.Messages.ToPagedList(1, 3);
        }

        public Chat GetChatbyId(int chatId)
        {
            return _mBDbContext.Chats.FirstOrDefault(c => c.Id == chatId);
        }

        public Chat GetChatbyName(string chatName)
        {
            return _mBDbContext.Chats.FirstOrDefault(c => c.Name == chatName);
        }

        public Chat TogglePrivateChat(Chat chat)
        {
            if (chat.Private == true)
                chat.Private = false;
            else
                chat.Private = true;

            _mBDbContext.SaveChanges();

            return chat;
        }

        public Chat UpdateChat(Chat chat)
        {
            var updateChat = _mBDbContext.Chats.FirstOrDefault(e => e.Id == chat.Id);

            if (updateChat != null)
            {
                updateChat.Name = chat.Name;
                updateChat.Content = chat.Content;
                updateChat.Messages = chat.Messages;

                _mBDbContext.SaveChanges();

                return updateChat;
            }
            return null;
        }

        public List<Message> SearchMessagesBySearchTerm(string searchTerm)
        {
            List<Message> listofmessages = new List<Message>();


            foreach (Message message in _mBDbContext.Messages)
            {
                if (message.Content.Contains(searchTerm))
                {
                    listofmessages.Add(message);
                }
            }

            return listofmessages;
        }
    }
}
