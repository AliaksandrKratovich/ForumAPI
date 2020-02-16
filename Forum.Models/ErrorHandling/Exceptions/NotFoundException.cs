namespace Forum.Models.ErrorHandling
{
    public class NotFoundException : BaseException
    {
        public NotFoundException(string message) : base(message, 500)
        {
        }
    }
}
