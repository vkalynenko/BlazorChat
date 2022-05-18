using Microsoft.AspNetCore.Identity;

namespace BlazorChatApp.DAL.Domain.Entities
{
    public class User : IdentityUser
    {
        public virtual ICollection<Message> Messages { get; set; } = new HashSet<Message>();
    }
}
