using System.ComponentModel.DataAnnotations;

namespace BlazorChatApp.DAL.Domain.Entities
{
    public class Message 
    {
        [Key]
        public int Id { get; set; }
        public string MessageText { get; set; }
        public DateTime SentTime { get; set; }
        public int ChatId { get; set; }
        public string UserId { get; set; }
        public string SenderName { get; set; }
        public bool IsItReply { get; set; }
        public int ImageId { get; set; }
        public Image Image { get; set; }
        public Message()
        {
            //DateTime time = DateTime.UtcNow;
            //TimeZoneInfo localZone = TimeZoneInfo.Local;
            //TimeZoneInfo cstZone = TimeZoneInfo.FindSystemTimeZoneById(localZone.Id);

            ////SentTime = TimeZoneInfo.ConvertTimeFromUtc(time, cstZone);
            //SentTime = time.AddHours(localZone.BaseUtcOffset.Hours);

            SentTime=DateTime.SpecifyKind(DateTime.UtcNow.AddHours(3.0), DateTimeKind.Local);

            //TimeZone tzone = TimeZone.CurrentTimeZone;

            //TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(tzone.DaylightName);
            //DateTime result = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tzi);

            //SentTime = result;
        }
    }
}