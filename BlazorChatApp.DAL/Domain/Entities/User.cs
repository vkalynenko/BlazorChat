using Microsoft.AspNetCore.Identity;

namespace BlazorChatApp.DAL.Domain.Entities
{
    public class User : IdentityUser
    {
        private ICollection<Message> Messages { get; set; }
    }
}
