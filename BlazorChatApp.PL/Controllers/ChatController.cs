using BlazorChatApp.BLL.Infrastructure.Interfaces;
using BlazorChatApp.DAL.Domain.Entities;
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

    public ChatController(IChatService chatService, 
        UserManager<IdentityUser> userManager, 
        IUserService userService) :base(userManager)
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

    [HttpGet("createPrivateRoom/{targetId}")]
    public async Task<IActionResult> CreatePrivateRoom(string targetId)
    {
        try
        {

            var rootId = await  GetUserId();
            var privateChat = await _chatService.GetPrivateChat(rootId, targetId);
            if (rootId != null && privateChat ==null)
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

    [HttpGet("getAllUserChats")]
    public async Task<IEnumerable<Chat>> GetAllUserChats()
    {
        try
        {
            var currentUser = await GetUserId();
            IEnumerable<Chat> chats = _chatService.GetAllUserChats(currentUser);
            return chats;
        }
        catch(Exception message)
        {
            Console.WriteLine($"{message}");
            return new List<Chat>();
        }
    }

    [HttpGet("getChat/{userId}/{chatId}")]
    public async Task<Chat> GetChat(int chatId)
    {
        try
        {
            var currentUser = await GetUserId();
            Chat chat = await _chatService.GetCurrentChat(currentUser, chatId);
            return chat;
        }
        catch
        {
            return new Chat();
        }
    }

    [HttpGet("getAllChats")]
    public async Task<IEnumerable<Chat>> GetAllChats()
    {
        try
        {
            string currentUser = await GetUserId();
            IEnumerable<Chat> users = _chatService.GetAllChats(currentUser);
            return users;
        }
        catch
        {
            return new List<Chat>();
        }
    }

    [HttpGet("joinRoom/{chatId}")]
    public async Task<IActionResult> JoinRoom(int chatId)
    {
        try
        {
            string userId = await GetUserId();
            bool result = await _chatService.JoinRoom(chatId, userId);
            if (result)
            {
                return Ok();
            }
        }
        catch
        {
            return BadRequest();
        }

        return BadRequest();
    }

    [HttpGet("getCurrentChat/{chatId}")]
    public async Task<Chat> GetCurrentChat(int chatId)
    {
        try
        {
            string userId = await GetUserId();
            Chat chat = await _chatService.GetCurrentChat(userId, chatId);
            return chat;
        }
        catch
        {
            return new Chat();
        }
    }
}