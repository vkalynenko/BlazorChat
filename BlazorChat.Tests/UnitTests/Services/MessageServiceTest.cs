using BlazorChatApp.BLL.Infrastructure.Services;
using BlazorChatApp.DAL.CustomExceptions;
using BlazorChatApp.DAL.Data.Interfaces;
using BlazorChatApp.DAL.Domain.Entities;
using BlazorChatApp.DAL.Models;

namespace BlazorChat.Tests.UnitTests.Services
{
    public class MessageServiceTest
    {
        private readonly Fixture _fixture = new();
        private readonly MessageService _sut;
        private readonly Mock<IUnitOfWork> _mock = new();

        public MessageServiceTest()
        {
            _sut = new MessageService(_mock.Object);
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        [Trait("Unit", "Message")]
        public async Task CreateMessage_ShouldReturnTrue()
        {
            // arrange 
            var chatId = _fixture.Create<int>();
            var message = _fixture.Create<string>();
            var senderName = _fixture.Create<string>();
            var userId = _fixture.Create<string>();

            _mock.Setup(unit => unit.Message.CreateMessage(chatId, message, senderName, userId));

            // act
            var actual = await _sut.CreateMessage(chatId, message, senderName, userId);

            //assert
            actual.Should().BeTrue();
            _mock.Verify(unit=>unit.SaveChangesAsync(), Times.Once);
            _mock.Verify(unit=>unit.Message.CreateMessage(chatId, message, senderName, userId));
        }

        [Fact]
        [Trait("Unit", "Message")]
        public async Task СreateMessage_IfIncorrectChatId_ShouldReturnFalse()
        {
            // arrange
            var chatId = 0;
            var message = _fixture.Create<string>();
            var senderName = _fixture.Create<string>();
            var userId = _fixture.Create<string>();

            _mock.Setup(unit => unit.Message.CreateMessage(chatId, message, senderName, userId))
                .Throws<ChatDoesNotExistException>();
            // act
            var actual = await _sut.CreateMessage(chatId, message, senderName, userId);

            //assert
            actual.Should().BeFalse();
            _mock.Verify(unit=>unit.Message.CreateMessage(chatId, message, senderName, userId), Times.Once);
            _mock.Verify(unit => unit.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        [Trait("Unit", "Message")]
        public async Task DeleteMessage_ShouldReturnTrue()
        {
            // arrange
            var messageId = _fixture.Create<int>();

            _mock.Setup(unit => unit.Message.DeleteMessageFromAll(messageId));

            // act
            var actual = await _sut.DeleteMessageFromAll(messageId);

            // assert
            _mock.Verify(unit => unit.Message.DeleteMessageFromAll(messageId), Times.Once);
            _mock.Verify(unit=>unit.SaveChangesAsync(), Times.Once);
            actual.Should().BeTrue();
        }

        [Fact]
        [Trait("Unit", "Message")]
        public async Task DeleteMessage_IfIncorrectMessageId_ShouldReturnFalse()
        {
            //arrange
            var messageId = 0;

            _mock.Setup(unit => unit.Message.DeleteMessageFromAll(messageId))
                .Throws<MessageDoesNotExistException>();
            // act
            var actual = await _sut.DeleteMessageFromAll(messageId);
            //assert
            _mock.Verify(unit=>unit.Message.DeleteMessageFromAll(messageId), Times.Once);
            _mock.Verify(unit=>unit.SaveChangesAsync(), Times.Never);
            actual.Should().BeFalse();
        }

        [Fact]
        [Trait("Unit", "Message")]
        public async Task EditMessage_ShouldReturnMessage()
        {
            // arrange
            var messageId = _fixture.Create<int>();
            var newText = _fixture.Create<string>();
            var userId = _fixture.Create<string>();
            var message = new Message
            {
                Id = messageId,
                MessageText = newText,
                UserId = userId,
            };

            _mock.Setup(unit => unit.Message.UpdateMessage(messageId, newText, userId));
            _mock.Setup(unit => unit.Message.GetById(messageId))
                .ReturnsAsync(message);
            //act
            var actual = await _sut.EditMessage(messageId, newText, userId);

            //assert
            actual.Should().NotBeNull();
            actual.Should().BeOfType<Message>();
            _mock.Verify(unit=>unit.Message.UpdateMessage(messageId, newText, userId), Times.Once);
            _mock.Verify(unit => unit.SaveChangesAsync(), Times.Once);
            actual.Id.Should().Be(messageId);
            actual.MessageText.Should().Be(newText);
            actual.UserId.Should().Be(userId);
        }

        [Fact]
        [Trait("Unit", "Message")]
        public async Task EditMessage_IfIncorrectMessageId_ShouldReturnNewMessage()
        {
            // arrange 
            var messageId = 0;
            var newText = _fixture.Create<string>();
            var userId = _fixture.Create<string>();

            _mock.Setup(unit => unit.Message.UpdateMessage(messageId, newText, userId));
            _mock.Setup(unit => unit.Message.GetById(messageId))
                .Throws<MessageDoesNotExistException>();
            // act
            var actual = await _sut.EditMessage(messageId, newText, userId);
            // assert
            _mock.Verify(unit => unit.Message.UpdateMessage(messageId, newText, userId), Times.Once);
            actual.MessageText.Should().BeNullOrEmpty();
            actual.Should().BeOfType<Message>();
        }

        [Fact]
        [Trait("Unit", "Message")]
        public async Task ReplyToGroup_ShouldReturnMessage()
        {
            // arrange
            var replyModel = _fixture.Build<ReplyToGroupModel>()
                .Create();
            var message = new Message
            {
                MessageText = $"Replied to {replyModel.UserName}:{replyModel.Message} - {replyModel.Reply}"
            };

            _mock.Setup(unit => unit.Message.ReplyToGroup(replyModel)).ReturnsAsync(message);

            // act
            var actual = await _sut.ReplyToGroup(replyModel);

            //assert 
            actual.Should().BeOfType<Message>();
            actual.Should().NotBeNull();
            _mock.Verify(unit => unit.SaveChangesAsync(), Times.Once);
            _mock.Verify(unit=>unit.Message.ReplyToGroup(replyModel), Times.Once);
            actual.MessageText.Should()
                .Be($"Replied to {replyModel.UserName}:{replyModel.Message} - {replyModel.Reply}");
        }

        [Fact]
        [Trait("Unit", "Message")]
        public async Task ReplyToUser_ShouldReturnMessage()
        {
            // arrange 
            var replyToUserModel = _fixture.Build<ReplyToUserModel>().Create();
            var message = new Message
            {
                MessageText =
                    $"Replied to {replyToUserModel.UserName}:{replyToUserModel.Message} - {replyToUserModel.Reply}",
            };

            _mock.Setup(unit => unit.Message.ReplyToUser(replyToUserModel)).ReturnsAsync(message);

            // act
            var actual = await _sut.ReplyToUser(replyToUserModel);

            // assert
            actual.Should().BeOfType<Message>();
            actual.Should().NotBeNull();
            _mock.Verify(unit=>unit.SaveChangesAsync(), Times.Once);
            _mock.Verify(unit => unit.Message.ReplyToUser(replyToUserModel), Times.Once);
            actual.MessageText.Should()
                .Be($"Replied to {replyToUserModel.UserName}:{replyToUserModel.Message} - {replyToUserModel.Reply}");
        }

        [Theory]
        [InlineData(0, 10)]
        [InlineData(10, 10)]
        [InlineData(20, 40)]
        [Trait("Unit", "Message")]
        public async Task GetMessages_ShouldReturnMessages(int toSkip,int toLoad)
        {
            // arrange
            var test = _fixture.Build<Message>()
                .Create();

            _mock.Setup(unit => unit.Message.GetMessages(test.ChatId, toSkip, toLoad))
                .ReturnsAsync(Messages(test, toSkip, toLoad));
            // act
            var actual = (await _sut.GetMessages(test.ChatId, toSkip, toLoad)).ToList();
            
            // assert
            actual.Should().NotBeNull();
            actual.Count.Should().BeLessThanOrEqualTo(toLoad);
            actual.Should().BeOfType<List<Message>>();
            actual[0].Id.Should().Be(toSkip + 1);
            actual.Count.Should().BeGreaterOrEqualTo(toSkip);
            _mock.Verify(unit=>unit.Message.GetMessages(test.ChatId, toSkip, toLoad));
        }

        private static IEnumerable<Message> Messages(Message test, int toSkip, int toTake)
        {
            var messages = new List<Message>();
            for (int i = 0; i < 100; i++)
            {
                var message = new Message()
                {
                    Id = i + 1,
                    ChatId = test.ChatId,
                    MessageText = test.MessageText,
                    SentTime = test.SentTime,
                    UserId = test.UserId,
                };
                messages.Add(message);
            }
            var result = messages.Skip(toSkip).Take(toTake);
            return result;
        }
    }
}
