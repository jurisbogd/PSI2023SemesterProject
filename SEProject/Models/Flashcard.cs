
namespace SEProject.Models
{
	public class Flashcard
	{
		public string question { get; set; }
		public string answer { get; set; }
		private bool IsFavorite { get; set; }
		private DateTime _creationDate;

		public Flashcard()
		{
			IsFavorite = false;
			_creationDate = DateTime.Today;
		}
		public Flashcard(string question, string answer)
		{
			this.question = question;
			this.answer = answer;
			IsFavorite = false;
			_creationDate = DateTime.Today;
		}

		// Edit flashcard method
		public void EditFlashcard(string newQuestion, string newAnswer)
		{
			question = newQuestion;
			answer = newAnswer;
		}

		// Switch favorite to opposite
		public void ToggleFavorite()
		{
			IsFavorite = IsFavorite;
		}

        public override string ToString()
        {
			return $"{question} - {answer}";
        }
    }
}

