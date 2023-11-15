using Microsoft.AspNetCore.Mvc;
using SEProject.Models;
using SEProject.Services;
using SEProject.EventArguments;
using SEProject.EventServices;

namespace SEProject.Controllers
{
    public class FlashcardController : Controller
    {
        private readonly IFlashcardIOService _flashcardIOService;
        private readonly IFlashcardEventService _flashcardEventService;
        private Func<Flashcard, bool> FlashcardIDValidation = flashcard => flashcard.ID != Guid.Empty;

        public FlashcardController(IFlashcardIOService flashcardIOService, IFlashcardEventService flashcardEventService)
        {
            _flashcardIOService = flashcardIOService;
            _flashcardEventService = flashcardEventService;
        }

        [HttpPost]
        public async Task<ActionResult> PresentFlashcard(Guid packID)
        {
            List<Flashcard> flashcards = await _flashcardIOService.LoadFlashcardsAsync(packID);

            return View("PresentFlashcard", flashcards.First());
        }

        [HttpPost]
        public async Task<ActionResult> PresentNextFlashcard(Guid packID, Guid currentFlashcardID)
        {
            List<Flashcard> flashcards = await _flashcardIOService.LoadFlashcardsAsync(packID);

            Flashcard currentFlashcard = flashcards.FirstOrDefault(f => f.ID == currentFlashcardID)!;

            int currentFlashcardIndex = flashcards.IndexOf(currentFlashcard);

            if(currentFlashcardIndex >= 0 && currentFlashcardIndex < flashcards.Count() - 1)
            {
                return View("PresentFlashcard", flashcards[currentFlashcardIndex + 1]);
            }
            // Return to flashcardPacks page if there are no more flashcards to present
            else
            {
                return RedirectToAction("ViewFlashcardPack", "FlashcardPack", new { id = packID });
            }
        }

        [HttpPost]
        public async Task<JsonResult> ToggleFavorite(Guid packID, Guid currentFlashcardID, bool isFavorite)
        {
            List<Flashcard> flashcards = await _flashcardIOService.LoadFlashcardsAsync(packID);

            Flashcard currentFlashcard = flashcards.FirstOrDefault(f => f.ID == currentFlashcardID)!;

            currentFlashcard.IsFavorite = isFavorite;

            _flashcardIOService.FlashcardChanged += _flashcardEventService.OnFlashcardChanged;
            await _flashcardIOService.SaveFlashcard(currentFlashcard, FlashcardIDValidation);

            return new JsonResult(Ok());
        }
    }
}
