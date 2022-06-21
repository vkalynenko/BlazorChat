using BlazorChatApp.BLL.Infrastructure.Services;
using BlazorChatApp.DAL.CustomExceptions;
using BlazorChatApp.DAL.Data.Interfaces;

namespace BlazorChat.Tests.Services
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
            _mock.Verify(unit=>unit.SaveChangesAsync(), Times.Once);
            _mock.Verify(unit=>unit.Chat.CreateChat(chatName, userId), Times.Once);
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
    }
}
