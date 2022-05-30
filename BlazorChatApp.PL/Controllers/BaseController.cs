using System.Security.Claims;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Mvc;

namespace BlazorChatApp.PL.Controllers
{
    [Controller]
    public class BaseController : ControllerBase
    {
      
        protected string? GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId;
        }
    }
}
