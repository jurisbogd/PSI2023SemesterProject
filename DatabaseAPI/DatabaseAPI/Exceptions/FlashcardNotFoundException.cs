using System;

namespace DatabaseAPI.Exceptions
{
    public class FlashcardNotFoundException : Exception
    {
        public FlashcardNotFoundException(string message) : base(message)
        {
        }
    }
}