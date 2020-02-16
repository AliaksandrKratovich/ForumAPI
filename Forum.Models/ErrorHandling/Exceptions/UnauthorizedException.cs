﻿namespace Forum.Models.ErrorHandling.Exceptions
{
    public class UnauthorizedException : BaseException
    {
        public UnauthorizedException(string message) : base(message, 401)
        {
        }
    }
}
