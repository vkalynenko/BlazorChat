namespace BlazorChatApp.BLL.Infrastructure.Interfaces
{
    public interface IRequestService
    {
        Task<bool> CreateRoomAsync(string chatName);


        

    }
}
