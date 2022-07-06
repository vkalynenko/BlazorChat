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

            // adding users 
            var user = new IdentityUser
            {
                Id = "testUserId",
                UserName = "testUserName",
            };
            var user2 = new IdentityUser
            {
                Id = "testUserId1",
                UserName = "testUserName1",
            };
            var user3 = new IdentityUser
            {
                Id = "testUserId2",
                UserName = "testUserName2",
            };

            context.Users.AddRange(user as User, user2 as User, user3 as User);
            await context.SaveChangesAsync();

            // adding public chat
            var chat = new Chat
            {
                Id = 1,
                ChatName = "Test Chat",
                Type = ChatType.Public
            };

            context.Chats.Add(chat);
            await context.SaveChangesAsync();

            // adding chat user to public chat
            var chatUser = new ChatUser
            {
                ChatId = 1,
                UserId = "testUserId1",
            };
            context.ChatUsers.Add(chatUser);
            await context.SaveChangesAsync();

            // adding private chat
            var privateChat = new Chat()
            {
                Id = 2,
                ChatName = "Test Private",
                Type = ChatType.Private,
            };
            context.Chats.Add(privateChat);
            await context.SaveChangesAsync();

            // adding chat users to private chat
            var chatUser1 = new ChatUser
            {
                ChatId = 2,
                UserId = "testUserId",
            };
            var chatUser2 = new ChatUser
            {
                ChatId = 2,
                UserId = "testUserId1",
            };
            context.AddRange(chatUser1, chatUser2);
            await context.SaveChangesAsync();

            // adding messages to public chat
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

        public async Task CreateTestDataForInMemoryDbMessage()
        {

            var fixture = new Fixture();
            var context = new BlazorChatAppContext(_contextOptions);

            await context.Database.EnsureDeletedAsync();

            // adding users 
            var user = new IdentityUser
            {
                Id = "testUserId",
                UserName = "testUserName",
            };

            context.Users.AddRange(user as User);
            await context.SaveChangesAsync();

            // adding public chat
            var chat = new Chat
            {
                Id = 1,
                ChatName = "Test Chat",
                Type = ChatType.Public
            };

            context.Chats.Add(chat);
            await context.SaveChangesAsync();

            // adding chat user to public chat
            var chatUser = new ChatUser
            {
                ChatId = 1,
                UserId = "testUserId1",
            };
            context.ChatUsers.Add(chatUser);
            await context.SaveChangesAsync();

            // adding messages to public chat
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

        public async Task<IEnumerable<Chat>> GetTestChats()
        {
            var context = new BlazorChatAppContext(_contextOptions);
            return await context.Chats.ToListAsync();
        }

        public async Task<IEnumerable<IdentityUser>> GetTestUsers()
        {
            var context = new BlazorChatAppContext(_contextOptions);
            return await context.Users.ToListAsync();
        }
    }
}
