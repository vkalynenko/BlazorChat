using System.Security.Claims;
using BlazorChatApp.DAL.Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorChatApp.PL.Controllers
{
    [Authorize]
    [Route("/api/chat")]
    [ApiController]
    public class ChatController : Controller
    {
        private readonly IChatRepository _chatRepository;

        public ChatController(IChatRepository chatRepository)
        {
            _chatRepository = chatRepository;
        }


        [HttpPost("createRoom")]
        public async Task<IActionResult> CreateRoom(string chatName)
        {
            try
            {
               
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (userId != null) await _chatRepository.CreateChat(chatName, userId);
                return Ok();
            }
            catch
            {
                return StatusCode(400);
            }

        }
    }
}
