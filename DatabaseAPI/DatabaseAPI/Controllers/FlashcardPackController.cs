using DatabaseAPI.Exceptions;
using Microsoft.AspNetCore.Mvc;
using DatabaseAPI.Models;
using DatabaseAPI.Services;

namespace DatabaseAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FlashcardPackController : ControllerBase
{
    private readonly DatabaseContext _context;
    private readonly IFlashcardPackDataHandler _flashcardPackDataHandler;
    private readonly IFlashcardIOService _flashcardIOService;
    public FlashcardPackController(DatabaseContext context, IFlashcardPackDataHandler flashcardPackDataHandler, IFlashcardIOService flashcardIOService)
    {
        _context = context;
        _flashcardPackDataHandler = flashcardPackDataHandler;
        _flashcardIOService = flashcardIOService;
    }
    
    [HttpGet]
    [Route("GetFlashcardPacks")]
    public async Task<ActionResult<IEnumerable<FlashcardPack>>> GetFlashcardPacks(Guid userId)
    {
        try
        {
            var flashcardPacks = await _flashcardPackDataHandler.LoadFlashcardPacksAsync(userId);
            return Ok(flashcardPacks);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
    
    [HttpGet]
    [Route("GetFlashcardPack")]
    public async Task<ActionResult<FlashcardPack>> GetFlashcardPack(Guid packID, Guid userID)
    {
        try
        {
            // Assuming you have a service method to get a FlashcardPack by ID and UserId
            var flashcardPack = await _flashcardPackDataHandler.LoadFlashcardPackAsync(packID, userID);

            if (flashcardPack == null)
            {
                return NotFound();
            }

            return Ok(flashcardPack);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "An error occurred while processing your request.");
        }
    }
    
    [HttpPost]
    [Route("AddFlashcardPack")]
    public async Task<ActionResult> AddFlashcardPack([FromBody] FlashcardPack flashcardPackRequest, Guid userID)
    {
        try
        {
            if (flashcardPackRequest == null)
            {
                return BadRequest("Invalid request payload");
            }

            var newFlashcardPack = new FlashcardPack
            (
                name: flashcardPackRequest.Name,
                id: Guid.NewGuid(),
                flashcards: new List<Flashcard>()
            );

            // Save the new flashcard pack (this will trigger the event)
            await _flashcardPackDataHandler.SaveFlashcardPackAsync(newFlashcardPack, userID, null);

            return Ok("Flashcard pack added successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
    
    [HttpPost]
    [Route("AddFlashcardToPack")]
    public async Task<ActionResult> AddFlashcardToPack([FromBody] Flashcard flashcard, Guid userID)
    {
        try
        {
            var flashcardPacks = await _flashcardPackDataHandler.LoadFlashcardPacksAsync(userID);
            var flashcardPack = flashcardPacks.FirstOrDefault(fpack => fpack.ID == flashcard.PackID);
            await _flashcardIOService.SaveFlashcard(flashcard, null);
            // Save the new flashcard
            await _flashcardPackDataHandler.SaveFlashcardPackAsync(flashcardPack, userID);
            
            return Ok("Flashcard added successfully");
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpDelete]
    [Route("RemoveFlashcardPack")]
    public async Task<ActionResult> RemoveFlashcardPack(Guid packID)
    {
        try
        {
            Console.WriteLine($"Removing flashcard pack with ID {packID}");
            await _flashcardPackDataHandler.RemoveFlashcardPackAsync(packID);
            Console.WriteLine($"Flashcard pack with ID {packID} removed successfully");
            return Ok("Flashcard pack removed successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception thrown: {ex.Message}");
            return StatusCode(500, "Internal server error");
        }
    }
    
    [HttpPost]
    [Route("RemoveFlashcardFromPack")]
    public async Task<ActionResult> RemoveFlashcardFromPack(Guid flashcardID, Guid packID, Guid userID)
    { 
        var flashcardPack = await _flashcardPackDataHandler.LoadFlashcardPackAsync(packID, userID)!;
        var flashcardToRemove = flashcardPack.Flashcards.FirstOrDefault(flashcard => flashcard.ID == flashcardID);

        try
        {
            await _flashcardIOService.RemoveFlashcard(flashcardToRemove!);
            return Ok("Flashcard removed successfully");
        }
        catch (FlashcardNotFoundException fnfe)
        {
            return NotFound("Flashcard not found");
        }
    }
    
    [HttpGet]
    [Route("GetFlashcard")]
    public async Task<ActionResult<Flashcard>> GetFlashcard(Guid flashcardID, Guid userID)
    {
        try
        {
            var flashcardPacks = await _flashcardPackDataHandler.LoadFlashcardPacksAsync(userID);
            var flashcardPack = flashcardPacks.FirstOrDefault(p => p.Flashcards.Any(f => f.ID == flashcardID));
            var flashcardToEdit = flashcardPack.Flashcards.First(f => f.ID == flashcardID);

            if (flashcardToEdit != null)
            {
                return Ok(flashcardToEdit);
            }

            return NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
    
    [HttpPost]
    [Route("EditFlashcard")]
    public async Task<IActionResult> EditFlashcard([FromBody] Flashcard editedFlashcard, Guid userID)
    {
        if (ModelState.IsValid)
        {
            var allFlashcardPacks = await _flashcardPackDataHandler.LoadFlashcardPacksAsync(userID);
            var flashcardToEdit = allFlashcardPacks
                .SelectMany(p => p.Flashcards)
                .FirstOrDefault(f => f.ID == editedFlashcard.ID);
            
            flashcardToEdit.Question = editedFlashcard.Question;
            flashcardToEdit.Answer = editedFlashcard.Answer;
            flashcardToEdit.Difficulty = editedFlashcard.Difficulty;
            flashcardToEdit.IsFavorite = editedFlashcard.IsFavorite;
            
            try
            {
                // Assuming _flashcardIOService has a method for editing flashcards
                await _flashcardIOService.SaveFlashcard(flashcardToEdit, null);

                // Return a successful response
                return Ok(new { PackID = flashcardToEdit.PackID });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal Server Error");
            }
        }
        return BadRequest("Invalid model");
    }
    
    [HttpPost("EditFlashcardPackName")]
    public async Task<IActionResult> EditFlashcardPackName(Guid id, string newName, Guid userId)
    {
        try
        {
            // Get the flashcard pack from the service
            var flashcardPack = await _flashcardPackDataHandler.LoadFlashcardPackAsync(id, userId);

            // Check if the flashcard pack exists
            if (flashcardPack == null)
            {
                return NotFound($"Flashcard pack with ID {id} not found for user with ID {userId}");
            }

            // Update the flashcard pack's name
            flashcardPack.Name = newName;

            // Save the updated flashcard pack
            await _flashcardPackDataHandler.SaveFlashcardPackAsync(flashcardPack, userId);
            Console.WriteLine($"Flashcard pack with ID {id} updated successfully");
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}