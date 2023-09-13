using System;
namespace SEProject.Models
{
	public class Flashcard
	{
		private string _question;
		private string _answer;
		private bool _isFavorite;
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

        public string Question
        {
            get { return _question; }
            set { _question = value; }
        }

        public string Answer
        {
            get { return _answer; }
            set { _answer = value; }
        }

        public override string ToString()
        {
			return $"{_question} - {_answer}";
        }
    }
}

