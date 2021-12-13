using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ChatApp2.Bussiness.Repositories;
using ChatApp2.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ChatApp2.API.Controllers
{
    [ApiController]
    [Route("api/Groups")]
    public class GroupController : Controller
    {
        private readonly IChatRepository chatRepository;
        private IGenericRepository<Group> repository = null;
        private readonly IGenericRepository<Group> ggrepository;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IGenericRepository<Chat> cgrepository;

        public GroupController(IChatRepository chatRepository, IGenericRepository<Group> repository, IGenericRepository<Group> ggrepository, SignInManager<User> signInManager
            , UserManager<User> userManager, IGenericRepository<Chat> cgrepository)
        {
            this.chatRepository = chatRepository;
            this.repository = repository;
            this.ggrepository = ggrepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.cgrepository = cgrepository;
        }

        [HttpPost]
        [Route("AddGroup/")]
        public IActionResult AddGroup(int chatId)
        {
            Group group = new Group();
            group.Chat = cgrepository.GetById(chatId);
            string userId = signInManager.Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            group.User = userManager.FindByIdAsync(userId).Result;

            ggrepository.Insert(group);
            ggrepository.Save();

            return Ok("Success");
        }

        [HttpPost]
        [Route("Join/")]
        public IActionResult Invite(int chatId, string userName, bool isAccepted)
        {
            if (isAccepted == true)
            {


                Group group = new Group();


                group.Chat = cgrepository.GetById(chatId);

                group.User = userManager.FindByNameAsync(userName).Result;

                ggrepository.Insert(group);
                ggrepository.Save();

                return Ok("Success");
            }

            else
            {
                //ignore action + change frontend UI
            }


            return Ok();
        }

        [HttpPost]
        [Route("Remove/")]
        public IActionResult RemoveUserFromGroup(int chatId, string userName)
        {
            Group group = new Group();
            group.User = userManager.FindByNameAsync(userName).Result;
            group.Chat = cgrepository.GetById(chatId);

            ggrepository.DeleteByObj(group);
            ggrepository.Save();
            return Ok("Success");
        }

        [HttpPost]
        [Route("Delete/")]
        public IActionResult DeleteEntireGroup(int chatId)
        {

            ggrepository.DeleteAllByChatId(chatId);
            ggrepository.Save();

            return Ok("Success");
        }

    }
}


