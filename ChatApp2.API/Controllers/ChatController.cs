using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp2.Bussiness.Repositories;
using ChatApp2.Domain.Enums;
using ChatApp2.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using ChatApp2.Bussiness.Services;

namespace ChatApp2.API.Controllers
{
    [ApiController]
    [Route("api/Messages")]
    public class ChatController : ControllerBase
    {
        private readonly IChatRepository chatRepository;
        private IGenericRepository<Chat> cgrepository = null;
        private IGenericRepository<Group> ggrepository = null;
        private readonly SignInManager<User> signInManager;
        private readonly IHttpContextAccessor _context;
        private readonly UserManager<User> userManager;
        private readonly HttpContextAccessor httpContextAccessor;
        private readonly ChatDataService chatDataService;

        public ChatController(IChatRepository chatRepository, IGenericRepository<Chat> cgrepository, IGenericRepository<Group> ggrepository, SignInManager<User> signInManager,
            IHttpContextAccessor context, UserManager<User> userManager, HttpContextAccessor httpContextAccessor, ChatDataService chatDataService)
        {
            this.chatRepository = chatRepository;
            this.cgrepository = cgrepository;
            this.ggrepository = ggrepository;
            this.signInManager = signInManager;
            _context = context;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
            this.chatDataService = chatDataService;
        }


        [HttpPost]
        [Route("CreateChat/")]
        public IActionResult CreateNewChat(ChatCreate chatCreate)
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


            chatRepository.AddChat(chat);
            

            return Ok(" chat added");
        }

        

        [HttpGet]
        [Route("Test/Generic/")]
        public IActionResult TestGeneric()
        {
            return Ok(cgrepository.GetAll());
        }

        [HttpGet]
        [Route("Test/User/")]
        public IActionResult TestUser()
        {
            return Ok();
        }



        [HttpPost]
        [Route("DeleteChat/")]
        public IActionResult DeleteChat(string chatname)
        {

            Chat chat = chatRepository.GetChatbyName(chatname);

            chatRepository.DeleteChat(chat.Id);

            return Ok(" chat deleted");
        }

        [HttpPost]
        [Route("TogglePrivateChat/")]
        public IActionResult TogglePrivateChat(int chatId)
        {

            Chat chat = chatRepository.GetChatbyId(chatId);

            chatRepository.TogglePrivateChat(chat);

            return Ok(" chat changed");
        }

        [HttpGet]
        [Route("AllChats/")]
        public IActionResult ShowAllChats()
        {


            chatRepository.GetAllChats();

            return Ok(chatRepository.GetAllChats());
        }

        [HttpGet]
        [Route("AllMessagesByChat/")]
        public IActionResult ShowAllMessagesByChat(int chatId)
        {
            

            return Ok(chatRepository.GetAllMessagesByChat(chatId));
        }

        [HttpGet]
        [Route("AllMessagesByPaging/")]
        public IActionResult ShowAllMessagesByPaging()
        {
            var test = chatRepository.GetAllMessagesByPaging();
            return Ok(test);
        }

        [HttpPost]
        [Route("MessageSimple/")]
        public IActionResult PostMessageSimple(string comment)
        {


            Chat chat = chatRepository.GetChatbyId(1);

            chat.Content += Environment.NewLine + DateTime.Now.ToString() + " " + comment;

            chatRepository.UpdateChat(chat);

            return Ok(chat.Content);
        }

        [HttpPost]
        
        public IActionResult PostMessageComplex(MessageCreate messageCreate)
        {
            

            Chat chat = chatRepository.GetChatbyId(messageCreate.ChatId);

            Message commentmodel = new Message();
            commentmodel.ChatId = messageCreate.ChatId;
            commentmodel.Content = messageCreate.Content;
            commentmodel.Date = DateTime.Now.ToString();
            commentmodel.UserName = userManager.GetUserName(User);
            commentmodel.Deleted = false;
            commentmodel.EmoticonLink = "Yordi";
            commentmodel.GifLink = "Yordi";
            commentmodel.Type = ConversationType.GeneralConversationType;


            if (chat.Messages == null)
            {

                chat.Messages = new List<Message>();
                chat.Messages.Add(commentmodel);
            }
            else
            {
                chat.Messages.Add(commentmodel);
            }





            chatRepository.UpdateChat(chat);

            return Ok(messageCreate.Content);
        }

        [HttpGet]
        [Route("SearchMessagesBySearchTerm/")]
        public IActionResult SearchMessagesBySearchTerm(string searchTerm)
        {

            chatRepository.SearchMessagesBySearchTerm(searchTerm);

            return Ok(chatRepository.SearchMessagesBySearchTerm(searchTerm));
        }

    }
}