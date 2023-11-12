using Microsoft.AspNetCore.Mvc;
using SEProject.Models;
using SEProject.Services;

namespace SEProject.Controllers
{
    public class FlashcardController : Controller
    {
        Func<FlashcardPack<Flashcard>, bool> FlashcardPackIDValidation = flashcardPack => flashcardPack.ID != Guid.Empty;
        private readonly IFlashcardPackDataHandler _flashcardPackDataHandler;
        private readonly ILoggingHandler _logger;

        public FlashcardController(IFlashcardPackDataHandler flashcardPackDataHandler, ILoggingHandler logger)
        {
            _flashcardPackDataHandler = flashcardPackDataHandler;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult AddFlashcardToPack(Flashcard viewModel, Guid id)
        {
            try
            {
                var allFlashcardPacks = _flashcardPackDataHandler.LoadFlashcardPacks();
                var flashcardPackToBeChanged = allFlashcardPacks.FirstOrDefault(fpack => fpack.ID == id);

                if (flashcardPackToBeChanged == null)
                {
                    return NotFound(); // Handle the case when the pack is not found
                }

                if (ModelState.IsValid)
                {
                    // Create a new Flashcard object from the form data
                    var newFlashcard = new Flashcard
                    {
                        PackID = id,
                        ID = Guid.NewGuid(),
                        Question = viewModel.Question,
                        Answer = viewModel.Answer,
                        Difficulty = viewModel.Difficulty
                    };

                    // Add the new flashcard to the pack
                    flashcardPackToBeChanged.Flashcards.Add(newFlashcard);

                    _flashcardPackDataHandler.SaveFlashcardPack(flashcardPackToBeChanged, FlashcardPackIDValidation);

                    // Create a log entry for the successful addition
                    var logEntry = new LogEntry(
                        message: $"Flashcard with ID {newFlashcard.ID} was added to pack with ID {id}",
                        level: LogLevel.Information
                    );

                    _logger.Log(logEntry);

                    // Redirect to the view that displays the pack of flashcards
                    return RedirectToAction("ViewFlashcardPack", new { id = flashcardPackToBeChanged.ID });
                }

                // If the model is not valid, return to the form view with validation errors
                return View(flashcardPackToBeChanged);
            }
            catch (Exception ex)
            {
                // Handle the exception and log an error message with the exception details
                var logEntry = new LogEntry(
                    message: "Error while adding a flashcard to the pack",
                    exception: ex,
                    level: LogLevel.Error
                );

                _logger.Log(logEntry);

                // You can also handle the exception further or return an error view
                return View("Error", ex);
            }
        }

        [HttpPost]
        public IActionResult RemoveFlashcardFromPack(Guid flashcardID, Guid packID)
        {
            var flashcardPack = _flashcardPackDataHandler.LoadFlashcardPack(packID);

            var indexToRemove = flashcardPack.Flashcards.FindIndex(flashcard => flashcard.ID == flashcardID);

            if (indexToRemove >= 0)
            {
                flashcardPack.Flashcards.RemoveAt(indexToRemove);

                _flashcardPackDataHandler.SaveFlashcardPack(flashcardPack, FlashcardPackIDValidation);
            }

            var logEntry = new LogEntry(message: $"Flashcard with ID {flashcardID} was removed from pack with ID {packID}");
            _logger.Log(logEntry);

            // Redirect to the view that displays the pack of flashcards
            return RedirectToAction("ViewFlashcardPack", new { id = flashcardPack.ID });
        }

        [HttpGet]
        public IActionResult EditFlashcard(Guid flashcardID)
        {
            var flashcardPack = _flashcardPackDataHandler.LoadFlashcardPacks().FirstOrDefault(p => p.Flashcards.Any(f => f.ID == flashcardID));
            if (flashcardPack == null)
            {
                return NotFound();
            }
            var flashcardToEdit = flashcardPack.Flashcards.First(f => f.ID == flashcardID);
            return View(flashcardToEdit);
        }

        [HttpPost]
        public IActionResult SortFlashcards(Guid flashcardPackID, string sortOption)
        {
            FlashcardComparer? comparer = null;
            var flashcardPack = _flashcardPackDataHandler.LoadFlashcardPack(flashcardPackID);
            var flashcardsInPack = flashcardPack.Flashcards;
            var sortedFlashcards = new List<Flashcard>();

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
                    sortedFlashcards = flashcardsInPack;
                    break;
            }

            // Compares by Ascending or Descending, depending on sortOption ending.
            if (comparer != null)
            {
                if (sortOption.EndsWith("Asc"))
                {
                    sortedFlashcards = flashcardsInPack.OrderBy(flashcard => flashcard, comparer).ToList();
                }
                else if (sortOption.EndsWith("Desc"))
                {
                    sortedFlashcards = flashcardsInPack.OrderByDescending(flashcard => flashcard, comparer).ToList();
                }
            }

            var logEntry = new LogEntry(message: $"Flashcards were sorted by sort option {sortOption}");
            _logger.Log(logEntry);

            var newPack = flashcardPack.CloneWithNewFlashcards(sortedFlashcards);

            return View("ViewFlashcardPack", newPack);
        }
    }
}
