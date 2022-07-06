using BlazorChatApp.DAL.CustomExtensions;

namespace BlazorChatApp.DAL.Models
{
    public class ProfileModel
    {
        public int Age { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ImageUrl{ get; set; }
        public byte[]? Photo { get; set; }
        public string? UserId { get; set; }
        public BrowserImageFile File { get; set; }


    }
}
