using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorChatApp.PL.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : BaseController
    {
        [HttpGet]
        public string Test()
        {
            return "Authorized!";
        }

    }
}
