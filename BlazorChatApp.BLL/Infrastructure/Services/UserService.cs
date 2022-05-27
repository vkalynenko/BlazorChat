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

        public async Task<string> Register(RegisterDto model)
        {
            var check = _userManager.Users.FirstOrDefault(x => x.UserName == model.UserName );
            if (check != null)
            {
                throw new Exception("Username is already in use, please choose other!");
            }

            var newUser = new IdentityUser { UserName = model.UserName };
            var result = await _userManager.CreateAsync(newUser, model.Password);

            if (result.Succeeded)
            {
                return "User was created!";
            }

            return "Failed to create user!";
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
