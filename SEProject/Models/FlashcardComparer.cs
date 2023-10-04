using System;
using System.Collections.Generic;

namespace SEProject.Models
{
    public class FlashcardComparer : IComparer<Flashcard>
    {
        public readonly ComparisonType _comparisonType;
        public enum ComparisonType
        {
            DifficultyLevel = 0,
            CreationDate
        }

        public FlashcardComparer(ComparisonType comparisonType)
        {
            _comparisonType = comparisonType;
        }

        public int Compare(Flashcard x, Flashcard y)
        {
            if (x == null || y == null)
            {
                throw new ArgumentException("Both objects must be non-null.");
            }
            
            switch(_comparisonType)
            {
                case ComparisonType.DifficultyLevel:
                    return x.difficultyLevel.CompareTo(y.difficultyLevel);
                    break;
                
                case ComparisonType.CreationDate:
                    return x.creationDate.CompareTo(y.creationDate);
                    break;
                
                default:
                    throw new ArgumentException("Invalid comparison type.");
            }
        }
    }
}