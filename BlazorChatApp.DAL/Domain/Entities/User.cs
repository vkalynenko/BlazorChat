using Microsoft.AspNetCore.Identity;

namespace BlazorChatApp.DAL.Domain.Entities
{
    public class User : IdentityUser
    {
        public int Age { get; set; }
        public string ImageUrl { get; set; }

    }
}
