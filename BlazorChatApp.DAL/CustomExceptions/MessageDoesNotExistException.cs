using System.Runtime.Serialization;

namespace BlazorChatApp.DAL.CustomExceptions
{
    public class MessageDoesNotExistException : Exception
    {
        public MessageDoesNotExistException()
        {
        }
        public MessageDoesNotExistException(string message) : base(message)
        {
        }
        public MessageDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MessageDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
