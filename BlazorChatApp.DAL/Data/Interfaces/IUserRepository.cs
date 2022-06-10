using Microsoft.AspNetCore.Identity;

namespace BlazorChatApp.DAL.Data.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<IdentityUser> GetOtherUsers(string id);
    }
}
