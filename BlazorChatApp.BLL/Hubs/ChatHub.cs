using Microsoft.AspNetCore.SignalR;

namespace BlazorChatApp.BLL.Hubs
{
    public class ChatHub : Hub
    {
        public async Task Send(string message, string userName)
        {
            await Clients.All.SendAsync("Send", message, userName);
        }


    }
}
