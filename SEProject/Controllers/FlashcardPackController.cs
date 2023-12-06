using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using SEProject.Models;
using SEProject.Services;
using SEProject.EventServices;
using SEProject.Exceptions;

namespace SEProject.Controllers
{
    public class FlashcardPackController : Controller
    {
        private readonly ILoggingHandler _logger;
        Func<FlashcardPack, bool> FlashcardPackIDValidation = flashcardPack => flashcardPack.ID != Guid.Empty;
        Func<Flashcard, bool> FlashcardIDValidation = flashcard => flashcard.ID != Guid.Empty;

        public FlashcardPackController(ILoggingHandler logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> CreateSampleFlashcardPack(string name)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Guid.TryParse(userId, out Guid parsedUserID);

                using (var httpClient = new HttpClient())
                {
                    var apiEndpoint = $"http://localhost:5123/api/FlashcardPack/GetFlashcardPacks?userId={parsedUserID}";

                    var response = await httpClient.GetAsync(apiEndpoint);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseContent))
                        {
                            var flashcardPacks = JsonConvert.DeserializeObject<List<FlashcardPack>>(responseContent);
                            
                            return View(flashcardPacks);
                        }
                    }
                    var logEntry = new LogEntry(
                        message: $"An error occurred while loading FlashcardPacks for user {parsedUserID}: {response.ReasonPhrase}",
                        level: LogLevel.Error);
                    _logger.Log(logEntry);
                    return View();
                }
            }
            catch (Exception ex)
            {
                var logEntry = new LogEntry(
                    message: $"An error occurred while loading FlashcardPacks for user {name}: {ex.Message}",
                    level: LogLevel.Error);
                _logger.Log(logEntry);
                return View();
            }
        }
        
        public async Task<IActionResult> ViewFlashcardPack(Guid id)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Guid.TryParse(userId, out Guid parsedUserID);

                using (var httpClient = new HttpClient())
                {
                    var apiEndpoint = $"http://localhost:5123/api/FlashcardPack/GetFlashcardPack?packID={id}&userId={parsedUserID}";

                    var response = await httpClient.GetAsync(apiEndpoint);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseContent))
                        {
                            var flashcardPackToView = JsonConvert.DeserializeObject<FlashcardPack>(responseContent);

                            return View(flashcardPackToView);
                        }
                    }
                }
                return NotFound();
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
                var newFlashcardPack = new FlashcardPack
                (
                    name: name,
                    id: Guid.NewGuid(),
                    flashcards: new List<Flashcard>()
                );
                var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Guid.TryParse(userID, out Guid parsedUserID);

                using (var httpClient = new HttpClient())
                {
                    var apiEndpoint = $"http://localhost:5123/api/FlashcardPack/AddFlashcardPack?userID={parsedUserID}";
                    
                    var response = await httpClient.PostAsync(apiEndpoint, new StringContent(JsonConvert.SerializeObject(newFlashcardPack), Encoding.UTF8, "application/json"));
                    
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseContent))
                        {
                            return RedirectToAction("CreateSampleFlashcardPack");
                        }
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                var logEntry = new LogEntry(
                    message: $"An error occurred while loading FlashcardPack with name {name}: {ex.Message}",
                    level: LogLevel.Error);
                _logger.Log(logEntry);

                return NotFound();
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
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Guid.TryParse(userId, out Guid parsedUserID);

                if (ModelState.IsValid)
                {
                    if (!Regex.IsMatch(flashcard.Question.TrimEnd(), @"\?$"))
                    {
                        TempData["AlertMessage"] = "The flashcard question must end with a question mark.";
                        return RedirectToAction("ViewFlashcardPack", new { id = flashcard.PackID });
                    }

                    // Assuming your API is hosted at http://your-api-url
                    var apiEndpoint = $"http://localhost:5123/api/FlashcardPack/AddFlashcardToPack?userID={parsedUserID}";

                    using (var httpClient = new HttpClient())
                    {
                        // Serialize the flashcard object to JSON
                        var flashcardJson = JsonConvert.SerializeObject(flashcard);

                        // Create a StringContent with the JSON data
                        var content = new StringContent(flashcardJson, Encoding.UTF8, "application/json");

                        // Make a POST request to the API endpoint
                        var response = await httpClient.PostAsync(apiEndpoint, content);

                        if (response.IsSuccessStatusCode)
                        {
                            // Redirect to the view that displays the pack of flashcards
                            return RedirectToAction("ViewFlashcardPack", new { id = flashcard.PackID });
                        }
                        else
                        {
                            // Handle the case where the API request is not successful
                            // You can log the response content or handle errors as needed
                            var errorResponse = await response.Content.ReadAsStringAsync();
                            return View("Error", errorResponse);
                        }
                    }
                }

                // If the model is not valid, return to the form view with validation errors
                return RedirectToAction("ViewFlashcardPack", new { id = flashcard.PackID });
            }
            catch (Exception ex)
            {
                // Handle the exception and log an error message with the exception details
                var logEntry = new LogEntry(
                        message: $"An error occurred while adding a flashcard to FlashcardPack with ID {packID}: {ex.Message}",
                        level: LogLevel.Error);
                _logger.Log(logEntry);

                // You can also handle the exception further or return an error view
                return View("Error", ex);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveFlashcardPack(Guid flashcardPackID)
        {
            try
            {
                using (var httpClient = new HttpClient())
                {
                    var apiEndpoint = $"http://localhost:5123/api/FlashcardPack/RemoveFlashcardPack?packID={flashcardPackID}";
                    Console.WriteLine($"apiEndpoint: {apiEndpoint}");
                    var response = await httpClient.DeleteAsync(apiEndpoint);
                    Console.WriteLine($"response: {response}");
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
        
                        if (!string.IsNullOrEmpty(responseContent))
                        {
                            return RedirectToAction("CreateSampleFlashcardPack");
                        }
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                var logEntry = new LogEntry(
                    message: $"An error occurred while removing FlashcardPack with ID {flashcardPackID}: {ex.Message}",
                    level: LogLevel.Error);
                _logger.Log(logEntry);
        
                return NotFound();
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveFlashcardFromPack(Guid flashcardID, Guid packID)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Guid.TryParse(userId, out Guid parsedUserID);
            
            var apiEndpoint = $"http://localhost:5123/api/FlashcardPack/RemoveFlashcardFromPack?flashcardID={flashcardID}&packID={packID}&userID={parsedUserID}";

            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.PostAsync(apiEndpoint, null);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("ViewFlashcardPack", new { id = packID });
                }
                var logEntry = new LogEntry(
                    message: $"An error occurred while removing Flashcard with ID {flashcardID}. Status code: {response.StatusCode}",
                    level: LogLevel.Error);
                _logger.Log(logEntry);

                return BadRequest($"Failed to remove Flashcard with ID {flashcardID}");
            }
        }
        
        [HttpGet]
        public async Task<IActionResult> EditFlashcard(Guid flashcardID)
        {
            try
            {
                var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Guid.TryParse(userID, out Guid parsedUserID);
                var apiEndpoint = $"http://localhost:5123/api/FlashcardPack/GetFlashcard?flashcardID={flashcardID}&userID={parsedUserID}";

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(apiEndpoint);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();

                        if (!string.IsNullOrEmpty(responseContent))
                        {
                            var flashcardToEdit = JsonConvert.DeserializeObject<Flashcard>(responseContent);

                            return View(flashcardToEdit);
                        }
                    }
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                var logEntry = new LogEntry(
                    message: $"An error occurred while loading Flashcard with ID {flashcardID}: {ex.Message}",
                    level: LogLevel.Error);
                _logger.Log(logEntry);

                // Handle error scenarios
                return NotFound();
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> EditFlashcard(Flashcard editedFlashcard)
        {
            using (var httpClient = new HttpClient())
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Guid.TryParse(userId, out Guid parsedUserID);

                var apiEndpoint = $"http://localhost:5123/api/FlashcardPack/EditFlashcard?userID={parsedUserID}";

                // Serialize the editedFlashcard object to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(editedFlashcard), Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(apiEndpoint, jsonContent);

                if (response.IsSuccessStatusCode)
                {
                    // Read the response content to get the PackID
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var packIdObject = JsonConvert.DeserializeAnonymousType(responseContent, new { PackID = Guid.Empty });

                    // Redirect to the view that displays the flashcards with the extracted PackID
                    return RedirectToAction("ViewFlashcardPack", new { id = packIdObject.PackID });
                }
                
                return RedirectToAction("CreateSampleFlashcardPack");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> EditFlashcardPackName(Guid id, string newName)
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                Guid.TryParse(userId, out Guid parsedUserID);

                using (var httpClient = new HttpClient())
                {
                    
                    var apiEndpoint = $"http://localhost:5123/api/FlashcardPack/EditFlashcardPackName?id={id}&newName={newName}&userId={parsedUserID}";

                    var response = await httpClient.PostAsync(apiEndpoint, null);

                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("CreateSampleFlashcardPack");
                    }
                    else
                    {
                        var logEntryError = new LogEntry(
                            message: $"An error occurred while editing FlashcardPack with ID {id}. Status code: {response.StatusCode}",
                            level: LogLevel.Error);
                        _logger.Log(logEntryError);
                        
                        return View("Error");
                    }
                }
            }
            catch (Exception ex)
            {
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