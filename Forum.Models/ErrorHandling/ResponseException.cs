using System;

namespace Forum.WebApi.ErrorHandling
{
    public class ResponseException : Exception
    {
        public string Message { get; }
        public int StatusCode { get; }

        public ResponseException(string message, int code) : base(message)
        {
            StatusCode = code;
            Message = message;
        }
    }
}
