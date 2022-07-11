namespace BlazorChatApp.PL.Options
{
    public class WebCamOptions
    {
        public int Width { get; set; } = 320;
        public string? VideoID { get; set; }
        public string? CanvasId { get; set; }
        public string? Filter { get; set; } = null;
    }
}
