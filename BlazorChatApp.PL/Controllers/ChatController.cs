using BlazorChatApp.BLL.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlazorChatApp.PL.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/api/chat")]
    public class ChatController : BaseController
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
                var userId = GetUserId();

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
