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
    
    // Display newly added flashcards
    [HttpPost]
    public IActionResult CreateSampleFlashcard(Flashcard viewModel)
    {
        List<Flashcard> allFlashcards = _flashcards.LoadFlashcards("flashcards.json", _env);
        if (ModelState.IsValid)
        {
            // Create a new Flashcard object from the form data
            var newFlashcard = new Flashcard
            {
                question = viewModel.question,
                answer = viewModel.answer,
                difficultyLevel = viewModel.difficultyLevel
            };
            
            // Add the new flashcard to the list
            allFlashcards.Add(newFlashcard);
            Console.WriteLine(allFlashcards.Count);

            // Save the updated list of flashcards to the JSON file
            _flashcards.SaveFlashcards("flashcards.json", allFlashcards);
        
            // Redirect to the view that displays the flashcards
            return RedirectToAction("CreateSampleFlashcard");
        }

        // If the model is not valid, return to the form view
        return View(allFlashcards);
    }
}