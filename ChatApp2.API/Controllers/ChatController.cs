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
using ChatApp2.Domain.DbContexts;

namespace ChatApp2.API.Controllers
{
    [ApiController]
    [Route("api/Messages")]
    public class ChatController : ControllerBase
    {
        private readonly IChatDataService chatDataService;
        private readonly IGroupDataService groupDataService;

        public ChatController(IChatDataService chatDataService, IGroupDataService groupDataService)
        {

            this.chatDataService = chatDataService;
            this.groupDataService = groupDataService;
        }


        [HttpPost]
        [Route("CreateChat/")]
        public IActionResult CreateNewChat(ChatCreate chatCreate)
        {
            chatDataService.Create(chatCreate);
            
            return Ok("chat added");
        }

        [HttpPost]
        [Route("DeleteChat/")]
        public IActionResult DeleteChat(int chatId)
        {

            chatDataService.Delete(chatId);

            return Ok("chat deleted");
        }

        [HttpPost]
        [Route("TogglePrivateChat/")]
        public IActionResult TogglePrivateChat(int chatId)
        {

            chatDataService.ToggleVisibility(chatId);

            return Ok("chat changed");
        }

        [HttpGet]
        [Route("AllChats/")]
        public IActionResult ShowAllChats()
        {
            chatDataService.GetAllChats();
            
            return Ok(chatDataService.GetAllChats());
        }

        [HttpPost]
        [Route("PrivateChats/")]
        public IActionResult GetPrivateChatsByCurrentUser()
        {
            chatDataService.GetCurrentUserPrivateChats();

            return Ok(chatDataService.GetCurrentUserPrivateChats());
        }

        [HttpGet]
        [Route("GetChat/")]
        public IActionResult GetChatById(int chatId)
        {
            chatDataService.GetChatById(chatId);

            return Ok(chatDataService.GetChatById(chatId));
        }

        [HttpGet]
        [Route("AllMessagesByChat/")]
        public IActionResult ShowAllMessagesByChat(int chatId)
        {           
            return Ok(chatDataService.GetAllMessagesByChat(chatId));
        }

        [HttpGet]
        [Route("AllMessagesByPaging/")]
        public IActionResult ShowAllMessagesByPaging(int page)
        {
            var paging = chatDataService.GetAllMessagesByPaging(page);
            return Ok(paging);
        }

        [HttpPost]     
        public IActionResult PostMessageComplex(MessageCreate messageCreate)
        {
            chatDataService.CreateMessage(messageCreate);

            return Ok(messageCreate.Content);
        }

        [HttpGet]
        [Route("SearchMessagesBySearchTerm/")]
        public IActionResult SearchMessagesBySearchTerm(string searchTerm)
        {

            chatDataService.SearchMessagesBySearchTerm(searchTerm);

            return Ok(chatDataService.SearchMessagesBySearchTerm(searchTerm));
        }

    }
}