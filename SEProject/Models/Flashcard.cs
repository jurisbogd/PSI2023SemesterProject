using System;

namespace SEProject.Models
{
	public class Flashcard
	{
		public Guid ID { get; set; }
		public string question { get; set; }
		public string answer { get; set; }
		public DifficultyLevel difficultyLevel { get; set; }
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
			CreationDate = DateTime.Today;
		}
    
		public Flashcard(Guid ID, string question, string answer, DifficultyLevel difficultyLevel)
		{
			this.ID = ID;
			this.question = question;
			this.answer = answer;
			this.difficultyLevel = difficultyLevel;
			IsFavorite = false;
			CreationDate = DateTime.Today;
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

