using BlazorChatApp.BLL.Infrastructure.Services;
using BlazorChatApp.DAL.Data;
using BlazorChatApp.DAL.Data.Interfaces;
using BlazorChatApp.DAL.Data.Repositories;
using BlazorChatApp.DAL.Domain.EF;
using BlazorChatApp.DAL.Domain.Entities;
using BlazorChatApp.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorChat.Tests.IntegrationTests.Services
{
    [TestCaseOrderer("BlazorChatApp.BlazorChat.Tests.PriorityOrderer", "BlazorChat.Tests")]
    public class IntegrationMessageServiceTest : BaseIntegrationServiceTest
    {
        private readonly Fixture _fixture = new();
        private readonly DbContextOptions<BlazorChatAppContext> _contextOptions;
        private readonly Mock<IChatRepository> _chatRepository = new();
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly BlazorChatAppContext _appContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly MessageService _sut;

        public IntegrationMessageServiceTest() : base()
        {
            _contextOptions = new DbContextOptionsBuilder<BlazorChatAppContext>()
                .UseInMemoryDatabase("TestBlazorChatDatabase")
                .Options;
            _appContext = new BlazorChatAppContext(_contextOptions);
            IMessageRepository messageRepository = new MessageRepository(_appContext);
            _unitOfWork = new UnitOfWork(_appContext, _chatRepository.Object, messageRepository,
                _userRepository.Object);
            _sut = new MessageService(_unitOfWork);
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact, TestPriority(1)]
        [Trait("Integration", "Message")]
        public void CreateMessage_ShouldReturnTrue()
        {
            // arrange
            CreateTestDataForInMemoryDb();
            var messages = GetTestMessages().Result;
            var countBefore = messages.ToList().Count;

            var chatId = 1;
            var text = _fixture.Create<string>();
            var userId = "testUserId";
            var userName = "textUsername";


            // act 
            var actual = _sut.CreateMessage(chatId, text, userName, userId).Result;
            messages = GetTestMessages().Result;
            var countAfter = messages.ToList().Count;

            // assert
            actual.Should().BeTrue();
            countAfter.Should().BeGreaterThan(countBefore);
            (countBefore + 1).Should().Be(countAfter);
        }

        [Fact, TestPriority(2)]
        [Trait("Integration", "Message")]
        public void CreateMessage_IfChatDoesNotExist_ShouldReturnFalse()
        {
            // arrange
            CreateTestDataForInMemoryDbMessage();
            var messages = GetTestMessages().Result;
            var countBefore = messages.ToList().Count;

            var chatId = 0;
            var text = _fixture.Create<string>();
            var userId = "testUserId";
            var userName = "textUsername";

            // act
            var actual = _sut.CreateMessage(chatId, text, userName, userId)
                .Result;
            messages = GetTestMessages().Result;
            var countAfter = messages.ToList().Count;

            // assert
            actual.Should().BeFalse();
            countAfter.Should().Be(countBefore);
        }

        [Fact, TestPriority(3)]
        [Trait("Integration", "Message")]
        public void DeleteMessage_ShouldReturnTrue()
        {
            // arrange
            CreateTestDataForInMemoryDbMessage();
            var messages = GetTestMessages().Result;
            var countBefore = messages.ToList().Count;

            var messageId = 1;

            // act 
            var actual = _sut.DeleteMessageFromAll(messageId).Result;
            messages = GetTestMessages().Result;
            var countAfter = messages.ToList().Count;

            // assert
            actual.Should().BeTrue();
            countBefore.Should().BeGreaterThan(countAfter);
            messages.ToList()[0].Id.Should().NotBe(messageId);
        }

        [Fact, TestPriority(4)]
        [Trait("Integration", "Message")]
        public void DeleteMessage_IfMessageDoesNotExist_ShouldReturnFalse()
        {
            // arrange
            CreateTestDataForInMemoryDbMessage();
            var messages = GetTestMessages().Result;
            var countBefore = messages.ToList().Count;

            var messageId = 0;

            // act 
            var actual = _sut.DeleteMessageFromAll(messageId).Result;
            messages = GetTestMessages().Result;
            var countAfter = messages.ToList().Count;

            // assert
            actual.Should().BeFalse();
            countBefore.Should().Be(countAfter);
        }

        [Fact, TestPriority(5)]
        [Trait("Integration", "Message")]
        public void EditMessage_ShouldReturnUpdatedMessage()
        {
            // arrange 
            CreateTestDataForInMemoryDbMessage();
            var messageId = 1;
            var newText = _fixture.Create<string>();
            var userId = "testUserId";

            // act
            var actual = _sut.EditMessage(messageId, newText, userId)
                .Result;
            var messages = GetTestMessages().Result;

            // assert
            actual.Should().NotBeNull();
            actual.Should().BeOfType<Message>();
            messages.ToList()[0].MessageText.Should().Be(newText);
        }

        [Fact, TestPriority(6)]
        [Trait("Integration", "Message")]
        public void EditMessage_IfMessageDoesNotExist_ShouldReturnNewMessage()
        {
            // arrange
            CreateTestDataForInMemoryDbMessage();
            var messageId = 1;
            var newText = _fixture.Create<string>();
            var userId = "testUserId";

            // act
            var actual = _sut.EditMessage(messageId, newText, userId)
                .Result;
            var messages = GetTestMessages().Result;

            // assert
            actual.Should().NotBeNull();
            actual.Should().BeOfType<Message>();
            messages.ToList()[0].Should().NotBe(newText);
        }

        [Fact, TestPriority(7)]
        [Trait("Integration", "Message")]
        public void ReplyToGroup_ShouldReturnMessage()
        {
            // arrange
            CreateTestDataForInMemoryDbMessage();
            var messages = GetTestMessages().Result;
            var countBefore = messages.ToList().Count;
            var replyModel = _fixture.Build<ReplyToGroupModel>()
                .With(x => x.ChatId, 1)
                .With(x => x.SenderId, "testUserId")
                .Create();

            // act
            var actual = _sut.ReplyToGroup(replyModel).Result;
            messages = GetTestMessages().Result;
            var countAfter = messages.ToList().Count;
            // assert
            actual.Should().BeOfType<Message>();
            actual.Should().NotBeNull();
            countAfter.Should().BeGreaterThan(countBefore);
        }

        [Fact, TestPriority(8)]
        [Trait("Integration", "Message")]
        public void ReplyToUser_ShouldReturnMessage()
        {
            // arrange
            CreateTestDataForInMemoryDbMessage();
            var messages = GetTestMessages().Result;
            var countBefore = messages.ToList().Count;
            var replyModel = _fixture.Build<ReplyToUserModel>()
                .With(x => x.ChatId, 1)
                .With(x => x.SenderId, "testUserId")
                .Create();

            // act
            var actual = _sut.ReplyToUser(replyModel).Result;
            messages = GetTestMessages().Result;
            var countAfter = messages.ToList().Count;

            // assert
            actual.Should().BeOfType<Message>();
            actual.Should().NotBeNull();
            countAfter.Should().BeGreaterThan(countBefore);
        }

        [Theory, TestPriority(9)]
        [Trait("Integration", "Message")]
        [InlineData(0, 10)]
        [InlineData(10, 10)]
        [InlineData(20, 40)]
        public void GetMessages_ShouldReturnMessages(int toSkip, int toLoad)
        {
            // arrange
            CreateTestDataForInMemoryDbMessage();
            var chatId = 1;

            // act
            var actual = _sut.GetMessages(chatId, toSkip, toLoad).Result;
            var messages = GetTestMessages().Result;
            var enumerable = actual.ToList();

            // assert
            enumerable.Should().BeOfType<List<Message>>();
            enumerable.Should().NotBeNull();
            enumerable.ToList().Count.Should().BeLessOrEqualTo(toLoad);
        }
    }
}