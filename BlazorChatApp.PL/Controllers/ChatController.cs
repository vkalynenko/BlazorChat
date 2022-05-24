using System.Security.Claims;
using BlazorChatApp.BLL.Hubs;
using BlazorChatApp.DAL.Data.Interfaces;
using BlazorChatApp.DAL.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace BlazorChatApp.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ChatHub> _chat;
        private readonly IChatRepository _chatRepository;
        private readonly IMessageRepository _messageRepository;
        public ChatController(IWebHostEnvironment environment, IUnitOfWork unitOfWork, IHubContext<ChatHub> chat, IChatRepository chatRepository, IMessageRepository messageRepository)
        {
            _environment = environment;
            _unitOfWork = unitOfWork;
            _chat = chat;
            _chatRepository = chatRepository;
            _messageRepository = messageRepository;
        }

        [HttpPost("[action]/{connectionId}/{chatId}")]
        public async Task<IActionResult> JoinRoom(string connectionId, string chatId)
        {
            await _chat.Groups.AddToGroupAsync(connectionId, chatId);
            return Ok();
        }

        [HttpPost("[action]/{connectionId}/{chatId}")]
        public async Task<IActionResult> LeaveRoom(string connectionId, string chatId)
        {
            await _chat.Groups.RemoveFromGroupAsync(connectionId, chatId);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SendMessage(int chatId, string roomName, string message)
        {
            try
            {
                if (message == null)
                {
                    return Ok();
                }

                var entity = new Message
                {
                    ChatId = chatId,
                    MessageText = message,
                    SenderName = User.Identity?.Name,
                    SentTime = DateTime.Now,
                };
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (User.Identity?.Name != null)
                    await _messageRepository.CreateMessage(chatId, message, User.Identity?.Name, userId);

                await _chat.Clients.Group(roomName)
                    .SendAsync("ReceiveMessage", new
                    {
                        UserName = entity.SenderName,
                        Text = entity.MessageText,
                        SentTime = entity.SentTime.ToString(),
                    });
                return Ok();
            }
            catch
            {
                return BadRequest("Something went wrong");
            }
        }

    }
}
