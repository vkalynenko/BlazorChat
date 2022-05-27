using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorChatApp.PL.Controllers
{
    [Authorize]
    [Route("/api/test")]
    [ApiController]
    public class TestController : Controller
    {

       
      
        [HttpPost("text")]
        public async Task<IActionResult> Text()
        {
            var text = "Hello";
            return Ok();
        }
    }
}
