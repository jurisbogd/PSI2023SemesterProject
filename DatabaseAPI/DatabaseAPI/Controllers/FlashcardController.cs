using Microsoft.AspNetCore.Mvc;
using DatabaseAPI.Models;
using DatabaseAPI.Services;

namespace DatabaseAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlashcardController : ControllerBase
{
    private readonly IFlashcardIOService _flashcardIOService;

    public FlashcardController(IFlashcardIOService flashcardIOService)
    {
        _flashcardIOService = flashcardIOService;
    }
    
    [HttpGet]
    [Route("GetFlashcards")]
    public async Task<ActionResult<IEnumerable<Flashcard>>> GetFlashcards(Guid packID)
    {
        List<Flashcard> flashcards = await _flashcardIOService.LoadFlashcardsAsync(packID);
        Console.WriteLine($"Pack id: {packID}");
        Console.WriteLine(flashcards);
        return Ok(flashcards);
    }
    
    [HttpGet]
    [Route("GetNextFlashcard")]
    public async Task<ActionResult<Flashcard>> GetNextFlashcard(Guid packID, Guid currentFlashcardID)
    {
        List<Flashcard> flashcards = await _flashcardIOService.LoadFlashcardsAsync(packID);
    
        Flashcard currentFlashcard = flashcards.FirstOrDefault(f => f.ID == currentFlashcardID);
    
        int currentFlashcardIndex = flashcards.IndexOf(currentFlashcard);
    
        if (currentFlashcardIndex >= 0 && currentFlashcardIndex < flashcards.Count() - 1)
        {
            return Ok(flashcards[currentFlashcardIndex + 1]);
        }
        return Ok(null);
    }
    
    [HttpPost]
    [Route("ToggleFavorite")]
    public async Task<ActionResult> ToggleFavorite(Guid packID, Guid flashcardID, bool isFavorite)
    {
        try
        {
            List<Flashcard> flashcards = await _flashcardIOService.LoadFlashcardsAsync(packID);

            Flashcard currentFlashcard = flashcards.FirstOrDefault(f => f.ID == flashcardID);

            if (currentFlashcard != null)
            {
                currentFlashcard.IsFavorite = isFavorite;

                // Save the updated flashcard to the database
                await _flashcardIOService.SaveFlashcard(currentFlashcard, null);

                return Ok("Toggle successful");
            }
            else
            {
                return NotFound("Flashcard not found");
            }
        }
        catch (Exception ex)
        {
            // Log the exception or handle it as needed
            return StatusCode(500, "Internal server error");
        }
    }
}