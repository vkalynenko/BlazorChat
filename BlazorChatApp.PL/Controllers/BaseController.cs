using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorChatApp.PL.Controllers
{
    [Authorize]
    [Controller]
    public class BaseController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _manager;

        public BaseController(UserManager<IdentityUser> manager)
        {
            _manager = manager;
        }

        protected async Task<string> GetUserId()
        {

            var userName = User.FindFirstValue(ClaimTypes.Name);

            var user = await _manager.FindByNameAsync(userName);
            var userId = user.Id;

            return userId;
        }
        protected string GetUserName()
        {

            var userName = User.FindFirstValue(ClaimTypes.Name);
            return userName;
        }
    }
}
