using BlazorChatApp.DAL.CustomExtensions;
using Microsoft.AspNetCore.Identity;

namespace BlazorChatApp.DAL.Data.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<IdentityUser> GetOtherUsers(string id);
        Task SaveProfile(BrowserImageFile model);
        Task<string> GetImageLink(string userId);
    }
}
