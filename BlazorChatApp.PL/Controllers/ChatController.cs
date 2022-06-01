using BlazorChatApp.BLL.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorChatApp.PL.Controllers;

[Authorize]
[ApiController]
[Route("/api/chat")]
public class ChatController : BaseController
{
    private readonly IChatService _chatService;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserService _userService;

    public ChatController(IChatService chatService, UserManager<IdentityUser> userManager, IUserService userService) :base(userManager)
    {
        _chatService = chatService;
        _userManager = userManager;
        _userService = userService;
    }


    [HttpGet("createRoom/{chatName}")]
    public async Task<IActionResult> CreateRoom(string chatName)
    {
        try
        {
            var userId = await GetUserId();

            if (userId == null || chatName == null)
                return StatusCode(400);

            await _chatService.CreateChat(chatName, userId);

            return Ok();
        }
        catch
        {
            return StatusCode(400);
        }
    }


    [HttpGet("createPrivateChat/{targetId}")]
    public async Task<IActionResult> CreatePrivateRoom(string targetId)
    {
        try
        {
            var rootId = await  GetUserId();
            if (rootId != null)
                await _chatService.CreatePrivateChat(rootId, targetId);
            return Ok();
        }
        catch
        {
            return StatusCode(400);
        }
    }

    [HttpGet("getAllUsers")]
    public async Task<IEnumerable<IdentityUser>> GetAllUsers()
    {
        try
        {
            var currentUser = await GetUserId();
            
            var users = _userService.GetUsers(currentUser);
            return users;
        }
        catch
        {
            return new List<IdentityUser>();

        }
    }
}