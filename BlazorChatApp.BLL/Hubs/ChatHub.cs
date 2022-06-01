using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace BlazorChatApp.BLL.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        //public async Task Send(string message, string userName)
        //{
        //    await Clients.All.SendAsync("Send", message, userName);
        //}
        public string GetConnectionId() => Context.ConnectionId;

        public Task JoinRoom(string roomId)
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, roomId);
        }

        public Task LeaveRoom(string roomId)
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);
        }

        public async Task SendMessage(string message, string userName)
        {
            await Clients.All.SendAsync("ReceiveMessage", message, userName);
        }


    }
}
