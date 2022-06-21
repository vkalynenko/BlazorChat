using System.Runtime.Serialization;

namespace BlazorChatApp.DAL.CustomExceptions
{
    public class ChatIsAlreadyExistsException : Exception
    {
        public ChatIsAlreadyExistsException()
        {
        }
        public ChatIsAlreadyExistsException(string message) : base(message)
        {
        }
        public ChatIsAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ChatIsAlreadyExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
