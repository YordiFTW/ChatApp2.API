using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ChatApp2.Bussiness.Repositories;
using ChatApp2.Domain.Enums;
using ChatApp2.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp2.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatRepository chatRepository;

        public ChatController(IChatRepository chatRepository)
        {
            this.chatRepository = chatRepository;
        }


        [HttpPost]
        [Route("CreateChat/")]
        public IActionResult CreateNewChat(string chatname, bool hidden)
        {
            Chat chat = new Chat();

            chat.Name = chatname;

            if (hidden == false)
                chat.Private = false;
            else
                chat.Private = true;


            chat.Content = "";

            chatRepository.AddChat(chat);

            return Ok(" chat added");
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
        public IActionResult ShowAllMessagesByChat()
        {
            int chatId = 1;

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
        [Route("MessageComplex/")]
        public IActionResult PostMessageComplex(string comment, int chatId)
        {
            chatId = 5;

            Chat chat = chatRepository.GetChatbyId(chatId);

            Message commentmodel = new Message();
            commentmodel.Content = comment;
            commentmodel.Date = DateTime.Now;
            commentmodel.UserName = "Yordi";
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

            return Ok(comment);
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