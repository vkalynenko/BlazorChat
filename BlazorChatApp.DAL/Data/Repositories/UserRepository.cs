using BlazorChatApp.DAL.Data.Interfaces;
using BlazorChatApp.DAL.Domain.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazorChatApp.DAL.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly BlazorChatAppContext _context;

        public UserRepository(BlazorChatAppContext context)
        {
            _context = context;
        }
        public IEnumerable<IdentityUser> GetUsers(string id)
        {
            var users = _context.Users
                .Where(x => x.Id != id);
            return users;
        }
    }
}
