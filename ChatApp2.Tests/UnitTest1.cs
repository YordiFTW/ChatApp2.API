using System;
using ChatApp2.Bussiness.Services;
using ChatApp2.Domain.Models;
using Xunit;

namespace ChatApp2.Tests
{
    public class UnitTest1
    {
        [Theory]
        [InlineData("this is 5h1t")]
        public void ProfanityFilterTest(string context)
        {
            IChatDataService chatDataService = new ChatDataService();

            Message message = new Message();
            message.Content = context;

            message = chatDataService.ProfanityChecker(message);



            Assert.Equal("this is ****", message.Content);
        }

        [Fact]
        public void TogglePrivateTest()
        {
            IChatDataService chatDataService = new ChatDataService();

            Chat chat = new Chat();
            chat.Private = false;
            chat = chatDataService.TogglePrivateChat(chat);



            Assert.True(chat.Private);
        }


    }
}
