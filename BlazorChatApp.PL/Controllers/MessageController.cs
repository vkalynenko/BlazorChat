using BlazorChatApp.BLL.Infrastructure.Interfaces;
using BlazorChatApp.DAL.Domain.Entities;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlazorChatApp.PL.Controllers
{
    [Authorize]
    [Route("/api/message")]
    [ApiController]
    public class MessageController : BaseController
    {
        private readonly IMessageService _messageService;

        public MessageController(IChatService chatService,
            UserManager<IdentityUser> userManager,
            IUserService userService, IMessageService messageService) : base(userManager)
        {
            _messageService = messageService;
        }

        [HttpGet("sendMessage/{chatId}/{message}")]
        public async Task<Message> SendMessage(int chatId, string message)
        {
            try

            {
                var userId = await GetUserId();
                var userName = GetUserName();

                if (message.IsNullOrEmpty())
                {
                    return new Message();
                }
                var image = await _messageService.GetImage(userId);
                var entity = new Message
                {
                    ChatId = chatId,
                    MessageText = message,
                    SenderName = userName,
                    UserId = userId,
                    Image = image,
                };
                bool result =
                    await _messageService.CreateMessage(chatId,
                        message, userName,  userId);

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

        [HttpGet("getAllMessages/{chatId}/{quantityToSkip}/{quantityToLoad}")]
        public async Task<IEnumerable<Message>> GetAllMessages(int chatId, int quantityToSkip, int quantityToLoad)
        {
            try
            {
                return await _messageService.GetMessages(chatId, quantityToSkip, quantityToLoad);
            }
            catch
            {
                return new List<Message>();
            }
        }
    }
}
