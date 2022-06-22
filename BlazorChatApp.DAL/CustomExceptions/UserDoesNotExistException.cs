using System.Runtime.Serialization;

namespace BlazorChatApp.DAL.CustomExceptions
{
    public class UserDoesNotExistException : Exception
    {
        public UserDoesNotExistException()
        {
        }
        public UserDoesNotExistException(string message) : base(message)
        {
        }
        public UserDoesNotExistException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected UserDoesNotExistException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
