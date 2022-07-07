using BlazorChatApp.BLL.Contracts.DTOs;
using BlazorChatApp.BLL.Infrastructure.Services;
using BlazorChatApp.DAL.Data;
using BlazorChatApp.DAL.Data.Interfaces;
using BlazorChatApp.DAL.Data.Repositories;
using BlazorChatApp.DAL.Domain.EF;
using BlazorChatApp.DAL.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorChat.Tests.IntegrationTests.Services
{
    public class IntegrationUserServiceTest : BaseIntegrationServiceTest
    {
        private readonly Fixture _fixture = new();
        private readonly Mock<IChatRepository> _chatRepository = new();
        private readonly Mock<IMessageRepository> _messageRepository = new();
        private readonly UserService _sut;

        public IntegrationUserServiceTest() : base()
        {
            var contextOptions = new DbContextOptionsBuilder<BlazorChatAppContext>()
                .UseInMemoryDatabase("TestBlazorChatDatabase")
                .Options;
            var appContext = new BlazorChatAppContext(contextOptions);
            UserManager<IdentityUser> userManager = new UserManager<IdentityUser>(
               new UserStore<IdentityUser>(appContext), null,
               new PasswordHasher<IdentityUser>(null),
           null, null, null, null, null, null);
            IUserRepository userRepository = new UserRepository(appContext);
            IUnitOfWork unitOfWork = new UnitOfWork(appContext, _chatRepository.Object, 
                _messageRepository.Object, userRepository);
           
            _sut = new UserService(userManager, unitOfWork);
            _fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
        }

        [Fact]
        [Trait("Integration", "User")]
        public void GetOtherUsers_ShouldReturnAllUsersExceptOfCurrentUser()
        {
            // arrange
            CreateTestDataForInMemoryDb();
            var userId = "testUserId";

            // act
            var actual = _sut.GetOtherUsers(userId).ToList();

            // assert
            actual.Should().BeOfType<List<IdentityUser>>();
            actual.Should().NotBeNull();
            actual.All(x => x.Id != userId).Should().BeTrue();
        }

        [Fact]
        [Trait("Integration", "User")]
        public void Register_ShouldReturnSuccessfulString()
        {
            // arrange
            CreateTestDataForInMemoryDb();
            var registerModel = _fixture.Create<RegisterDto>();

            var users = GetTestUsers().Result.ToList();
            var countBefore = users.Count();

            // act
            var actual = _sut.Register(registerModel).Result;
            users = GetTestUsers().Result.ToList();
            var countAfter = users.Count();

            // assert
            actual.Should().Be("User was created!");
            users.Should().Contain(x => x.UserName == registerModel.UserName);
            (countBefore + 1).Should().Be(countAfter);
        }

        [Fact]
        [Trait("Integration", "User")]
        public void Register_IfUserIsAlreadyExists_ShouldReturnUnsuccessfulString()
        {
            // arrange
            CreateTestDataForInMemoryDb();
            var users = GetTestUsers().Result.ToList();
            var countBefore = users.Count();

            var registerModel = _fixture.Build<RegisterDto>()
                .With(x => x.UserName, "testUserName")
                .Create();

            // act
            var actual = _sut.Register(registerModel).Result;
            users = GetTestUsers().Result.ToList();
            var countAfter = users.Count();

            // assert
            actual.Should().Be("Failed to create user!");
            countAfter.Should().Be(countBefore);
        }

        [Fact]
        [Trait("Integration", "User")]
        public void Login_ShouldReturnIdentityUser()
        {
            // arrange
            CreateTestDataForInMemoryDb();

            var registerModel = new RegisterDto
            {
                UserName = "testUser",
                Password = "testPassword",
            };
            var loginModel = new LoginDto
            {
                UserName = "testUser",
                Password = "testPassword",
            };
            // act
            var task = _sut.Register(registerModel);
            var actual = _sut.Login(loginModel).Result;

            // assert
            actual.Should().NotBeNull();
            actual.UserName.Should().Be(loginModel.UserName);
            actual.Should().BeOfType<IdentityUser>();
        }

        [Fact]
        [Trait("Integration", "User")]
        public void Login_IfDataIsIncorrect_ShouldThrowException()
        {
            // arrange
            CreateTestDataForInMemoryDb();

            var loginModel = _fixture.Create<LoginDto>();
           
            // act
            // assert
            Assert.Throws<AggregateException>(()=>_sut.Login(loginModel).Result);

        }
    }
}
