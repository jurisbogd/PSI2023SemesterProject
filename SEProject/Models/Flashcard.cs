namespace SEProject.Models
{
	public class Flashcard
	{
		public Guid ID { get; set; }
		public string? question { get; set; }
		public string? answer { get; set; }
		public DifficultyLevel difficultyLevel { get; set; }
		public bool isFavorite { get; set; }
		public DateTime creationDate { get; set; }
		
		public enum DifficultyLevel
		{
			Easy = 0,
			Medium,
			Hard
		}

		public Flashcard()
		{
			isFavorite = false;
			creationDate = DateTime.Now;
		}
    
		public Flashcard(Guid ID, string question, string answer, DifficultyLevel difficultyLevel)
		{
			this.ID = ID;
			this.question = question;
			this.answer = answer;
			this.difficultyLevel = difficultyLevel;
			isFavorite = false;
			creationDate = DateTime.Now;
		}

		// Edit flashcard method
		public void EditFlashcard(string newQuestion, string newAnswer, DifficultyLevel newDifficultyLevel)
		{
			question = newQuestion;
			answer = newAnswer;
			difficultyLevel = newDifficultyLevel;
		}

		// Switch favorite to opposite
		public void ToggleFavorite()
		{
			isFavorite = !isFavorite;
		}

        public override string ToString()
        {
			return $"{question} - {answer}";
        }
    }
}

