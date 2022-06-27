using BlazorChatApp.BLL.Infrastructure.Services;
using BlazorChatApp.DAL.Data;
using BlazorChatApp.DAL.Data.Interfaces;
using BlazorChatApp.DAL.Data.Repositories;
using BlazorChatApp.DAL.Domain.EF;
using BlazorChatApp.DAL.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BlazorChat.Tests.IntegrationTests.Services
{
    public class IntegrationChatServiceTest : BaseIntegrationServiceTest
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<IMessageRepository> _messageRepository = new();
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly ChatService _sut;

        public IntegrationChatServiceTest() : base()
        {
            var contextOptions = new DbContextOptionsBuilder<BlazorChatAppContext>()
                .UseInMemoryDatabase("TestBlazorChatDatabase")
                .Options;
            var appContext = new BlazorChatAppContext(contextOptions);
            IChatRepository chatRepository = new ChatRepository(appContext);
            IUnitOfWork unitOfWork = new UnitOfWork(appContext, chatRepository, 
                _messageRepository.Object, _userRepository.Object);
            _sut = new ChatService(unitOfWork);
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        [Trait("Integration", "Chat")]
        public void CreateChat_ShouldReturnTrue()
        {
            // arrange
            CreateTestDataForInMemoryDb();
            var chats = GetTestChats().Result;
            var countBefore = chats.ToList().Count;

            var chatName = _fixture.Create<string>();
            var userId = "testUserId";

            // act
            var actual =_sut.CreateChat(chatName, userId).Result;
            chats = GetTestChats().Result;
            var countAfter = chats.ToList().Count;

            // assert
            actual.Should().BeTrue();
            countAfter.Should().BeGreaterThan(countBefore);
            (countBefore + 1).Should().Be(countAfter);
        }

        [Fact]
        [Trait("Integration", "Chat")]
        public void CreateChat_IfChatIsAlreadyExists_ShouldReturnFalse()
        {
            // arrange
            CreateTestDataForInMemoryDb();
            var chats = GetTestChats().Result;
            var countBefore = chats.ToList().Count;

            var chatName = "Test Chat";
            var userId = "testUserId";

            // act
            var actual = _sut.CreateChat(chatName, userId).Result;
            chats = GetTestChats().Result;
            var countAfter = chats.ToList().Count;

            // assert
            actual.Should().BeFalse();
            countBefore.Should().Be(countAfter);
        }

        [Fact]
        [Trait("Integration", "Chat")]
        public void CreatePrivateChat_ShouldReturnTrue()
        {
            // arrange
            CreateTestDataForInMemoryDb();
            var chats = GetTestChats().Result;
            var countBefore = chats.ToList().Count;

            var user1Id = "testUserId";
            var user2Id = "testUserId1";

            // act
            var actual = _sut.CreatePrivateChat(user1Id, user2Id).Result;
            chats = GetTestChats().Result;
            var countAfter = chats.ToList().Count;

            // assert
            actual.Should().BeTrue();
            countAfter.Should().BeGreaterThan(countBefore);
            (countBefore + 1).Should().Be(countAfter);
        }

        [Fact]
        [Trait("Integration", "Chat")]
        public void CreatePrivateChat_IfUserDoesNotExist_ShouldReturnTrue()
        {
            // arrange
            CreateTestDataForInMemoryDb();
            var chats = GetTestChats().Result;
            var countBefore = chats.ToList().Count;

            var user1Id = "testUserId";
            var user2Id = "fakeUserId";

            // act
            var actual = _sut.CreatePrivateChat(user1Id, user2Id).Result;
            chats =  GetTestChats().Result;
            var countAfter = chats.ToList().Count;

            // assert
            actual.Should().BeFalse();
            countBefore.Should().Be(countAfter);
        }

        [Fact]
        [Trait("Integration", "Chat")]
        public void GetAllUserChats_ShouldReturnChats()
        {
            // arrange
            CreateTestDataForInMemoryDb();
            var userId = "testUserId";

            // act
            var actual = _sut.GetAllUserChats(userId).Result;
            var chats = GetTestChats().Result.ToList();
            var enumerable = actual.ToList();

            // assert
            enumerable.Should().BeOfType<List<Chat>>();
            enumerable.Should().NotBeNull();
        }

        [Fact]
        [Trait("Integration", "Chat")]
        public void GetAllUserChats_IfUserDoesNotExist_ShouldEmptyListOfChat()
        {
            // arrange
            CreateTestDataForInMemoryDb();
            var userId = "fakeUserId";

            // act
            var actual = _sut.GetAllUserChats(userId).Result;
            var chats = GetTestChats().Result.ToList();
            var enumerable = actual.ToList();

            // assert
            enumerable.Should().BeOfType<List<Chat>>();
            enumerable.Should().NotBeNull();
            enumerable.Should().BeEmpty();
        }

        [Fact]
        [Trait("Integration", "Chat")]
        public void GetCurrentChat_ShouldReturnCurrentChat()
        {
            // arrange
            CreateTestDataForInMemoryDb();
            var chatId = 1;

            // act
            var actual = _sut.GetCurrentChat(chatId).Result;

            // assert
            actual.Should().NotBeNull();
            actual.Should().BeOfType<Chat>();
            actual.Id.Should().Be(chatId);
        }

        [Fact]
        [Trait("Integration", "Chat")]
        public void GetCurrentChat_IfChatDoesNotExist_ShouldReturnNewEmptyChat()
        {
            // arrange
            CreateTestDataForInMemoryDb();
            var chatId = 0;

            // act
            var actual = _sut.GetCurrentChat(chatId).Result;

            // assert
            actual.Should().NotBeNull();
            actual.Should().BeOfType<Chat>();
        }

        [Fact]
        [Trait("Integration", "Chat")]
        public void GetNoJoinedChats_ShouldReturnChats()
        {
            // arrange
            CreateTestDataForInMemoryDb();
            var userId = "testUserId";

            // act
            var actual = _sut.GetNotJoinedChats(userId);
            var enumerable = actual.ToList();

            // assert
            enumerable.Should().NotBeNull();
            enumerable.Should().BeOfType<List<Chat>>();
            //enumerable.All(x => x.Users.All(x => x.User.Id != userId)).Should().BeTrue();
        }

        [Fact]
        [Trait("Integration", "Chat")]
        public void GetNoJoinedChats_IfChatsDoNotExist_ShouldReturnEmptyListOfChat()
        {
            // arrange
            CreateTestDataForInMemoryDb();
            var userId = "testUserId1";

            // act
            var actual = _sut.GetNotJoinedChats(userId);
            var enumerable = actual.ToList();

            // assert
            enumerable.Should().NotBeNull();
            enumerable.Should().BeOfType<List<Chat>>();
            enumerable.Should().BeEmpty();
        }

        [Fact]
        [Trait("Integration", "Chat")]
        public void JoinRoom_ShouldReturnTrue()
        {
            // arrange
            CreateTestDataForInMemoryDb();
            var chatId = 1;
            var userId = "testUserId";

            // act
            var actual = _sut.JoinRoom(chatId, userId).Result;

            // assert
            actual.Should().BeTrue();
        }

        [Fact]
        [Trait("Integration", "Chat")]
        public void JoinRoom_IfChatDoesNotExist_ShouldReturnTrue()
        {
            // arrange
            CreateTestDataForInMemoryDb();
            var chatId = 0; 
            var userId = "testUserId";

            // act
            var actual = _sut.JoinRoom(chatId, userId).Result;

            // assert
            actual.Should().BeFalse();
        }

        [Fact]
        [Trait("Integration", "Chat")]
        public void JoinRoom_IfUserDoesNotExist_ShouldReturnTrue()
        {
            // arrange
            CreateTestDataForInMemoryDb();
            var chatId = 1; 
            var userId = "fakeUserId";

            // act
            var actual = _sut.JoinRoom(chatId, userId).Result;

            // assert
            actual.Should().BeFalse();
        }

        [Fact]
        [Trait("Integration", "Chat")]
        public void GetPrivateChat_ShouldReturnPrivateChat()
        {
            // arrange 
            CreateTestDataForInMemoryDb();

            var user1Id = "testUserId";
            var user2Id = "testUserId1";

            // act
            var actual = _sut.GetPrivateChat(user1Id, user2Id).Result;

            // assert
            actual.Should().NotBeNull();
            actual.Should().BeOfType<Chat>();
            actual.Type.Should().Be(ChatType.Private);
            actual.Users.Should().HaveCount(2);
        }

        [Fact]
        [Trait("Integration", "Chat")]
        public void FindPrivateChat_ShouldReturnIdOfPrivateChat()
        {
            // arrange
            CreateTestDataForInMemoryDb();

            var user1Id = "testUserId";
            var user2Id = "testUserId1";

            // act
            var actual = _sut.FindPrivateChat(user1Id, user2Id).Result;

            // assert
            actual.Should().NotBe(0);
        }

        [Fact]
        [Trait("Integration", "Chat")]
        public void FindPrivateChat_IfChatDoesNotExist_ShouldReturnIdOfPrivateChat()
        {
            // arrange
            CreateTestDataForInMemoryDb();

            var user1Id = "testUserId";
            var user2Id = "testUserId2"; // new user

            // act
            var actual = _sut.FindPrivateChat(user1Id, user2Id).Result;

            // assert
            actual.Should().NotBe(0);
        }
    }
}
