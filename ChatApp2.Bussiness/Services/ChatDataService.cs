using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatApp2.Bussiness.Repositories;
using ChatApp2.Domain.Enums;
using ChatApp2.Domain.Lists;
using ChatApp2.Domain.Models;
using Microsoft.AspNetCore.Identity;
using PagedList;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ChatApp2.Bussiness.Services;
using ChatApp2.Domain.DbContexts;

namespace ChatApp2.Bussiness.Services
{
    public class ChatDataService : IChatDataService
    {
        private IGenericRepository<Chat> genericRepository = null;
        private readonly IGenericRepository<Group> grouprepo;
        private readonly UserManager<User> userManager;
        private readonly HttpContextAccessor httpContextAccessor;

        public ChatDataService(IGenericRepository<Chat> genericRepository, IGenericRepository<Group> grouprepo, UserManager<User> userManager, HttpContextAccessor httpContextAccessor)
        {
            this.genericRepository = genericRepository;
            this.grouprepo = grouprepo;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        ProfanityList profanityList = new ProfanityList();

        public Chat Create(ChatCreate chatCreate)
        {
            Chat chat = new Chat();

            chat.Name = chatCreate.Chatname;

            if (chatCreate.Hidden == false)
                chat.Private = false;
            else
                chat.Private = true;

            chat.MaxUsers = chatCreate.Maxusers;
            chat.Password = chatCreate.Password;

            chat.Content = "";

            genericRepository.Insert(chat);
            genericRepository.Save();

            Group group = new Group();
            group.Chat = genericRepository.GetById(chat.Id);
            group.User = userManager.GetUserAsync(httpContextAccessor.HttpContext.User).Result;

            grouprepo.Insert(group);
            grouprepo.Save();


            

            return chat;
        }

        public Message ProfanityChecker(Message message)
        {

            foreach (string word in profanityList._wordList)
            {
                if (message.Content.Contains(word))
                {
                    string reformed = word;

                    foreach (char letter in word)
                    {

                        reformed = reformed.Replace(letter, '*');
                    }

                    message.Content = message.Content.Replace(word, reformed);
                }
                else
                {
                    //donothing
                }
            }

            return message;
        }

        public Chat TogglePrivateChat(Chat chat)
        {
            if (chat.Private == true)
                chat.Private = false;
            else
                chat.Private = true;



            return chat;
        }

        public void Delete(int chatId)
        {
            if(genericRepository.GetById(chatId) != null)

            genericRepository.DeleteById(chatId);
        }

        public void ToggleVisibility(int chatId)
        {

            if (genericRepository.GetById(chatId) != null)
            {
                Chat chat = genericRepository.GetById(chatId);
                if (chat.Private == true)
                    chat.Private = false;
                else
                    chat.Private = true;


                genericRepository.Update(chat);
                genericRepository.Save();
            }

           
        }

        public IEnumerable<Chat> GetAllChats()
        {
           return genericRepository.GetAll();
        }
        public Chat GetChatById(int chatId)
        {
            return genericRepository.GetById(chatId);
        }

        public List<Message> GetAllMessagesByChat(int chatId)
        {
           return genericRepository.GetAllMessagesByChat(chatId);
        }

        public IPagedList<Message> GetAllMessagesByPaging(int page)
        {
            return genericRepository.GetAllMessagesByPaging(page);
        }

        public Message CreateMessage(MessageCreate messageCreate)
        {
            Chat chat = genericRepository.GetById(messageCreate.ChatId);

            Message message = new Message();
            message.ChatId = messageCreate.ChatId;
            message.Content = messageCreate.Content;
            message.Date = DateTime.Now.ToString();
            message.UserName = userManager.GetUserName(httpContextAccessor.HttpContext.User);
            message.Deleted = false;
            message.EmoticonLink = "";
            message.GifLink = "";
            message.Type = ConversationType.GeneralConversationType;


            if (chat.Messages == null)
            {

                chat.Messages = new List<Message>();
                chat.Messages.Add(message);
            }
            else
            {
                chat.Messages.Add(message);
            }

            message = ProfanityChecker(message);

            genericRepository.Update(chat);
            genericRepository.Save();

            return (message);
        }

        public List<Message> SearchMessagesBySearchTerm(string searchTerm)
        {
            return genericRepository.SearchMessagesBySearchTerm(searchTerm);
        }

        public IEnumerable<Chat> GetCurrentUserPrivateChats()
        {
            Group group = new Group();
            IEnumerable<Chat> chats;
            IEnumerable<Group> groups;
            User user = new User();
            //List<Chat> chats = new List<Chat>();

            chats = genericRepository.GetAll();

            user = userManager.GetUserAsync(httpContextAccessor.HttpContext.User).Result;

            groups = grouprepo.GetAll().Where(x => x.User == user);
            groups.ToList();

            chats = groups.Select(groups => groups.Chat).ToList();

            return chats;
            
        }
    }
}
