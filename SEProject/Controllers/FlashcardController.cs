using Microsoft.AspNetCore.Mvc;
using SEProject.Models;
using SEProject.Services;

namespace SEProject.Controllers;

public class FlashcardController : Controller
{
    private readonly IWebHostEnvironment _env;
    private FlashcardService _flashcards;

    public FlashcardController(IWebHostEnvironment env)
    {
        _env = env;
        _flashcards = new FlashcardService();
    }

    public IActionResult CreateSampleFlashcard() // NOTE: this will be executed every time you reload the page
    {
        List<Flashcard> allFlashcards = _flashcards.LoadFlashcards("flashcards.json", _env);

        return View(allFlashcards);
    }

    
}