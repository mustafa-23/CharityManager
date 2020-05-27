using System;

namespace CharityManager.Service
{

    [Serializable]
    public class CustomException : Exception
    {
        public string UserMessage { get; set; }
        public CustomException() { }
        public CustomException(string userMessage, string message) : this(userMessage, message, null) { }
        public CustomException(string userMessage, string message, Exception inner) : base(message, inner)
        {
            UserMessage = userMessage;
        }
        protected CustomException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
