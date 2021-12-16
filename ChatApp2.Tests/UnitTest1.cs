using System;
using ChatApp2.Bussiness.Repositories;
using ChatApp2.Bussiness.Services;
using ChatApp2.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Xunit;

namespace ChatApp2.Tests
{
    public class UnitTest1
    {
        private readonly IGenericRepository<Chat> genericRepository;
        private readonly IGenericRepository<Group> grouprepo;
        private readonly UserManager<User> userManager;
        private readonly HttpContextAccessor httpContextAccessor;

        public UnitTest1(IGenericRepository<Chat> genericRepository, IGenericRepository<Group> grouprepo, UserManager<User> userManager, HttpContextAccessor httpContextAccessor)
        {
            this.genericRepository = genericRepository;
            this.grouprepo = grouprepo;
            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        [Theory]
        [InlineData("this is 5h1t")]
        public void ProfanityFilterTest(string context)
        {
            
            IChatDataService chatDataService = new ChatDataService(genericRepository, grouprepo, userManager, httpContextAccessor );

            Message message = new Message();
            message.Content = context;

            message = chatDataService.ProfanityChecker(message);



            Assert.Equal("this is ****", message.Content);
        }

        [Fact]
        public void TogglePrivateTest()
        {
            IChatDataService chatDataService = new ChatDataService(genericRepository, grouprepo, userManager, httpContextAccessor);

            Chat chat = new Chat();
            chat.Private = false;
            chat = chatDataService.TogglePrivateChat(chat);



            Assert.True(chat.Private);
        }


    }
}
