using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using SEProject.Models;
using SEProject.Services;
using SEProject.EventServices;
using SEProject.Exceptions;
using System.Net.Http;
using System.Text.Json;
using System.Collections.Generic;

namespace SEProject.Controllers
{
    public class FlashcardPackController : Controller
    {
        private readonly ILoggingHandler _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        Func<FlashcardPack, bool> FlashcardPackIDValidation = flashcardPack => flashcardPack.ID != Guid.Empty;
        Func<Flashcard, bool> FlashcardIDValidation = flashcard => flashcard.ID != Guid.Empty;

        public FlashcardPackController(ILoggingHandler logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> CreateSampleFlashcardPack(string name) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try {
                using var httpClient = _httpClientFactory.CreateClient();

                var flashcardPacks = await httpClient.GetFromJsonAsync<List<FlashcardPack>>(
                    $"http://localhost:5123/api/FlashcardPack/GetFlashcardPacks?userId={userId}",
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));

                return View(flashcardPacks);
            }
            catch (Exception ex) {
                var logEntry = new LogEntry(
                    message: $"An error occurred while loading FlashcardPacks for user {name}: {ex.Message}",
                    level: LogLevel.Error);
                _logger.Log(logEntry);
                return View(ex);
            }
        }
        
        public async Task<IActionResult> ViewFlashcardPack(Guid id) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            try{
                using var httpClient = _httpClientFactory.CreateClient();

                var flashcardPack = await httpClient.GetFromJsonAsync<FlashcardPack>(
                    $"http://localhost:5123/api/FlashcardPack/GetFlashcardPack?packID={id}&userId={userId}",
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));

                return View(flashcardPack);
            }
            catch (Exception ex) {
                var logEntry = new LogEntry(
                    message: $"An error occurred while loading FlashcardPack with ID {id}: {ex.Message}",
                    level: LogLevel.Error);
                _logger.Log(logEntry);

                return NotFound();
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> AddFlashcardPack(string name) {
            var newFlashcardPack = new FlashcardPack (
                name: name,
                id: Guid.NewGuid(),
                flashcards: new List<Flashcard>()
            );

            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            try {
                using var httpClient = _httpClientFactory.CreateClient();

                await httpClient.PostAsJsonAsync(
                    $"http://localhost:5123/api/FlashcardPack/AddFlashcardPack?userID={userID}",
                    newFlashcardPack);
                
                return RedirectToAction("CreateSampleFlashcardPack");
            }
            catch (Exception ex) {
                var logEntry = new LogEntry(
                    message: $"An error occurred while loading FlashcardPack with name {name}: {ex.Message}",
                    level: LogLevel.Error);
                _logger.Log(logEntry);

                return NotFound();
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
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            try {
                if (ModelState.IsValid) {
                    /*
                    if (!Regex.IsMatch(flashcard.Question.TrimEnd(), @"\?$")) {
                        TempData["AlertMessage"] = "The flashcard question must end with a question mark.";
                        return RedirectToAction("ViewFlashcardPack", new { id = flashcard.PackID });
                    }
                    */

                    using var httpClient = _httpClientFactory.CreateClient();

                    await httpClient.PostAsJsonAsync(
                        $"http://localhost:5123/api/FlashcardPack/AddFlashcardToPack?userID={userId}",
                        flashcard);
                }

                return RedirectToAction("ViewFlashcardPack", new { id = flashcard.PackID });
            }
            catch (Exception ex) {
                var logEntry = new LogEntry(
                        message: $"An error occurred while adding a flashcard to FlashcardPack with ID {packID}: {ex.Message}",
                        level: LogLevel.Error);
                _logger.Log(logEntry);
                return View("Error", ex);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveFlashcardPack(Guid flashcardPackID) {
            try {
                using var httpClient = _httpClientFactory.CreateClient();
            
                await httpClient.DeleteAsync(
                    $"http://localhost:5123/api/FlashcardPack/RemoveFlashcardPack?packID={flashcardPackID}");
                
                return RedirectToAction("CreateSampleFlashcardPack");
            }
            catch (Exception ex) {
                var logEntry = new LogEntry(
                    message: $"An error occurred while removing FlashcardPack with ID {flashcardPackID}: {ex.Message}",
                    level: LogLevel.Error);
                _logger.Log(logEntry);
                return NotFound();
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveFlashcardFromPack(Guid flashcardID, Guid packID) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            try {
                using var httpClient = _httpClientFactory.CreateClient();

                await httpClient.DeleteAsync(
                    $"http://localhost:5123/api/FlashcardPack/RemoveFlashcardFromPack?flashcardID={flashcardID}&packID={packID}&userID={userId}");
            
                return RedirectToAction("ViewFlashcardPack", new { id = packID });
            }
            catch {
                var logEntry = new LogEntry(
                    message: $"An error occurred while removing Flashcard with ID {flashcardID}.",
                    level: LogLevel.Error);
                _logger.Log(logEntry);
                return BadRequest($"Failed to remove Flashcard with ID {flashcardID}");
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> EditFlashcard(Guid flashcardID) {
            var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            try {
                using var httpClient = _httpClientFactory.CreateClient();
            
                var flashcardToEdit = await httpClient.GetFromJsonAsync<Flashcard>(
                    $"http://localhost:5123/api/FlashcardPack/GetFlashcard?flashcardID={flashcardID}&userID={userID}",
                    new JsonSerializerOptions(JsonSerializerDefaults.Web));
                
                return View(flashcardToEdit);
            }
            catch (Exception ex) {
                var logEntry = new LogEntry(
                    message: $"An error occurred while loading Flashcard with ID {flashcardID}: {ex.Message}",
                    level: LogLevel.Error);
                _logger.Log(logEntry);
                return NotFound();
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> EditFlashcard(Flashcard editedFlashcard) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            try {
                using var httpClient = _httpClientFactory.CreateClient();

                var response = await httpClient.PostAsJsonAsync(
                    $"http://localhost:5123/api/FlashcardPack/EditFlashcard?userID={userId}",
                    editedFlashcard);

                return RedirectToAction("ViewFlashcardPack", new { id = editedFlashcard.PackID });
            }   
            catch (Exception ex) {
                var logEntry = new LogEntry(
                    message: $"An error occurred while loading Flashcard with ID {editedFlashcard.ID}: {ex.Message}",
                    level: LogLevel.Error);
                _logger.Log(logEntry);
                return NotFound();
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> EditFlashcardPackName(Guid id, string newName) {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            try {
                using var httpClient = _httpClientFactory.CreateClient();

                await httpClient.PostAsync(
                    $"http://localhost:5123/api/FlashcardPack/EditFlashcardPackName?id={id}&newName={newName}&userId={userId}",
                    null);
                
                return RedirectToAction("CreateSampleFlashcardPack");
            }
            catch (Exception ex) {
                var logEntryError = new LogEntry(
                    message: $"An error occurred while editing FlashcardPack with ID {id}: {ex.Message}",
                    level: LogLevel.Error);
                _logger.Log(logEntryError);
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Present(Guid packID)
        {
            return RedirectToAction("PresentFlashcard", packID);
        }
    }
}