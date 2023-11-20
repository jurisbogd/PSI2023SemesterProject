using System;

namespace SEProject.Exceptions
{
    public class FlashcardNotFoundException : Exception
    {
        public FlashcardNotFoundException(string message) : base(message)
        {
        }
    }
}