using BlazorChatApp.BLL.Contracts.DTOs;
using BlazorChatApp.BLL.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BlazorChatApp.BLL.Infrastructure.Services
{

    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        public Task<string> Register(RegisterDto model)
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityUser> Login(LoginDto model)
        {
            var user = _userManager.Users.FirstOrDefault(x => x.UserName == model.UserName);

            if (user != null)
            {
                return await _userManager.CheckPasswordAsync(user, model.Password) ? user : null;
            }

            throw new Exception("Incorrect login or password!");

        }
    }
}
