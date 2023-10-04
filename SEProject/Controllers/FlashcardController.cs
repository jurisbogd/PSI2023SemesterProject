using Microsoft.AspNetCore.Mvc;
using System;
using SEProject.Models;
using SEProject.Services;

namespace SEProject.Controllers;

public class FlashcardController : Controller
{
    private readonly IWebHostEnvironment _env;
    private FlashcardService _flashcardService;

    public FlashcardController(IWebHostEnvironment env)
    {
        _env = env;
        _flashcardService = new FlashcardService();
    }

    public IActionResult CreateSampleFlashcard() // NOTE: this will be executed every time you reload the page
    {

        List<Flashcard> allFlashcards = _flashcardService.LoadFlashcards();

        return View(allFlashcards);
    }
    
    // Display newly added flashcards
    [HttpPost]
    public IActionResult CreateSampleFlashcard(Flashcard viewModel)
    {
        List<Flashcard> allFlashcards = _flashcardService.LoadFlashcards();
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
            allFlashcards.Add(newFlashcard);

            // Save the updated list of flashcards to the JSON file
            _flashcardService.SaveFlashcard(newFlashcard);
        
            // Redirect to the view that displays the flashcards
            return RedirectToAction("CreateSampleFlashcard");
        }

        // If the model is not valid, return to the form view
        return View(allFlashcards);
    }
    
    [HttpPost]
    public IActionResult RemoveSampleFlashcard(Guid ID)
    {
        _flashcardService.RemoveFlashcard(ID);
        
        // Redirect to the view that displays the flashcards
        return RedirectToAction("CreateSampleFlashcard");
    }

    [HttpGet]
    public IActionResult EditFlashcard(Guid id)
    {
        List<Flashcard> allFlashcards = _flashcardService.LoadFlashcards();
        Flashcard flashcardToEdit = allFlashcards.FirstOrDefault(flashcard => flashcard.ID == id);

        if (flashcardToEdit == null)
        {
            return NotFound();
        }

        return View(flashcardToEdit);
    }

    [HttpPost]
    public IActionResult EditFlashcard(Flashcard editedFlashcard)
    {
        List<Flashcard> allFlashcards = _flashcardService.LoadFlashcards();

        // Find the index of the flashcard to be edited
        int indexToEdit = allFlashcards.FindIndex(flashcard => flashcard.ID == editedFlashcard.ID);

        if (indexToEdit >= 0)
        {
            // Update the flashcard with the edited data
            allFlashcards[indexToEdit] = editedFlashcard;

            // Save the updated list of flashcards to the JSON file
            _flashcardService.SaveFlashcards(allFlashcards);

            // Redirect to the view that displays the flashcards
            return RedirectToAction("CreateSampleFlashcard");
        }

        return NotFound(); // Flashcard not found, return a 404 response
    }

    [HttpPost]
    public IActionResult SortFlashcards(string sortOption)
    {
        List<Flashcard> sortedFlashcards;
        List<Flashcard> allFlashcards = _flashcardService.LoadFlashcards(_env);

        switch (sortOption)
        {
            case "DateAsc":
                sortedFlashcards = allFlashcards.OrderBy(flashcard => flashcard.creationDate).ToList();
                break;
            case "DateDesc":
                sortedFlashcards = allFlashcards.OrderByDescending(flashcard => flashcard.creationDate).ToList();
                break;
            case "DifficultyAsc":
                sortedFlashcards = allFlashcards.OrderBy(flashcard => flashcard.difficultyLevel).ToList();
                break;
            case "DifficultyDesc":
                sortedFlashcards = allFlashcards.OrderByDescending(flashcard => flashcard.difficultyLevel).ToList();
                break;
            default:
                sortedFlashcards = allFlashcards;
                break;
        }

        return View("CreateSampleFlashcard", sortedFlashcards);
    }

}