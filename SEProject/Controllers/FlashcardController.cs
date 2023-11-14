using Microsoft.AspNetCore.Mvc;
using SEProject.Models;
using SEProject.Services;

namespace SEProject.Controllers
{
    public class FlashcardController : Controller
    {
        private readonly IFlashcardIOService _flashcardIOService;
        private Func<Flashcard, bool> FlashcardIDValidation = flashcard => flashcard.ID != Guid.Empty;

        public FlashcardController(IFlashcardIOService flashcardIOService)
        {
            _flashcardIOService = flashcardIOService;
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
        public async Task<ActionResult> ToggleFavorite(Guid packID, Guid currentFlashcardID, bool isFavorite)
        {
            List<Flashcard> flashcards = await _flashcardIOService.LoadFlashcardsAsync(packID);

            Flashcard currentFlashcard = flashcards.FirstOrDefault(f => f.ID == currentFlashcardID)!;

            currentFlashcard.IsFavorite = isFavorite;

            await _flashcardIOService.SaveFlashcard(currentFlashcard, FlashcardIDValidation);

            // Perform any logic related to toggling favorite status here

            return RedirectToAction("PresentNextFlashcard", new { packID = packID, currentFlashcardID = currentFlashcardID });
        }
    }
}
