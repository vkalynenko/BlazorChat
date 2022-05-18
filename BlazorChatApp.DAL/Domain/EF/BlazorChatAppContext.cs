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
}
