using Microsoft.AspNetCore.Mvc;
using SEProject.Models;
using SEProject.Services;

namespace SEProject.Controllers
{
    public class FlashcardPackController : Controller
    {
        private readonly IFlashcardPackDataHandler _flashcardPackDataHandler;
        private readonly ILoggingHandler _logger;

        public FlashcardPackController(IFlashcardPackDataHandler flashcardPackDataHandler, ILoggingHandler logger)
        {
            _flashcardPackDataHandler = flashcardPackDataHandler;
            _logger = logger;
        }

        public IActionResult CreateSampleFlashcardPack(string name)
        {
            var allFlashcardPacks = _flashcardPackDataHandler.LoadFlashcardPacks();

            return View(allFlashcardPacks);
        }


        public IActionResult ViewFlashcardPack(Guid id)
        {
            var allFlashcardPacks = _flashcardPackDataHandler.LoadFlashcardPacks();
            var flashcardPackToView = allFlashcardPacks.FirstOrDefault(fpack => fpack.ID == id);

            if (flashcardPackToView == null)
            {
                return NotFound(); // Handle the case when the pack is not found
            }

            return View(flashcardPackToView);
        }

        [HttpPost]
        public IActionResult AddFlashcardPack(string name)
        {
            var newFlashcardPack = new FlashcardPack<Flashcard>(
                name: name,
                id: Guid.NewGuid(),
                flashcards: new List<Flashcard>());
            
            var logEntry = new LogEntry(message: "Flashcard pack was added: " + newFlashcardPack.ID);
            _logger.Log(logEntry);

            _flashcardPackDataHandler.SaveFlashcardPack(newFlashcardPack);

            return RedirectToAction("CreateSampleFlashcardPack");
        }


        [HttpPost]
        public IActionResult AddFlashcardToPack(Flashcard viewModel, Guid id)
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

                // Save the updated pack of flashcards to the JSON file
                _flashcardPackDataHandler.SaveFlashcardPack(flashcardPackToBeChanged);

                // Redirect to the view that displays the pack of flashcards
                return RedirectToAction("ViewFlashcardPack", new { id = flashcardPackToBeChanged.ID });
            }

            // If the model is not valid, return to the form view with validation errors
            return View(flashcardPackToBeChanged);
        }

        [HttpPost]
        public IActionResult RemoveFlashcardPack(Guid flashcardPackID)
        {
            _flashcardPackDataHandler.RemoveFlashcardPack(flashcardPackID);

            var logEntry = new LogEntry(message: "Flashcard pack was removed: " + flashcardPackID);
            _logger.Log(logEntry);

            return RedirectToAction("CreateSampleFlashcardPack");
        }

        [HttpPost]
        public IActionResult RemoveFlashcardFromPack(Guid flashcardID, Guid packID)
        {
            var flashcardPack = _flashcardPackDataHandler.LoadFlashcardPack(packID);

            var indexToRemove = flashcardPack.Flashcards.FindIndex(flashcard => flashcard.ID == flashcardID);

            if (indexToRemove >= 0)
            {
                flashcardPack.Flashcards.RemoveAt(indexToRemove);

                _flashcardPackDataHandler.SaveFlashcardPack(flashcardPack);
            }

            var logEntry = new LogEntry(message: "Flashcard with ID:" + flashcardID + " was removed from:" + packID + "pack");
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
        public IActionResult EditFlashcard(Flashcard editedFlashcard)
        {
            var allFlashcardPacks = _flashcardPackDataHandler.LoadFlashcardPacks();
            var flashcardToEdit = allFlashcardPacks.SelectMany(p => p.Flashcards).FirstOrDefault(f => f.ID == editedFlashcard.ID);

            if (flashcardToEdit == null)
            {
                return NotFound(); // Flashcard not found, return a 404 response
            }

            if (ModelState.IsValid)
            {
                flashcardToEdit.Question = editedFlashcard.Question;
                flashcardToEdit.Answer = editedFlashcard.Answer;
                flashcardToEdit.Difficulty = editedFlashcard.Difficulty;

                _flashcardPackDataHandler.SaveFlashcardPacks(allFlashcardPacks);

                // Redirect to the view that displays the flashcards
                return RedirectToAction("ViewFlashcardPack", new { id = flashcardToEdit.PackID });
            }

            var logEntry = new LogEntry(message: "Flashcard was edited: " + editedFlashcard);
            _logger.Log(logEntry);

            // If the model is not valid, return to the form view with validation errors
            return View(flashcardToEdit);
        }

        [HttpPost]
        public IActionResult EditFlashcardPackName(Guid id, string newName)
        {
            // Get the list of all flashcard packs
            var allFlashcardPacks = _flashcardPackDataHandler.LoadFlashcardPacks();

            // Find the flashcard pack with the specified ID
            var flashcardPackToEdit = allFlashcardPacks.FirstOrDefault(fpack => fpack.ID == id)!;

            if (flashcardPackToEdit == null)
            {
                return NotFound(); // Handle the case when the pack is not found
            }

            if(newName != null)
            {
                // Update the flashcard pack's name
                flashcardPackToEdit.Name = newName;

                // Save the updated flashcard pack
                _flashcardPackDataHandler.SaveFlashcardPack(flashcardPackToEdit);
            }

            var logEntry = new LogEntry(message: "Flashcard pack name was edited: " + flashcardPackToEdit);
            _logger.Log(logEntry);

            // Redirect back to the page that displays the flashcard packs
            return RedirectToAction("CreateSampleFlashcardPack");
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

            var logEntry = new LogEntry(message: "Flashcards were sorted");
            _logger.Log(logEntry);

            var newPack = flashcardPack.CloneWithNewFlashcards(sortedFlashcards);

            return View("ViewFlashcardPack", newPack);
        }
    }
}
