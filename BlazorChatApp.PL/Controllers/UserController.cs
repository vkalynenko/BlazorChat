using BlazorChatApp.BLL.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorChatApp.PL.Controllers
{
    [Authorize]
    [Route("/api/user")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(UserManager<IdentityUser> userManager, IUserService userService):base(userManager)
        {
            _userService = userService;
        }

        [HttpGet("getOtherUsers")]
        public async Task<IEnumerable<IdentityUser>> GetOtherUsers() 
        {
            try
            {
                var currentUser = await GetUserId();
                var users = _userService.GetOtherUsers(currentUser);
                return users;
            }
            catch
            {
                return new List<IdentityUser>();
            }
        }

        [HttpGet("getUserId")] 
        public async Task<string> GetId()
        {
            try
            {
                return await GetUserId();
            }
            catch
            {
                return String.Empty;
            }
        }

        [HttpGet("getUserName")]
        public string GetName()
        {
            try
            {
                return GetUserName();
            }
            catch
            {
                return String.Empty;
            }
        }
    }
}
