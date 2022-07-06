using BlazorChatApp.DAL.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorChatApp.DAL.Domain.EF;

public class BlazorChatAppContext : IdentityDbContext<IdentityUser>
{

    public BlazorChatAppContext(DbContextOptions<BlazorChatAppContext> options)
        : base(options)
    {
    }
    public DbSet<Chat> Chats { get; set; }
    public DbSet<Message> Messages { get; set; }   
    public DbSet<ChatUser> ChatUsers { get; set; }
    public DbSet<User> Users { get; set; }
}
