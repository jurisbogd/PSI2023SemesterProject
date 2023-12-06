using System;

namespace DatabaseAPI.Exceptions
{
    public class FlashcardPackNotFoundException : Exception
    {
        public FlashcardPackNotFoundException(string message) : base(message)
        {
        }
    }
}