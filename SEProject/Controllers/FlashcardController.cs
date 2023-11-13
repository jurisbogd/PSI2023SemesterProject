using Microsoft.AspNetCore.Mvc;
using SEProject.Models;
using SEProject.Services;

namespace SEProject.Controllers
{
    public class FlashcardController : Controller
    {
        private readonly IFlashcardIOService _flashcardIOService;

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

            Flashcard currentFlashcard = flashcards.FirstOrDefault(f => f.ID == currentFlashcardID);

            int currentFlashcardIndex = flashcards.IndexOf(currentFlashcard);

            if(currentFlashcardIndex >= 0 && currentFlashcardIndex < flashcards.Count() - 1)
            {
                return View("PresentFlashcard", flashcards[currentFlashcardIndex + 1]);
            }

            return RedirectToAction("PresentFlashard");
        }

        /*[HttpPost]
        public IActionResult RevealAnswer(Guid ID)
        {

        }*/

    }
}
