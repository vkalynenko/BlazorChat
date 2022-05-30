using System.Security.Claims;
using BlazorChatApp.BLL.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorChatApp.PL.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;


        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        
        [HttpGet("createRoom/{chatName}")]
        public async Task<IActionResult> CreateRoom(string chatName)
        {
            try
            {
                //var userId = GetUserId();
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (userId != null)
                    if (chatName != null)
                        await _chatService.CreateChat(chatName, userId);

                return Ok();
            }
            catch
            {
                return StatusCode(400);
            }

        }

        //[HttpGet("createPrivateChat/{targetId}")]
        //public async Task<IActionResult> CreatePrivateRoom(string targetId)
        //{
        //    try
        //    {
        //        var rootId = GetUserId();
        //        if (rootId != null)
        //            await _chatService.CreatePrivateChat(rootId, targetId);
        //        return Ok();
        //    }
        //    catch
        //    {
        //        return StatusCode(400);
        //    }
        //}
    }
}
