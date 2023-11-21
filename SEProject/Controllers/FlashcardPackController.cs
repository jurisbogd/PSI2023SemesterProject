using Microsoft.AspNetCore.Mvc;
using SEProject.Models;
using SEProject.Services;
using SEProject.EventArguments;
using SEProject.EventServices;
using SEProject.Exceptions;

namespace SEProject.Controllers
{
    public class FlashcardPackController : Controller
    {
        private readonly IFlashcardPackDataHandler _flashcardPackDataHandler;
        private readonly IFlashcardIOService _flashcardIOService;
        private readonly ILoggingHandler _logger;
        private readonly IFlashcardPackEventService _flashcardPackEventService;
        private readonly IFlashcardEventService _flashcardEventService;
        Func<FlashcardPack, bool> FlashcardPackIDValidation = flashcardPack => flashcardPack.ID != Guid.Empty;
        Func<Flashcard, bool> FlashcardIDValidation = flashcard => flashcard.ID != Guid.Empty;

        public FlashcardPackController(IFlashcardPackDataHandler flashcardPackDataHandler, 
            IFlashcardIOService flashcardIOService, ILoggingHandler logger, 
            IFlashcardPackEventService flashcardPackEventService, IFlashcardEventService flashcardEventService)
        {
            _flashcardPackDataHandler = flashcardPackDataHandler;
            _flashcardIOService = flashcardIOService;
            _logger = logger;
            _flashcardPackEventService = flashcardPackEventService;
            _flashcardEventService = flashcardEventService;
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


        public async Task<IActionResult> ViewFlashcardPack(Guid id) {
            try {
                var pack = await _flashcardIOService.FetchFlashcardPack(id);
                return View(pack);
            }
            catch (Exception e) {
                _logger.Log(message: $"An error occurred while fetching flashcard pack with ID {id}.", exception: e, level: LogLevel.Error);
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFlashcardPack(string name)
        {
            try
            {
                var newFlashcardPack = new FlashcardPack
                (
                    name: name,
                    id: Guid.NewGuid(),
                    flashcards: new List<Flashcard>()
                );

                _flashcardPackDataHandler.FlashcardPackChanged += _flashcardPackEventService.OnFlashcardPackChanged;

                // Save the new flashcard (this will trigger the event)
                await _flashcardPackDataHandler.SaveFlashcardPackAsync(newFlashcardPack, FlashcardPackIDValidation);

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

        [HttpGet]
        public IActionResult AddFlashcard(Guid packID)
        {

            var newFlashcard = new Flashcard
            {
                PackID = packID,
                ID = Guid.NewGuid(),
                Question = "Question",
                Answer = "Answer",
                Difficulty = 0
            };
            return View(newFlashcard);
        }

        [HttpPost]
        public async Task<IActionResult> AddFlashcardToPack(Flashcard flashcard)
        {
            var packID = flashcard.PackID;

            try
            {
                var flashcardPacks = await _flashcardPackDataHandler.LoadFlashcardPacksAsync();
                var flashcardPack = flashcardPacks.FirstOrDefault(fpack => fpack.ID == packID);

                if (ModelState.IsValid)
                {
                    // Create a new Flashcard object from the form data
                    _flashcardIOService.FlashcardChanged += _flashcardEventService.OnFlashcardChanged;
                    // Save the new flashcard (this will trigger the event)
                    await _flashcardIOService.SaveFlashcard(flashcard, FlashcardIDValidation);

                    // Redirect to the view that displays the pack of flashcards
                    return RedirectToAction("ViewFlashcardPack", new { id = flashcardPack!.ID });
                }

                // If the model is not valid, return to the form view with validation errors
                return View(flashcardPack);
            }
            catch (Exception ex)
            {
                // Handle the exception and log an error message with the exception details
                var logEntry = new LogEntry(
                        message: $"An error occurred while loading FlashcardPack with ID {packID}: {ex.Message}",
                        level: LogLevel.Error);
                _logger.Log(logEntry);

                // You can also handle the exception further or return an error view
                return View("Error", ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFlashcardPack(Guid flashcardPackID)
        {
            // Subscribing to event
            _flashcardPackDataHandler.FlashcardPackChanged += _flashcardPackEventService.OnFlashcardPackChanged;

            try
            {
                await _flashcardPackDataHandler.RemoveFlashcardPackAsync(flashcardPackID);
            }catch (FlashcardPackNotFoundException fpnfe)
            {
                var logEntry = new LogEntry(
                        message: $"An error occurred while removing FlashcardPack with ID {flashcardPackID}: {fpnfe.Message}",
                        level: LogLevel.Error);
                _logger.Log(logEntry);
                return BadRequest($"FlashcardPack with ID {flashcardPackID} not found.");
            }

            return RedirectToAction("CreateSampleFlashcardPack");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFlashcardFromPack(Guid flashcardID, Guid packID) {
            try {
                await _flashcardIOService.RemoveFlashcardFromPack(packID, flashcardID);
            }
            catch (FlashcardPackNotFoundException e) {
                _logger.Log(message: $"An error occurred while removing flashcard with ID {flashcardID} from pack with ID {packID}", exception: e, level: LogLevel.Error);
                return BadRequest($"Flashcard pack with ID {packID} not found.");
            }
            catch (FlashcardNotFoundException e) {
                _logger.Log(message: $"An error occurred while removing flashcard with ID {flashcardID} from pack with ID {packID}", exception: e, level: LogLevel.Error);
                return BadRequest($"Flashcard with ID {flashcardID} not found.");
            }
            catch (ArgumentException e) {
                _logger.Log(message: $"An error occurred while removing flashcard with ID {flashcardID} from pack with ID {packID}", exception: e, level: LogLevel.Error);
                return BadRequest($"Flashcard with ID {flashcardID} does not belong to pack with ID {packID}.");
            }

            // Redirect to the view that displays the pack of flashcards
            return RedirectToAction("ViewFlashcardPack", new { id = packID });
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
                flashcardToEdit.IsFavorite = editedFlashcard.IsFavorite;

                _flashcardIOService.FlashcardChanged += _flashcardEventService.OnFlashcardChanged;
                // Save the new flashcard (this will trigger the event)
                await _flashcardIOService.SaveFlashcard(flashcardToEdit, FlashcardIDValidation);

                // Redirect to the view that displays the flashcards
                return RedirectToAction("ViewFlashcardPack", new { id = flashcardToEdit.PackID });
            }

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
                    var oldName = flashcardPackToEdit.Name;
                    // Update the flashcard pack's name
                    flashcardPackToEdit.Name = newName;

                    _flashcardPackDataHandler.FlashcardPackChanged += _flashcardPackEventService.OnFlashcardPackChanged;

                    // Save the new flashcard (this will trigger the event)
                    await _flashcardPackDataHandler.SaveFlashcardPackAsync(flashcardPackToEdit);
                }

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