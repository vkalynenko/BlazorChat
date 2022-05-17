using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace BlazorChatApp.DAL.Domain.Entities
{
    public class Message
    {
        private string text;

        public int MessageId { get; set; }
        public string MessageText { get; set; }

        public DateTime SenTime { get; set; }
    }
}
