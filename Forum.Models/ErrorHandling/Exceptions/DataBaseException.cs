namespace Forum.Models.ErrorHandling
{
    public class DatabaseException : BaseException
    {
        public DatabaseException(string message) : base(message, 500)
        {
        }
    }
}
