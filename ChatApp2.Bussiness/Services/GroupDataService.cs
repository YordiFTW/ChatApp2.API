using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ChatApp2.Bussiness.Repositories;
using ChatApp2.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace ChatApp2.Bussiness.Services
{
  

   public class GroupDataService : IGroupDataService
    {
        private readonly IGenericRepository<Group> genericRepository;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly IGenericRepository<Chat> chatrepo;
        private readonly HttpContextAccessor httpContextAccessor;

        public GroupDataService(IGenericRepository<Group> genericRepository, SignInManager<User> signInManager, UserManager<User> userManager, IGenericRepository<Chat> chatrepo, HttpContextAccessor httpContextAccessor)
        {
            this.genericRepository = genericRepository;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.chatrepo = chatrepo;
            this.httpContextAccessor = httpContextAccessor;
        }

        public Group Add(int chatId)
        {
            Group group = new Group();
            group.Chat = chatrepo.GetById(chatId);
            string userId = signInManager.Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            group.User = userManager.FindByIdAsync(userId).Result;

            genericRepository.Insert(group);


            return (group);
        }

        public void DeleteEntireGroup(int chatId)
        {
            genericRepository.DeleteAllByChatId(chatId);
            genericRepository.Save();
        }

        public void Invite(UserAddtoGroup userAddtoGroup)
        {
            if (userAddtoGroup.isAccepted == true)
            {
                Group group = new Group();
                group.Chat = chatrepo.GetById(userAddtoGroup.chatId);
                group.User = userManager.FindByNameAsync(userAddtoGroup.userName).Result;

                genericRepository.Insert(group);
                genericRepository.Save();
            }

            else
            {
                //ignore action + change frontend UI
            }
        }

        public void RemoveUserFromGroup(UserRemoveFromGroup userRemoveFromGroup)
        {
            Group group = new Group();
            group.User = userManager.FindByNameAsync(userRemoveFromGroup.userName).Result;
            group.Chat = chatrepo.GetById(userRemoveFromGroup.chatId);

            genericRepository.DeleteByObj(group);
            genericRepository.Save();
        }
    }
}
