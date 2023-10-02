using System;

namespace SEProject.Models
{
	public class Flashcard
	{
		public Guid ID { get; set; }
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

		public DateTime CreationDate
		{
			get { return _creationDate; }
		}

		public Flashcard()
		{
			IsFavorite = false;
			_creationDate = DateTime.Today;
		}
    
		public Flashcard(Guid ID, string question, string answer, DifficultyLevel difficultyLevel)
		{
			this.ID = ID;
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

