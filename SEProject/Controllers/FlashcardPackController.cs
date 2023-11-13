using Microsoft.AspNetCore.Mvc;
using SEProject.Models;
using SEProject.Services;

namespace SEProject.Controllers
{
    public class FlashcardPackController : Controller
    {
        private readonly IFlashcardPackDataHandler _flashcardPackDataHandler;
        private readonly IFlashcardIOService _flashcardIOService;
        private readonly ILoggingHandler _logger;

        public FlashcardPackController(IFlashcardPackDataHandler flashcardPackDataHandler, 
            IFlashcardIOService flashcardIOService, ILoggingHandler logger)
        {
            _flashcardPackDataHandler = flashcardPackDataHandler;
            _flashcardIOService = flashcardIOService;
            _logger = logger;
        }

        public async Task<IActionResult> CreateSampleFlashcardPack(string name)
        {
            try
            {
                // Get the list of all flashcard packs
                var allFlashcardPacks = await _flashcardPackDataHandler.LoadFlashcardPacksAsync();
                return View(allFlashcardPacks);
            }
            catch (Exception ex)
            {
                var logEntry = new LogEntry(
                        message: $"An error occurred while loading FlashcardPack with name {name}: {ex.Message}",
                        level: LogLevel.Error);
                _logger.Log(logEntry);
                return View();
            }
        }


