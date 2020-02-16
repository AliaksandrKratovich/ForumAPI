using System;
using System.Collections.Generic;
using System.Text;

namespace Forum.Tests.Helpers
{
    public static class GeneralHelpers
    {

        private static Random _random = new Random();
        public static string GetRandomString(int length)
        {

            var resultCharArray = new char[length];
            for (int i = 0; i < length; i++)
            {
                resultCharArray[i] = (char)(_random.Next(50) + 1);
            }
            return new string(resultCharArray);
        }
    }
}
