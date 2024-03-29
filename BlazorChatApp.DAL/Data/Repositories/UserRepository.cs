﻿using BlazorChatApp.DAL.CustomExceptions;
using BlazorChatApp.DAL.CustomExtensions;
using BlazorChatApp.DAL.Data.Interfaces;
using BlazorChatApp.DAL.Domain.EF;
using BlazorChatApp.DAL.Domain.Entities;
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
            var image = await _context.Images.FirstOrDefaultAsync(x => x.UserId == model.UserId);

            if (image != null)
            {
                image.ImageUrl = model.ImageUrl;
                _context.Images.Update(image);
            }
            else
            {
                var newImage = new Image
                {
                    ImageUrl = model.ImageUrl,
                    UserId = model.UserId,
                };

                await _context.Images.AddAsync(newImage);
            }
        }

        public async Task<string> GetImageLink(string userId)
        {
            var image = await _context.Images.FirstOrDefaultAsync(x => x.UserId == userId);
            if (image == null)
            {
                return "https://storageaccountchatapp.blob.core.windows.net/images/avatar.png";
            }
            return image.ImageUrl;
        }
    }
}
