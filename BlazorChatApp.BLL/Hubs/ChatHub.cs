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
            await Clients.Groups(chatId.ToString()).SendAsync("DeleteMessageFromAll", messageId);
        }

        public async Task Edit(int chatId, int messageId, string messageText)
        {
            var message = await _messageService.EditMessage(messageId, messageText);
            await Clients.Groups(chatId.ToString()).SendAsync("ReceiveEditedMessage", message);
        }

        public async Task ReplyToGroup(ReplyToGroupModel model)
        {
            var message = await _messageService.ReplyToGroup(model);
            await SendMessage(model.ChatId, message);
        }

        public async Task ReplyToUser(ReplyToUserModel model)
        {
            var message = await _messageService.ReplyToUser(model);
            await SendMessage(model.ChatId, message);
        }

        public async Task ReadMore(int chatId, int quantityToSkip, int quantityToLoad)
        {
            var messages = await _messageService.GetMessages(chatId, quantityToSkip, quantityToLoad);
            await Clients.Caller.SendAsync("ReceiveLoadedMessages", messages);
        }
       

    }
}
