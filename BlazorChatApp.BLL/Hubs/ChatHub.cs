using BlazorChatApp.BLL.Infrastructure.Interfaces;
using BlazorChatApp.DAL.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace BlazorChatApp.BLL.Hubs
{
    public class ChatHub : Hub
    {
        
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


    }
}
