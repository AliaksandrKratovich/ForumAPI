using Forum.Models.ErrorHandling;

namespace Forum.WebApi.ErrorHandling
{
    public class ResponseException :BaseException
    {
        public ResponseException(string message, int code) : base(message, code)
        {
        }
    }
}
