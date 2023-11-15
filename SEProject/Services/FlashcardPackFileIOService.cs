using Microsoft.EntityFrameworkCore;
using SEProject.Models;
using SEProject.Events;

namespace SEProject.Services;

public class FlashcardPackFileIOService : IFlashcardPackDataHandler
{

    private DatabaseContext _context;
    public FlashcardPackFileIOService(DatabaseContext context)
    {
        this._context = context;
    }

    // Event for when a flashcard is saved or updated
    public event EventHandler<FlashcardPackEventArgs> FlashcardPackSavedOrUpdated;

    // Event for when a flashcard is removed
    public event EventHandler<FlashcardPackEventArgs> FlashcardPackRemoved;

    public async Task<FlashcardPack<Flashcard>>? LoadFlashcardPackAsync(Guid ID)
    {
        FlashcardPack<Flashcard>? flashcardPack = await _context.FlashcardPacks
            .Include(pack => pack.Flashcards)
            .FirstOrDefaultAsync(pack => pack.ID == ID);
         if (flashcardPack == null)
        {
            // Handle the case where the flashcardPack is null
            throw new Exception("FlashcardPack not found");
        }

        return flashcardPack;
    }

    public async Task<List<FlashcardPack<Flashcard>>> LoadFlashcardPacksAsync()
    {
        List<FlashcardPack<Flashcard>> flashcardPacks = await _context.FlashcardPacks
        .Include(pack => pack.Flashcards)
        .ToListAsync();

        return flashcardPacks;
    }

    public async Task SaveFlashcardPackAsync(FlashcardPack<Flashcard> flashcardPack, Func<FlashcardPack<Flashcard>, bool> validationFunction = null)
    {
        if (validationFunction != null && !validationFunction(flashcardPack))
        {
            // Validation failed, do not save the flashcard
            return;
        }
        var existingPack = await _context.FlashcardPacks
            .FirstOrDefaultAsync(pack => pack.ID == flashcardPack.ID);

        if (existingPack == null)
        {
            _context.FlashcardPacks.Add(flashcardPack);
        }
        else
        {
            existingPack.Name = flashcardPack.Name;
        }

        // Notify subscribers that the flashcard pack was saved or updated
        OnFlashcardPackSavedOrUpdated(new FlashcardPackEventArgs(flashcardPack));
        
        await _context.SaveChangesAsync();
    }

    public async Task RemoveFlashcardPackAsync(Guid ID)
    {
        var packToDelete = await _context.FlashcardPacks
        .Include(pack => pack.Flashcards)
        .FirstOrDefaultAsync(pack => pack.ID == ID);

        _context.FlashcardPacks.Remove(packToDelete!);
        _context.Flashcards.RemoveRange(packToDelete.Flashcards!);

        // Notify subscribers that the flashcard pack was removed
        OnFlashcardPackRemoved(new FlashcardPackEventArgs(packToDelete));

        await _context.SaveChangesAsync();
    }

    public virtual void OnFlashcardPackSavedOrUpdated(FlashcardPackEventArgs e)
    {
        FlashcardPackSavedOrUpdated?.Invoke(this, e);
    }

    public virtual void OnFlashcardPackRemoved(FlashcardPackEventArgs e)
    {
        FlashcardPackRemoved?.Invoke(this, e);
    }
}