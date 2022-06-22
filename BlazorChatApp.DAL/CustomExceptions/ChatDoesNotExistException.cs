using System.Runtime.Serialization;

namespace BlazorChatApp.DAL.CustomExceptions
{
    public class ChatDoesNotExistException : Exception
    {
        public ChatDoesNotExistException()
        {
        }
        public ChatDoesNotExistException(string message) : base(message)
        {
        }
        public ChatDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ChatDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
