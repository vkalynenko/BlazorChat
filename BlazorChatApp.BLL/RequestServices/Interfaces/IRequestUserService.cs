using BlazorChatApp.BLL.Responses;

namespace BlazorChatApp.BLL.MainRequestServices.Interfaces
{
    public interface IRequestUserService
    {
        Task<GetCurrentUserInfo> GetUserInfo();
        Task<GetAllUsersResponse> GetOtherUsersAsync();
    }
}
