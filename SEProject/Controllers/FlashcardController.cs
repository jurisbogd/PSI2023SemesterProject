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
            new Flashcard { question = "What is ASP.NET?", answer = "A web framework" },
            new Flashcard { question = "What is C#?", answer = "A programming language" },
            new Flashcard { question = "What is MVC?", answer = "A design pattern" }
        };

        
        return View(flashcards);
    }
}