using BlazorChatApp.BLL.Contracts.DTOs;
using BlazorChatApp.BLL.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BlazorChatApp.BLL.Infrastructure.Services
{
    
    public class AuthorizationService : IAuthorizationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;


        public AuthorizationService(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

        }

        public async Task<string> Register(string userName, string password)
        {
            var model = new RegisterDto
            {
                UserName = userName,
                Password = password,
            };
            var user = _userManager.Users.FirstOrDefault(c => c.UserName == model.UserName);
            if (user != null)
            {
                throw new Exception("User is already exists");
            }

            var appUser = new IdentityUser {UserName = model.UserName};
            var result = await _userManager.CreateAsync(appUser, model.Password);
            return result.Succeeded ? "Ok" : "User wasn't created";

        }

        public async Task<string> Login(string userName, string password)
        {
            var model = new LoginDto
            {
                UserName = userName,
                Password = password,
            };
            var user = _userManager.Users.FirstOrDefault(c => c.UserName == model.UserName);
            if (user != null)
            {
                var result = await _signInManager
                .PasswordSignInAsync(model.UserName, model.Password, true, false);
                
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, false);
                }

                throw new Exception( "Login or password isn't correct") ;
            }

            throw new NullReferenceException("This user doesn't exist");


        }

        public async Task LogOut()
        {
            await _signInManager.SignOutAsync();
        }

    }
}
