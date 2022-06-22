using System.Runtime.Serialization;

namespace BlazorChatApp.DAL.CustomExceptions
{
    public class ChatsDoNotExistException : Exception
    {
        public ChatsDoNotExistException()
        {
        }
        public ChatsDoNotExistException(string message) : base(message)
        {
        }
        public ChatsDoNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ChatsDoNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
