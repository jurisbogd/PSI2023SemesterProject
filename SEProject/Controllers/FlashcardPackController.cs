using Microsoft.AspNetCore.Mvc;
using System;
using SEProject.Models;
using SEProject.Services;

namespace SEProject.Controllers;

public class FlashcardPackController : Controller
{
    private readonly IWebHostEnvironment _env;
    private FlashcardPackService _flashcardPackService;

    public FlashcardPackController(IWebHostEnvironment env)
    {
        _env = env;
        _flashcardPackService = new FlashcardPackService();
    }
    public IActionResult CreateSampleFlashcardPack() // NOTE: this will be executed every time you reload the page
    {

        List<FlashcardPack> allFlaschardPacks = _flashcardPackService.LoadFlashcardPacks(_env);

        return View(allFlaschardPacks);
    }

    public IActionResult ViewFlashcardPack(Guid id)
    {
        List<FlashcardPack> allFlashcardPacks = _flashcardPackService.LoadFlashcardPacks(_env);
        FlashcardPack flashcardPackToView = allFlashcardPacks.FirstOrDefault(flashcardPack => flashcardPack.ID == id);

        return View(flashcardPackToView);
    }

    [HttpPost]
    public IActionResult AddFlashcardToPack(Flashcard viewModel, Guid id)
    {
        List<FlashcardPack> allFlashcardPacks = _flashcardPackService.LoadFlashcardPacks(_env);
        FlashcardPack flashcardPackToBeChanged = allFlashcardPacks.FirstOrDefault(flashcardPack => flashcardPack.ID == id);
        
        if (ModelState.IsValid)
        {
            // Create a new Flashcard object from the form data
            var newFlashcard = new Flashcard
            {
                ID = Guid.NewGuid(),
                question = viewModel.question,
                answer = viewModel.answer,
                difficultyLevel = viewModel.difficultyLevel
            };
            
            // Add the new flashcard to the list
            flashcardPackToBeChanged.flashcard.Add(newFlashcard);

            // Save the updated list of flashcards to the JSON file
            _flashcardPackService.SaveFlashcardPacks(allFlashcardPacks);
        
            // Redirect to the view that displays the flashcards
            return RedirectToAction("ViewFlashcardPack", new {id = flashcardPackToBeChanged.ID});
        }

        // If the model is not valid, return to the form view
        return View(flashcardPackToBeChanged);
    }

    [HttpPost]
    public IActionResult RemoveFlashcardFromPack(Guid id, Guid PackID)
    {
        List<FlashcardPack> allFlashcardPacks = _flashcardPackService.LoadFlashcardPacks(_env);
        FlashcardPack flashcardPackToBeChanged = allFlashcardPacks.FirstOrDefault(flashcardPack => flashcardPack.ID == PackID);

        // Remove flashcard from the list
        _flashcardPackService.RemoveFlashcard(id, flashcardPackToBeChanged);
        // Save the updaed JSON
        _flashcardPackService.SaveFlashcardPacks(allFlashcardPacks);
        
        // Redirect to the view that displays the flashcards
        return RedirectToAction("ViewFlashcardPack", new {id = flashcardPackToBeChanged.ID});
    }
    
}