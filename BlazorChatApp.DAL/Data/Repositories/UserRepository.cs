using BlazorChatApp.DAL.Data.Interfaces;
using BlazorChatApp.DAL.Domain.EF;
using Microsoft.AspNetCore.Identity;

namespace BlazorChatApp.DAL.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BlazorChatAppContext _context;

        public UserRepository(BlazorChatAppContext context)
        {
            _context = context;
        }
        public IEnumerable<IdentityUser> GetOtherUsers(string id)
        {
            var users = _context.Users
                .Where(x => x.Id != id);
            return users;
        }
    }
}
