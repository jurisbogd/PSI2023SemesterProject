using Microsoft.AspNetCore.Mvc;
using System;
using SEProject.Models;
using SEProject.Services;

namespace SEProject.Controllers;

public class FlashcardController : Controller
{
    private readonly IFlashcardDataHandler _flashcardDataHandler;
    private readonly ILoggingHandler _LoggingHandler;

    public FlashcardController(IFlashcardDataHandler flashcardDataHandler, ILoggingHandler LoggingHandler)
    {
        _flashcardDataHandler = flashcardDataHandler;
        _LoggingHandler = LoggingHandler;
    }

    public IActionResult CreateSampleFlashcard() // NOTE: this will be executed every time you reload the page
    {

        List<Flashcard> allFlashcards = _flashcardDataHandler.LoadFlashcards();

        return View(allFlashcards);
    }
    
    // Display newly added flashcards
    [HttpPost]
    public IActionResult CreateSampleFlashcard(Flashcard viewModel)
    {
        List<Flashcard> allFlashcards = _flashcardDataHandler.LoadFlashcards();
        if (ModelState.IsValid)
        {
            // Create a new Flashcard object from the form data
            var newFlashcard = new Flashcard
            {
                ID = Guid.NewGuid(),
                Question = viewModel.Question,
                Answer = viewModel.Answer,
                Difficulty = viewModel.Difficulty
            };
            
            // Add the new flashcard to the list
            allFlashcards.Add(newFlashcard);

            // Save the updated list of flashcards to the JSON file
            _flashcardDataHandler.SaveFlashcard(newFlashcard);

            LogEntry newLogEntry = new LogEntry(DateTime.Now, "Flashcard was added: " + newFlashcard, LogLevel.Information);
            _LoggingHandler.Log(newLogEntry);
            // Redirect to the view that displays the flashcards
            return RedirectToAction("CreateSampleFlashcard");
        }

        // If the model is not valid, return to the form view
        return View(allFlashcards);
    }
    
    [HttpPost]
    public IActionResult RemoveSampleFlashcard(Guid ID)
    {
        _flashcardDataHandler.RemoveFlashcard(ID);
        LogEntry newLogEntry = new LogEntry(DateTime.Now, "Flashcard with ID:" + ID + " Was removed", LogLevel.Information);
        _LoggingHandler.Log(newLogEntry);
        // Redirect to the view that displays the flashcards
        return RedirectToAction("CreateSampleFlashcard");
    }

    [HttpGet]
    public IActionResult EditFlashcard(Guid id)
    {
        var flashcard = _flashcardDataHandler.LoadFlashcard(id);

        if (flashcard == null)
        {
            return NotFound();
        }
        
        return View(flashcard);
    }

    [HttpPost]
    public IActionResult EditFlashcard(Flashcard editedFlashcard)
    {
        List<Flashcard> allFlashcards = _flashcardDataHandler.LoadFlashcards();

        // Find the index of the flashcard to be edited
        int indexToEdit = allFlashcards.FindIndex(flashcard => flashcard.ID == editedFlashcard.ID);

        if (indexToEdit >= 0)
        {
            // Update the flashcard with the edited data
            allFlashcards[indexToEdit] = editedFlashcard;

            // Save the updated list of flashcards to the JSON file
            _flashcardDataHandler.SaveFlashcards(allFlashcards);

            LogEntry newLogEntry = new LogEntry(DateTime.Now, "Flashcard was edited: " + editedFlashcard, LogLevel.Information);
            _LoggingHandler.Log(newLogEntry);

            // Redirect to the view that displays the flashcards
            return RedirectToAction("CreateSampleFlashcard");
        }

        return NotFound(); // Flashcard not found, return a 404 response
    }

    [HttpPost]
    public IActionResult SortFlashcards(string sortOption)
    {
        FlashcardComparer comparer = null;
        List<Flashcard> sortedFlashcards = new List<Flashcard>();
        List<Flashcard> allFlashcards = _flashcardDataHandler.LoadFlashcards();

        // Checks what sort of comparison will be done and creates that type of object.
        switch (sortOption)
        {
            case "DateAsc":
            case "DateDesc":
                comparer = new FlashcardComparer(FlashcardComparer.ComparisonType.CreationDate);
                break;
            case "DifficultyAsc":
            case "DifficultyDesc":
                comparer = new FlashcardComparer(FlashcardComparer.ComparisonType.DifficultyLevel);
                break;
            default:
                sortedFlashcards = allFlashcards;
                break;
        }

        // Compares by Ascending or Descending, depending on sortOption ending.
        if (comparer != null)
        {
            if (sortOption.EndsWith("Asc"))
            {
                sortedFlashcards = allFlashcards.OrderBy(flashcard => flashcard, comparer).ToList();
            }
            else if (sortOption.EndsWith("Desc"))
            {
                sortedFlashcards = allFlashcards.OrderByDescending(flashcard => flashcard, comparer).ToList();
            }
        }

        LogEntry newLogEntry = new LogEntry(DateTime.Now, "Flashcards were sorted: ", LogLevel.Information);
        _LoggingHandler.Log(newLogEntry);
        return View("CreateSampleFlashcard", sortedFlashcards);
    }
}