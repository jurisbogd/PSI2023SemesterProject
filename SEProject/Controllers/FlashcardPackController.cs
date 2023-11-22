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
        private readonly IFlashcardDataService _flashcardDataService;
        private readonly IFlashcardPackDataService _flashcardPackDataService;

        private readonly IFlashcardIOService _flashcardIOService;
        private readonly ILoggingHandler _logger;
        private readonly IFlashcardPackEventService _flashcardPackEventService;
        private readonly IFlashcardEventService _flashcardEventService;
        Func<FlashcardPack, bool> FlashcardPackIDValidation = flashcardPack => flashcardPack.ID != Guid.Empty;
        Func<Flashcard, bool> FlashcardIDValidation = flashcard => flashcard.ID != Guid.Empty;

        public FlashcardPackController(
            IFlashcardIOService flashcardIOService, ILoggingHandler logger, 
            IFlashcardPackEventService flashcardPackEventService, IFlashcardEventService flashcardEventService,
            IFlashcardDataService flashcardData,
            IFlashcardPackDataService flashcardPackData
        ) {
            _flashcardDataService = flashcardData;
            _flashcardPackDataService = flashcardPackData;

            _flashcardIOService = flashcardIOService;
            _logger = logger;
            _flashcardPackEventService = flashcardPackEventService;
            _flashcardEventService = flashcardEventService;

            _flashcardDataService.FlashcardChanged += _flashcardEventService.OnFlashcardChanged;
            _flashcardPackDataService.FlashcardPackChanged += _flashcardPackEventService.OnFlashcardPackChanged;
        }

        public async Task<IActionResult> CreateSampleFlashcardPack(string name) {
            var packs = await _flashcardPackDataService.FetchSampleFlashcardPacks();
            return View(packs);
        }


        public async Task<IActionResult> ViewFlashcardPack(Guid id) {
            try {
                var pack = await _flashcardPackDataService.FetchFlashcardPack(id);
                return View(pack);
            }
            catch (Exception e) {
                _logger.Log(message: $"An error occurred while fetching flashcard pack with ID {id}.", exception: e, level: LogLevel.Error);
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddFlashcardPack(string name) {
            try {
                var newFlashcardPack = new FlashcardPack (
                    name: name,
                    id: Guid.NewGuid(),
                    flashcards: new List<Flashcard>()
                );
                
                await _flashcardPackDataService.SaveFlashcardPack(newFlashcardPack);
                return RedirectToAction("CreateSampleFlashcardPack");
            }
            catch (Exception e) {
                _logger.Log(message: $"An error occurred while loading FlashcardPack with name {name}.", exception: e, level: LogLevel.Error);
                return View("Error", e);
            }
        }

        [HttpGet]
        public IActionResult AddFlashcard(Guid packID) {
            var newFlashcard = new Flashcard {
                PackID = packID,
                ID = Guid.NewGuid(),
                Question = "Question",
                Answer = "Answer",
                Difficulty = 0
            };

            return View(newFlashcard);
        }

        [HttpPost]
        public async Task<IActionResult> AddFlashcardToPack(Flashcard flashcard) {
            var packID = flashcard.PackID;

            try {
                var flashcardPack = await _flashcardPackDataService.FetchFlashcardPack(packID);

                if (ModelState.IsValid) {
                    await _flashcardDataService.SaveFlashcard(flashcard);

                    _logger.Log($"Added flashcard with ID {flashcard.ID} to pack with ID {flashcard.PackID}");

                    // Redirect to the view that displays the pack of flashcards
                    return RedirectToAction("ViewFlashcardPack", new { id = flashcard.PackID });
                }

                // If the model is not valid, return to the form view with validation errors
                return View(flashcardPack);
            }
            catch (Exception e) {
                _logger.Log(message: $"An error occurred while loading FlashcardPack with ID {packID}", exception: e, level: LogLevel.Error);

                // You can also handle the exception further or return an error view
                return View("Error", e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFlashcardPack(Guid flashcardPackID) {
            try {
                await _flashcardPackDataService.DeleteFlashcardPack(flashcardPackID);
            }
            catch (FlashcardPackNotFoundException e) {
                _logger.Log(message: $"An error occurred while removing FlashcardPack with ID {flashcardPackID}.", exception: e, level: LogLevel.Error);
                return BadRequest($"FlashcardPack with ID {flashcardPackID} not found.");
            }

            return RedirectToAction("CreateSampleFlashcardPack");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFlashcardFromPack(Guid flashcardID, Guid packID) {
            try {
                await _flashcardDataService.DeleteFlashcard(flashcardID);
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
        public async Task<IActionResult> EditFlashcard(Guid flashcardID) {
            try {
                var flashcard = await _flashcardDataService.FetchFlashcard(flashcardID);
                return View(flashcard);
            }
            catch (Exception e) {
                _logger.Log(message: $"An error occurred while loading FlashcardPack with ID {flashcardID}.", exception: e, level: LogLevel.Error);
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> EditFlashcard(Flashcard flashcard) {
            if (ModelState.IsValid) {
                var editedFlashcard = await _flashcardDataService.FetchFlashcard(flashcard.ID);
                editedFlashcard.Question = flashcard.Question;
                editedFlashcard.Answer = flashcard.Answer;
                editedFlashcard.Difficulty = flashcard.Difficulty;
                editedFlashcard.IsFavorite = flashcard.IsFavorite;

                // Save the new flashcard (this will trigger the event)
                await _flashcardDataService.SaveFlashcard(editedFlashcard);

                // Redirect to the view that displays the flashcards
                return RedirectToAction("ViewFlashcardPack", new { id = editedFlashcard.PackID });
            }

            return View(flashcard);
        }

        [HttpPost]
        public async Task<IActionResult> EditFlashcardPackName(Guid id, string newName) {
            try {
                var pack = await _flashcardPackDataService.FetchFlashcardPack(id);
                if(newName != null) {
                    var oldName = pack.Name;
                    pack.Name = newName;
                    // Save the new flashcard (this will trigger the event)
                    await _flashcardPackDataService.SaveFlashcardPack(pack);
                }

                return RedirectToAction("CreateSampleFlashcardPack");
            }
            catch (Exception e) {
                _logger.Log(message: $"An error occurred while loading FlashcardPack with ID {id}.", exception: e, level: LogLevel.Error);
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> SortFlashcards(Guid flashcardPackID, string sortOption) {
            FlashcardComparer? comparer = null;
            var flashcardPack = await _flashcardPackDataService.FetchFlashcardPack(flashcardPackID);
            var flashcardsInPack = flashcardPack.Flashcards;
            var sortedFlashcards = new List<Flashcard>();

            // Checks what sort of comparison will be done and creates that type of object.
            switch (sortOption) {
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
            if (comparer != null) {
                if (sortOption.EndsWith("Asc"))
                {
                    sortedFlashcards = flashcardsInPack.OrderBy(flashcard => flashcard, comparer).ToList();
                }
                else if (sortOption.EndsWith("Desc"))
                {
                    sortedFlashcards = flashcardsInPack.OrderByDescending(flashcard => flashcard, comparer).ToList();
                }
            }

            _logger.Log(message: $"Flashcards were sorted by sort option {sortOption}");

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