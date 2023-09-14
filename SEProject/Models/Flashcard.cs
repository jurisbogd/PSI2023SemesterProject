using System;
namespace SEProject.Models
{
	public class Flashcard
	{
		private string _question { get; set; }
		private string _answer { get; set; }
		private bool _isFavorite { get; set; } = false;
		private DateTime _creationDate = DateTime.Today;

		// Constructor for creating a flashcard
		public Flashcard(string question, string answer)
		{
			_question = question;
			_answer = answer;
		}

		// Edit flashcard method
		public void EditFlashcard(string newQuestion, string newAnswer)
		{
			_question = newQuestion;
			_answer = newAnswer;
		}

		// Switch favorite to opposite
		public void ToggleFavorite()
		{
			_isFavorite = !_isFavorite;
		}

        public override string ToString()
        {
			return $"{_question} - {_answer}";
        }
    }
}

