using BlazorChatApp.BLL.Responses;
using BlazorChatApp.DAL.CustomExtensions;
using BlazorChatApp.DAL.Models;

namespace BlazorChatApp.BLL.RequestServices.Interfaces
{
    public interface IRequestUserService
    {
        Task<GetCurrentUserInfo> GetUserInfo();
        Task<GetAllUsersResponse> GetOtherUsersAsync();
        Task<SaveProfileResponse> SaveUserProfileInfo(BrowserImageFile profile);
    }
}
