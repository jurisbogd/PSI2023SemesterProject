namespace SEProject.Models
{
	public class Flashcard
	{
		public Guid ID { get; set; }
		public Guid PackID { get; set; }
		public string? Question { get; set; }
		public string? Answer { get; set; }
		public DifficultyLevel Difficulty { get; set; }
		public bool IsFavorite { get; set; }
		public DateTime CreationDate { get; set; }
		
		public enum DifficultyLevel
		{
			Easy = 0,
			Medium,
			Hard
		}

		public Flashcard()
		{
			IsFavorite = false;
			CreationDate = DateTime.Now;
		}

		// optional arguments
		public Flashcard(Guid id, string question = "Test question", string answer = "Test answer", DifficultyLevel difficulty = DifficultyLevel.Easy, bool isFavorite = false)
		{
			ID = id;
			Question = question;
			Answer = answer;
			Difficulty = difficulty;
			IsFavorite = isFavorite;
			CreationDate = DateTime.Now;
		}

        public override string ToString()
        {
			return $"{Question} - {Answer} - {Difficulty} - {CreationDate} - {ID}";
        }
    }
}

