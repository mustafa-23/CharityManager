using System;

namespace CharityManager.UI
{

    public class CallServiceException : Exception
    {
        public string ServiceName { get; private set; }
        public object Request { get; private set; }
        public string UserMessage { get; private set; }
        public CallServiceException(string service, string message, string userMessage, object request = null)
            : base(message)
        {
            ServiceName = service;
            UserMessage = userMessage;
            Request = request;
        }
    }
}
