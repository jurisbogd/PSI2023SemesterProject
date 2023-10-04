using System;
using System.Collections.Generic;

namespace SEProject.Models
{
    public class FlashcardComparer : IComparer<Flashcard>
    {
        public int Compare(Flashcard x, Flashcard y)
        {
            if (x == null || y == null)
            {
                throw new ArgumentException("Both objects must be non-null.");
            }
            
            return x.difficultyLevel.CompareTo(y.difficultyLevel);
        }
    }
}