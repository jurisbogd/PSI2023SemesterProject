
namespace SEProject.Models
{
	public class Flashcard
	{
		public string question { get; set; }
		public string answer { get; set; }
		public DifficultyLevel difficultyLevel { get; set; }
		private bool IsFavorite { get; set; }
		private DateTime _creationDate;
		
		public enum DifficultyLevel
		{
			Easy = 0,
			Medium,
			Hard
		}

		public Flashcard()
		{
			IsFavorite = false;
			_creationDate = DateTime.Today;

        }
		public Flashcard(string question, string answer, DifficultyLevel difficultyLevel)
		{
			this.question = question;
			this.answer = answer;
			this.difficultyLevel = difficultyLevel;
			IsFavorite = false;
			_creationDate = DateTime.Today;
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
			IsFavorite = !IsFavorite;
		}

        public override string ToString()
        {
			return $"{question} - {answer}";
        }
    }
}

