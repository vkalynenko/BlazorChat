using BlazorChatApp.DAL.CustomExceptions;
using BlazorChatApp.DAL.CustomExtensions;
using BlazorChatApp.DAL.Data.Interfaces;
using BlazorChatApp.DAL.Domain.EF;
using BlazorChatApp.DAL.Models;
using Castle.Core.Internal;
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
        public IEnumerable<IdentityUser> GetOtherUsers(string id)
        {
            var users = _context.Users
                .Where(x => x.Id != id);
            return users;
        }

        public async Task SaveProfile(BrowserImageFile model)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x =>
                x.Id == model.UserId);

            if (user == null)
            {
                throw new UserDoesNotExistException();
            }

            //if (!model.PhoneNumber.IsNullOrEmpty())
            //{
            //    user.PhoneNumber = model.PhoneNumber;
            //}
            //user.Age = model.Age;

            if (!model.ImageUrl.IsNullOrEmpty())
            {
                user.ImageUrl = model.ImageUrl;
            }
            
            _context.Users.Update(user);
        }
    }
}
