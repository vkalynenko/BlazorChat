using BlazorChatApp.BLL.Contracts.DTOs;
using BlazorChatApp.BLL.Hubs;
using BlazorChatApp.BLL.Infrastructure.Interfaces;
using BlazorChatApp.BLL.Responses;
using BlazorChatApp.DAL.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BlazorChatApp.PL.Controllers
{
    [Authorize]
    [Route("/api/message")]
    [ApiController]
    public class MessageController : BaseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMessageService _messageService;
        private readonly IHubContext<ChatHub> _chatHub;

        public MessageController(IChatService chatService,
            UserManager<IdentityUser> userManager,
            IUserService userService, IMessageService messageService, IHubContext<ChatHub> chatHub) : base(userManager)
        {
            _userManager = userManager;
            _messageService = messageService;
            _chatHub = chatHub;
        }

        [HttpGet("joinRoom/{connectionId}/{chatId}")]
        public async Task<IActionResult> JoinRoom(string connectionId, string chatId)
        {
            await _chatHub.Groups.AddToGroupAsync(connectionId, chatId);
            return Ok();
        }

        [HttpPost("sendMessage")]
        public async Task<Message> SendMessage(MessageDto messageDto)
        {
            try
            {
                if (messageDto.Message == null)
                {
                    new Message();
                }
                var entity = new Message
                {
                    ChatId = messageDto.ChatId,
                    MessageText = messageDto.Message,
                    SenderName = User.Identity.Name,
                    SentTime = DateTime.Now,
                };
                bool result =
                    await _messageService.CreateMessage(messageDto.ChatId, 
                        messageDto.Message, User.Identity.Name, await GetUserId());
                if (result)
                {
                    return entity;
                }
                return new Message();
            }
            catch
            {
                return new Message();
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
                return null;
            }
        }
        [HttpGet("getUserName")]
        public async Task<string> GetName()
        {
            try
            {
                return await GetUserName();
            }
            catch
            {
                return null;
            }
        }
    }
}