        public async Task<IActionResult> ViewFlashcardPack(Guid id)
        {
            try
            {
                // Get the list of all flashcard packs
                var allFlashcardPacks = await _flashcardPackDataHandler.LoadFlashcardPacksAsync();
                var flashcardPackToView = allFlashcardPacks.FirstOrDefault(fpack => fpack.ID == id);
                return View(flashcardPackToView);
            }
            catch (Exception ex)
            {
                var logEntry = new LogEntry(
                        message: $"An error occurred while loading FlashcardPack with ID {id}: {ex.Message}",
                        level: LogLevel.Error);
                _logger.Log(logEntry);
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFlashcardPack(string name)
        {
            try
            {
                var newFlashcardPack = new FlashcardPack<Flashcard>
                (
                    name: name,
                    id: Guid.NewGuid(),
                    flashcards: new List<Flashcard>()
                );

                // Create a log entry with a custom message and log level (optional parameters)
                var logEntry = new LogEntry(
                    message: $"Flashcard pack with ID {newFlashcardPack.ID} was added",
                    level: LogLevel.Information
                );

                // Log the entry using the injected logger
                _logger.Log(logEntry);

                await _flashcardPackDataHandler.SaveFlashcardPackAsync(newFlashcardPack);

                return RedirectToAction("CreateSampleFlashcardPack");
            }
            catch (Exception ex)
            {
                // Handle the exception and log an error message with the exception details
                var logEntry = new LogEntry(
                        message: $"An error occurred while loading FlashcardPack with name {name}: {ex.Message}",
                        level: LogLevel.Error);
                _logger.Log(logEntry);

                // You can also handle the exception further or return an error view
                return View("Error", ex);
            }
        }


        [HttpPost]
        public async Task<IActionResult> AddFlashcardToPack(Flashcard viewModel, Guid id)
        {
            try
            {
                var flashcardPacks = await _flashcardPackDataHandler.LoadFlashcardPacksAsync();
                var flashcardPack = flashcardPacks.FirstOrDefault(fpack => fpack.ID == id);

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

                    await _flashcardIOService.SaveFlashcard(newFlashcard);

                    // Create a log entry for the successful addition
                    var logEntry = new LogEntry(
                        message: $"Flashcard with ID {newFlashcard.ID} was added to pack with ID {id}",
                        level: LogLevel.Information
                    );

                    _logger.Log(logEntry);

                    // Redirect to the view that displays the pack of flashcards
                    return RedirectToAction("ViewFlashcardPack", new { id = flashcardPack.ID });
                }

                // If the model is not valid, return to the form view with validation errors
                return View(flashcardPack);
            }
            catch (Exception ex)
            {
                // Handle the exception and log an error message with the exception details
                var logEntry = new LogEntry(
                        message: $"An error occurred while loading FlashcardPack with ID {id}: {ex.Message}",
                        level: LogLevel.Error);
                _logger.Log(logEntry);

                // You can also handle the exception further or return an error view
                return View("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFlashcardPack(Guid flashcardPackID)
        {
            await _flashcardPackDataHandler.RemoveFlashcardPackAsync(flashcardPackID);

            var logEntry = new LogEntry(message: $"Flashcard pack with ID {flashcardPackID} was removed");
            _logger.Log(logEntry);

            return RedirectToAction("CreateSampleFlashcardPack");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFlashcardFromPack(Guid flashcardID, Guid packID)
        {
            var flashcardPack = await _flashcardPackDataHandler.LoadFlashcardPackAsync(packID);

            var flashcardToRemove = flashcardPack.Flashcards.FirstOrDefault(flashcard => flashcard.ID == flashcardID);

            await _flashcardIOService.RemoveFlashcard(flashcardToRemove);

            var logEntry = new LogEntry(message: $"Flashcard with ID {flashcardID} was removed from pack with ID {packID}");
            _logger.Log(logEntry);

            // Redirect to the view that displays the pack of flashcards
            return RedirectToAction("ViewFlashcardPack", new { id = flashcardPack.ID });
        }

        [HttpGet]
        public async Task<IActionResult> EditFlashcard(Guid flashcardID)
        {
            try
            {
                // Get the list of all flashcard packs
                var flashcardPacks = await _flashcardPackDataHandler.LoadFlashcardPacksAsync();
                var flashcardPack = flashcardPacks.FirstOrDefault(p => p.Flashcards.Any(f => f.ID == flashcardID));
                var flashcardToEdit = flashcardPack.Flashcards.First(f => f.ID == flashcardID);
                return View(flashcardToEdit);
            }
            catch (Exception ex)
            {
                var logEntry = new LogEntry(
                        message: $"An error occurred while loading FlashcardPack with ID {flashcardID}: {ex.Message}",
                        level: LogLevel.Error);
                _logger.Log(logEntry);
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditFlashcard(Flashcard editedFlashcard)
        {
            var allFlashcardPacks = await _flashcardPackDataHandler.LoadFlashcardPacksAsync();
            var flashcardToEdit = allFlashcardPacks
                .SelectMany(p => p.Flashcards)
                .FirstOrDefault(f => f.ID == editedFlashcard.ID);

            if (ModelState.IsValid)
            {
                flashcardToEdit.Question = editedFlashcard.Question;
                flashcardToEdit.Answer = editedFlashcard.Answer;
                flashcardToEdit.Difficulty = editedFlashcard.Difficulty;

                await _flashcardIOService.SaveFlashcard(flashcardToEdit);

                // Redirect to the view that displays the flashcards
                return RedirectToAction("ViewFlashcardPack", new { id = flashcardToEdit.PackID });
            }

            var logEntry = new LogEntry(message: $"Flashcard with ID {editedFlashcard.ID} was edited");
            _logger.Log(logEntry);

            // If the model is not valid, return to the form view with validation errors
            return View(flashcardToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> EditFlashcardPackName(Guid id, string newName)
        {
            try
            {
                // Get the list of all flashcard packs
                var allFlashcardPacks = await _flashcardPackDataHandler.LoadFlashcardPacksAsync();
                // Find the flashcard pack with the specified ID
                var flashcardPackToEdit = allFlashcardPacks.FirstOrDefault(fpack => fpack.ID == id)!;
                if(newName != null)
                {
                    // Update the flashcard pack's name
                    flashcardPackToEdit.Name = newName;

                    // Save the updated flashcard pack
                    await _flashcardPackDataHandler.SaveFlashcardPackAsync(flashcardPackToEdit);
                }
                var logEntry = new LogEntry(message: $"Name of flashcard pack with ID {flashcardPackToEdit.ID} with was edited");
                _logger.Log(logEntry);

                // Redirect back to the page that displays the flashcard packs
                return RedirectToAction("CreateSampleFlashcardPack");
            }
            catch (Exception ex)
            {
                var logEntryError = new LogEntry(
                        message: $"An error occurred while loading FlashcardPack with ID {id}: {ex.Message}",
                        level: LogLevel.Error);
                _logger.Log(logEntryError);
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SortFlashcards(Guid flashcardPackID, string sortOption)
        {
            FlashcardComparer? comparer = null;
            var flashcardPack = await _flashcardPackDataHandler.LoadFlashcardPackAsync(flashcardPackID);
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

        [HttpPost]
        public IActionResult Present(Guid packID)
        {
            return RedirectToAction("PresentFlashcard", packID);
        }
    }
}