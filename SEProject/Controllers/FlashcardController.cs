using Microsoft.AspNetCore.Mvc;
using SEProject.Models;

namespace SEProject.Controllers;

public class FlashcardController : Controller
{
    public IActionResult CreateSampleFlashcard()
    {
        // Create sample flashcards
        var flashcards = new List<Flashcard>
        {
            new Flashcard { question = "What is ASP.NET?", answer = "A web framework", 
            difficultyLevel = Flashcard.DifficultyLevel.Easy},

            new Flashcard { question = "What is C#?", answer = "A programming language", 
            difficultyLevel = Flashcard.DifficultyLevel.Medium},

            new Flashcard { question = "What is MVC?", answer = "A design pattern",
            difficultyLevel = Flashcard.DifficultyLevel.Hard}
        };

        
        return View(flashcards);
    }
}