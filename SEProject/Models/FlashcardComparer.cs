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

        public int Compare(Flashcard? x, Flashcard? y)
        {
            if (x == null || y == null)
            {
                throw new ArgumentException("Both objects must be non-null.");
            }
            
            switch(_comparisonType)
            {
                case ComparisonType.DifficultyLevel:
                    return x.Difficulty.CompareTo(y.Difficulty);
                
                case ComparisonType.CreationDate:
                    return x.CreationDate.CompareTo(y.CreationDate);
                
                default:
                    throw new ArgumentException("Invalid comparison type.");
            }
        }
    }
}