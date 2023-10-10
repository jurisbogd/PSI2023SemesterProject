namespace SEProject.Models
{
	public class Flashcard
	{
		public Guid ID { get; set; }
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
    
		public Flashcard(Guid id, string question, string answer, DifficultyLevel difficulty)
		{
			ID = id;
			Question = question;
			Answer = answer;
			Difficulty = difficulty;
			IsFavorite = false;
			CreationDate = DateTime.Now;
		}

		// Edit flashcard method
		public void EditFlashcard(string question, string answer, DifficultyLevel difficulty)
		{
			Question = question;
			Answer = answer;
			Difficulty = difficulty;
		}

		// Switch favorite to opposite
		public void ToggleFavorite()
		{
			IsFavorite = !IsFavorite;
		}

        public override string ToString()
        {
			return $"{Question} - {Answer}";
        }
    }
}

