using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SEProject.Models;
using SEProject.Services;
using SEProject.EventArguments;
using SEProject.EventServices;

namespace SEProject.Controllers
{
    public class FlashcardController : Controller
    {
        private Func<Flashcard, bool> FlashcardIDValidation = flashcard => flashcard.ID != Guid.Empty;
        
        [HttpPost]
        public async Task<ActionResult> PresentFlashcard(Guid packID)
        {
            using (var httpClient = new HttpClient())
            {
                var apiEndpoint = $"http://localhost:5123/api/Flashcard/GetFlashcards?packID={packID}";

                var response = await httpClient.GetAsync(apiEndpoint);
                
                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseContent);

                    if (!string.IsNullOrEmpty(responseContent))
                    {
                        // Deserialize the JSON array into a List<Flashcard>
                        var flashcards = JsonConvert.DeserializeObject<List<Flashcard>>(responseContent);

                        if (flashcards != null && flashcards.Any())
                        {
                            return View("PresentFlashcard", flashcards.First());
                        }
                    }
                }
                
                return View("Error");
            }
        }
        
        [HttpPost]
        public async Task<ActionResult> PresentNextFlashcard(Guid packID, Guid currentFlashcardID)
        {
            using (var httpClient = new HttpClient())
            {
                var apiEndpoint = $"http://localhost:5123/api/Flashcard/GetNextFlashcard?packID={packID}&currentFlashcardID={currentFlashcardID}";
        
                var response = await httpClient.GetAsync(apiEndpoint);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
        
                    if (!string.IsNullOrEmpty(responseContent))
                    {
                        // Deserialize the JSON response into a Flashcard
                        var nextFlashcard = JsonConvert.DeserializeObject<Flashcard>(responseContent);
        
                        return View("PresentFlashcard", nextFlashcard);
                    }
                    return RedirectToAction("ViewFlashcardPack", "FlashcardPack", new { id = packID });
                }
                return View("Error");
            }
        }
        
        [HttpPost]
        public async Task<JsonResult> ToggleFavorite(Guid packID, Guid currentFlashcardID, bool isFavorite)
        {
            using (var httpClient = new HttpClient())
            {
                // Assuming your API is hosted at http://localhost:5123
                var apiEndpoint = $"http://localhost:5123/api/Flashcard/ToggleFavorite?packID={packID}&flashcardID={currentFlashcardID}&isFavorite={isFavorite}";

                var response = await httpClient.PostAsync(apiEndpoint, null);

                if (response.IsSuccessStatusCode)
                {
                    // If the API returns a success status code, you can handle it accordingly
                    return new JsonResult("Success");
                }
                return new JsonResult("Error");
            }
        }
    }
}
