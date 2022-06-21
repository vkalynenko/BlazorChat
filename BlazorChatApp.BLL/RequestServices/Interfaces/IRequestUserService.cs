using BlazorChatApp.BLL.Responses;

namespace BlazorChatApp.BLL.RequestServices.Interfaces
{
    public interface IRequestUserService
    {
        Task<GetCurrentUserInfo> GetUserInfo();
        Task<GetAllUsersResponse> GetOtherUsersAsync();
    }
}
