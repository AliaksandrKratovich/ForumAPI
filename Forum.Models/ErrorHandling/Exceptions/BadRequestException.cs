namespace Forum.Models.ErrorHandling
{
    public class BadRequestException : BaseException
    {
        public BadRequestException(string message) : base(message, 400)
        {
        }
    }
}
