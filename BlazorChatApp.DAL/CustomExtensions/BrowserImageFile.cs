using Microsoft.AspNetCore.Components.Forms;

namespace BlazorChatApp.DAL.CustomExtensions
{
    public class BrowserImageFile 
    {
        public string Name { get; set; }
        public string ContentType { get; }
        public string Data { get; set; }
        public string Type { get; set; }
        public string UserId { get; set; }
        public string ImageUrl { get; set; }
    }
}
