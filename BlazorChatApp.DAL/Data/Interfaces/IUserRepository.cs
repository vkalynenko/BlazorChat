using Microsoft.AspNetCore.Identity;

namespace BlazorChatApp.DAL.Data.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<IdentityUser> GetUsers(string id);
    }
}
