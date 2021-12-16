using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatApp2.Bussiness.Repositories;
using ChatApp2.Bussiness.Services;
using ChatApp2.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp2.API.Controllers
{
    [ApiController]
    [Route("api/Groups")]
    public class GroupController : Controller
    {

        private readonly IGroupDataService groupDataService;

        public GroupController(IGroupDataService groupDataService)
        {

            this.groupDataService = groupDataService;
        }

        [HttpPost]
        [Route("AddGroup/")]
        public IActionResult AddGroup(int chatId)
        {
            groupDataService.Add(chatId);

            return Ok("Success");
        }

        [HttpPost]
        [Route("Join/")]
        public IActionResult Invite(UserAddtoGroup userAddtoGroup)
        {
            groupDataService.Invite(userAddtoGroup);


            return Ok();
        }

        [HttpPost]
        [Route("Remove/")]
        public IActionResult RemoveUserFromGroup(UserRemoveFromGroup userRemoveFromGroup)
        {
            groupDataService.RemoveUserFromGroup(userRemoveFromGroup);
            return Ok("Success");
        }

        [HttpPost]
        [Route("Delete/")]
        public IActionResult DeleteEntireGroup(int chatId)
        {

            groupDataService.DeleteEntireGroup(chatId);

            return Ok("Success");
        }

    }
}


