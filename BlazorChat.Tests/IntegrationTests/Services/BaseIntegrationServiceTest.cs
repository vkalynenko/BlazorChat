using BlazorChatApp.DAL.Data.Repositories;
using BlazorChatApp.DAL.Domain.EF;
using BlazorChatApp.DAL.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazorChat.Tests.IntegrationTests.Services
{
    public class BaseIntegrationServiceTest
    {
        private readonly DbContextOptions<BlazorChatAppContext> _contextOptions;

        public BaseIntegrationServiceTest()
        {
            _contextOptions = new DbContextOptionsBuilder<BlazorChatAppContext>()
                .UseInMemoryDatabase("TestBlazorChatDatabase")
                .Options;
        }

        public async Task CreateTestDataForInMemoryDb()
        {

            var fixture = new Fixture();
            var context = new BlazorChatAppContext(_contextOptions);

            await context.Database.EnsureDeletedAsync();

            var chat = new Chat
            {
                Id = 1,
                ChatName = "Test Chat",
                Type = ChatType.Public,
            };

            context.Chats.Add(chat);
            await context.SaveChangesAsync();
            var user = new IdentityUser
            {
                Id = "testUserId",
                UserName = "testUserName",
            };

            context.Users.Add(user);

            for (var i = 1; i <= 50; i++)
            {
                var message = new Message
                {
                    Id = i,
                    MessageText = fixture.Create<string>(),
                    SenderName = "testUserName",
                    UserId = "testUserId",
                    ChatId = 1,
                    IsItReply = false,

                };
                context.Messages.Add(message);
                await context.SaveChangesAsync();
            }

            await context.SaveChangesAsync();
        }


        public async Task<IEnumerable<Message>> GetTestMessages()
        {
            var context = new BlazorChatAppContext(_contextOptions);
            return await context.Messages.ToListAsync();
        }
    }
}
