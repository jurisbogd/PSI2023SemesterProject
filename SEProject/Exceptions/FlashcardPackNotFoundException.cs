using System;

namespace SEProject.Exceptions
{
    public class FlashcardPackNotFoundException : Exception
    {
        public FlashcardPackNotFoundException(string message) : base(message)
        {
        }
    }
}