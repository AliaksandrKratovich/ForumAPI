using System;

namespace Forum.Models.ErrorHandling
{
    public abstract class BaseException : Exception
    {
        public string Message { get;}
        public int StatusCode { get; }

        protected BaseException(string message, int statusCode)
        {
            Message = message;
            StatusCode = statusCode;
        }
    }
}
