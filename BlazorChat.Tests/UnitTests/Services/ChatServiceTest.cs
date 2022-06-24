using BlazorChatApp.BLL.Infrastructure.Services;
using BlazorChatApp.DAL.CustomExceptions;
using BlazorChatApp.DAL.Data.Interfaces;
using BlazorChatApp.DAL.Domain.Entities;

namespace BlazorChat.Tests.UnitTests.Services
{
    public class ChatServiceTest
    {
        private readonly Fixture _fixture = new();
        private readonly ChatService _sut;
        private readonly Mock<IUnitOfWork> _mock = new();

        public ChatServiceTest()
        {
            _sut = new ChatService(_mock.Object);
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        [Trait("Chat", "CRUD")]
        public async Task CreatePublicChat_ShouldReturnTrue()
        {
            // arrange 
            var chatName = _fixture.Create<string>();
            var userId = _fixture.Create<string>();

            _mock.Setup(unit => unit.Chat.CreateChat(chatName, userId));

            // act
            var actual = await _sut.CreateChat(chatName, userId);

            // assert
            actual.Should().BeTrue();
            _mock.Verify(unit => unit.SaveChangesAsync(), Times.Once);
            _mock.Verify(unit => unit.Chat.CreateChat(chatName, userId), Times.Once);
        }

        [Fact]
        [Trait("Chat", "CRUD")]
        public async Task CreatePublicChat_IfChatAlreadyExists_ShouldReturnFalse()
        {
            // arrange 
            var chatName = _fixture.Create<string>();
            var userId = _fixture.Create<string>();

            _mock.Setup(unit => unit.Chat.CreateChat(chatName, userId))
                .Throws<ChatIsAlreadyExistsException>();

            // act
            var actual = await _sut.CreateChat(chatName, userId);

            // assert
            actual.Should().BeFalse();
            _mock.Verify(unit => unit.SaveChangesAsync(), Times.Never);
            _mock.Verify(unit => unit.Chat.CreateChat(chatName, userId), Times.Once);
        }

        [Fact]
        [Trait("Chat ", "CRUD")]
        public async Task CreatePrivateChat_ShouldReturnChatId()
        {
            // arrange
            var user1Id = _fixture.Create<string>();
            var user2Id = _fixture.Create<string>();
            var id = _fixture.Create<int>();

            _mock.Setup(unit => unit.Chat.CreatePrivateChat(user1Id, user2Id))
                .ReturnsAsync(id);

            // act
            var actual = await _sut.CreatePrivateChat(user1Id, user2Id);

            // assert
            actual.Should().BeTrue();
            _mock.Verify(unit => unit.Chat.CreatePrivateChat(user1Id, user2Id), Times.Once);
            _mock.Verify(unit => unit.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        [Trait("Chat ", "CRUD")]
        public async Task CreatePrivateChat_IfUserDoesNotExist_ShouldReturnTrue()
        {
            // arrange
            var user1Id = _fixture.Create<string>();
            var user2Id = _fixture.Create<string>();
            _mock.Setup(unit => unit.Chat.CreatePrivateChat(user1Id, user2Id))
                .Throws<UserDoesNotExistException>();

            // act
            var actual = await _sut.CreatePrivateChat(user1Id, user2Id);
            actual.Should().BeFalse();
            _mock.Verify(unit => unit.Chat.CreatePrivateChat(user1Id, user2Id), Times.Once);
            _mock.Verify(unit => unit.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        [Trait("Chat ", "CRUD")]
        public async Task GetAllUsersChats_ShouldReturnChats()
        {
            // arrange 
            var userId = _fixture.Create<string>();
            var chat = _fixture.Build<Chat>()
                .Without(c => c.Users)
                .Without(c => c.Messages)
                .Create();

            _mock.Setup(unit => unit.Chat.GetAllUserChats(userId))
                .ReturnsAsync(GetChats(chat));

            // act
            var actual = await _sut.GetAllUserChats(userId);
            var enumerable = actual.ToList();

            // assert
            enumerable.Should().NotBeNull();
            enumerable.Should().BeOfType<List<Chat>>();
            enumerable.Count().Should().Be(GetChats(chat).Count());
            enumerable[0].Id.Should().Be(chat.Id);
            _mock.Verify(unit => unit.Chat.GetAllUserChats(userId), Times.Once);

        }

        [Fact]
        [Trait("Chat ", "CRUD")]
        public async Task GetAllUsersChats_IfUserDoesNorExist_ShouldReturnNewList()
        {
            // arrange 
            var userId = _fixture.Create<string>();

            _mock.Setup(unit => unit.Chat.GetAllUserChats(userId))
                .Throws<UserDoesNotExistException>();

            // act 
            var actual = await _sut.GetAllUserChats(userId);

            // assert
            actual.Should().BeEmpty();
            actual.Should().NotBeNull();
            actual.Should().BeOfType<List<Chat>>();
            _mock.Verify(unit => unit.Chat.GetAllUserChats(userId), Times.Once);
        }

        [Fact]
        [Trait("Chat ", "CRUD")]
        public async Task GetCurrentChat_ShouldReturnChat()
        {
            // arrange
            var chatId = _fixture.Create<int>();
            var chat = _fixture.Build<Chat>()
                .With(c => c.Id, chatId)
                .Create();

            _mock.Setup(unit => unit.Chat.GetChat(chatId)).ReturnsAsync(chat);

            // act
            var action = await _sut.GetCurrentChat(chatId);

            // assert
            action.Should().NotBeNull();
            action.Should().BeOfType<Chat>();
            action.Id.Should().Be(chatId);
            _mock.Verify(unit=>unit.Chat.GetChat(chatId), Times.Once);
        }

        [Fact]
        [Trait("Chat ", "CRUD")]
        public async Task GetCurrentChat_IfChatDoesNotExist_ShouldReturnNewChat()
        {
            // arrange
            var chatId = _fixture.Create<int>();

            _mock.Setup(unit => unit.Chat.GetChat(chatId)).Throws<ChatDoesNotExistException>();

            // act
            var actual = await _sut.GetCurrentChat(chatId);

            // assert
            actual.Should().BeOfType<Chat>();
            actual.Should().NotBeNull();
        }

        [Fact]
        [Trait("Chat ", "CRUD")]
        public void GetNotJoinedChats_ShouldReturnChats()
        {
            // arrange 
            var userId = _fixture.Create<string>();
            var chat = _fixture.Build<Chat>().Create();

            _mock.Setup(unit => unit.Chat.GetNotJoinedChats(userId)).Returns(GetChats(chat));

            // act
            var actual = _sut.GetNotJoinedChats(userId);

            // assert
            var enumerable = actual.ToList();
            enumerable.Should().BeOfType<List<Chat>>();
            enumerable.Should().NotBeNull();
            enumerable[0].Id.Should().Be(chat.Id);
            _mock.Verify(unit=>unit.Chat.GetNotJoinedChats(userId), Times.Once);
        }

        [Fact]
        [Trait("Chat ", "CRUD")]
        public void GetNotJoinedChats_IfSuchChatsDoNotExist_ShouldReturnNewList()
        {
            // arrange 
            var userId = _fixture.Create<string>();

            _mock.Setup(unit => unit.Chat.GetNotJoinedChats(userId))
                .Throws<ChatsDoNotExistException>();

            // act
            var actual = _sut.GetNotJoinedChats(userId).ToList();

            // assert
            actual.Should().BeEmpty();
            actual.Should().NotBeNull();
            actual.Should().BeOfType<List<Chat>>();
            _mock.Verify(unit=>unit.Chat.GetNotJoinedChats(userId), Times.Once);
        }

        [Fact]
        [Trait("Chat ", "Interaction")]
        public async Task JoinChat_ShouldReturnTrue()
        {
            // arrange 
            var chatId = _fixture.Create<int>();
            var userId = _fixture.Create<string>();

            _mock.Setup(unit => unit.Chat.JoinRoom(chatId, userId));

            // act
            var actual = await _sut.JoinRoom(chatId, userId);

            // assert
            actual.Should().BeTrue();
            _mock.Verify(unit=>unit.Chat.JoinRoom(chatId, userId), Times.Once);
            _mock.Verify(unit=>unit.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        [Trait("Chat ", "Interaction")]
        public async Task JoinChat_IfUserDoesNotExist_ShouldReturnFalse()
        {
            // arrange
            var chatId = _fixture.Create<int>();
            var userId = _fixture.Create<string>();

            _mock.Setup(unit => unit.Chat.JoinRoom(chatId, userId))
                .Throws<UserDoesNotExistException>();

            // act
            var actual = await _sut.JoinRoom(chatId, userId);

            // assert
            actual.Should().BeFalse();
            _mock.Verify(unit => unit.Chat.JoinRoom(chatId, userId), Times.Once);
            _mock.Verify(unit => unit.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        [Trait("Chat ", "Interaction")]
        public async Task JoinChat_IfUChatDoesNotExist_ShouldReturnFalse()
        {
            // arrange
            var chatId = _fixture.Create<int>();
            var userId = _fixture.Create<string>();

            _mock.Setup(unit => unit.Chat.JoinRoom(chatId, userId))
                .Throws<ChatDoesNotExistException>();

            // act
            var actual = await _sut.JoinRoom(chatId, userId);

            // assert
            actual.Should().BeFalse();
            _mock.Verify(unit => unit.Chat.JoinRoom(chatId, userId), Times.Once);
            _mock.Verify(unit => unit.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        [Trait("Chat ", "CRUD")]
        public async Task GetPrivateChat_ShouldReturnPrivateChat()
        {
            // arrange 
            var user1Id = _fixture.Create<string>();
            var user2Id = _fixture.Create<string>();

            var user1 = new ChatUser
            {
                Id = _fixture.Create<int>(),
                UserId = user1Id
            };
            var user2 = new ChatUser
            {
                Id = _fixture.Create<int>(),
                UserId = user2Id
            };

            Chat chat = _fixture.Build<Chat>()
                .Without(x=>x.Users)
                .Do(x=>x.Users.Add(user1))
                .Do(x=>x.Users.Add(user2))
                .Create();

            _mock.Setup(unit => unit.Chat.GetPrivateChat(user1Id, user2Id))
                .ReturnsAsync(chat);

            // act
            var actual = await _sut.GetPrivateChat(user1Id, user2Id);

            // assert
            actual.Users.Should().Contain(user1);
            actual.Users.Should().Contain(user2);
            actual.Should().BeOfType<Chat>();
            _mock.Verify(unit=>unit.Chat.GetPrivateChat(user1Id, user2Id));
        }

        [Fact]
        [Trait("Chat ", "Interaction")] 
        public async Task FindPrivateChat_ShouldReturnChatId()
        {
            // arrange
            var user1Id = _fixture.Create<string>();
            var user2Id = _fixture.Create<string>();

            int chatId = _fixture.Create<int>();

            _mock.Setup(unit => unit.Chat.FindPrivateChat(user1Id, user2Id))
                .ReturnsAsync(chatId);

            // act 
            var actual = await _sut.FindPrivateChat(user1Id, user2Id);

            // assert
            actual.Should().Be(chatId);
            _mock.Verify(unit=>unit.Chat.FindPrivateChat(user1Id, user2Id));
            _mock.Verify(unit=>unit.Chat.CreateNewPrivateChat(user1Id, user2Id), Times.Never);
            _mock.Verify(unit=>unit.SaveChangesAsync(), Times.Never);
            _mock.Verify(unit=>unit.Chat.GetChatIdByName(It.IsAny<string>()), Times.Never);
        }

        [Fact]
        [Trait("Chat ", "Interaction")]
        public async Task FindPrivateChat_IfChatDoesNotExist_ShouldReturnChatId()
        {
            // arrange
            var user1Id = _fixture.Create<string>();
            var user2Id = _fixture.Create<string>();

            int chatId = 0;
            var chatName = _fixture.Create<string>();
            int newChatId = _fixture.Create<int>();

            _mock.Setup(unit => unit.Chat.FindPrivateChat(user1Id, user2Id))
                .ReturnsAsync(chatId);

            _mock.Setup(unit => unit.Chat.CreateNewPrivateChat(user1Id, user2Id))
                .ReturnsAsync(chatName);

            _mock.Setup(unit => unit.Chat.GetChatIdByName(chatName))
                .ReturnsAsync(newChatId);
            // act 
            var actual = await _sut.FindPrivateChat(user1Id, user2Id);

            // assert
            actual.Should().Be(newChatId);
            _mock.Verify(unit => unit.Chat.FindPrivateChat(user1Id, user2Id));
            _mock.Verify(unit => unit.Chat.CreateNewPrivateChat(user1Id, user2Id), Times.Once);
            _mock.Verify(unit => unit.SaveChangesAsync(), Times.Once);
            _mock.Verify(unit => unit.Chat.GetChatIdByName(It.IsAny<string>()), Times.Once);
        }

        private IEnumerable<Chat> GetChats(Chat chat)
        {
            var chats = new List<Chat>();
            for (int i = 0; i < 10; i++)
            {
                chats.Add(chat);
            }

            return chats;
        }
    }
}
