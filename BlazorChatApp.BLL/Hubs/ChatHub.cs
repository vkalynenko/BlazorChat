using BlazorChatApp.BLL.Infrastructure.Interfaces;
using BlazorChatApp.BLL.Models;
using BlazorChatApp.DAL.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BlazorChatApp.BLL.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessageService _messageService;

        public ChatHub(IMessageService messageService)
        {
            _messageService = messageService;
        }

        public string GetConnectionId() => Context.ConnectionId;

        public Task JoinRoom(int chatId)
        {
            return Groups.AddToGroupAsync(GetConnectionId(), chatId.ToString());
        }

        public Task LeaveRoom(string chatId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task SendMessage(int chatId, Message message)
        {
            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", message);
        }

        public async Task DeleteFromAll(int chatId, int messageId)
        {
            await _messageService.DeleteMessageFromAll(messageId);
        }
        public async Task Edit(int messageId, string messageText)
        {
            await _messageService.EditMessage(messageId, messageText);
        }

        public async Task ReplyToGroup(ReplyToGroupModel model)
        {
            var message = await _messageService.ReplyToGroup(model);
            await SendMessage(model.ChatId, message);
        }


    }
}
