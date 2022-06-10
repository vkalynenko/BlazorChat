using BlazorChatApp.BLL.Contracts.DTOs;
using BlazorChatApp.BLL.Infrastructure.Interfaces;
using BlazorChatApp.DAL.Data.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace BlazorChatApp.BLL.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
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
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                return await _userManager.CheckPasswordAsync(user, model.Password) ? user : null;
            }
            throw new Exception("Incorrect login or password!");

        }

        public IEnumerable<IdentityUser> GetOtherUsers(string id)
        {
            return _unitOfWork.User.GetOtherUsers(id);
        }
    }
}
