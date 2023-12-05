using Microsoft.EntityFrameworkCore;
using SEProject.Models;
using SEProject.EventArguments;
using SEProject.Exceptions;

namespace SEProject.Services;

public class FlashcardPackIOService : IFlashcardPackDataHandler
{
    public delegate void FlashcardPackChangedEventHandler(object source, FlashcardPackEventArgs args);
    public event FlashcardPackChangedEventHandler? FlashcardPackChanged;

    private DatabaseContext _context;
    public FlashcardPackIOService(DatabaseContext context)
    {
        this._context = context;
    }

    public async Task<FlashcardPack>? LoadFlashcardPackAsync(Guid packId, Guid userID)
    {
        // Check if the user has access to the specified pack
        bool userHasAccess = await _context.UserFlashcardPacks
            .AnyAsync(ufp => ufp.UserID == userID && ufp.PackID == packId);

        if (!userHasAccess)
        {
            // Handle the case where the user doesn't have access to the pack
            throw new Exception("User does not have access to the specified FlashcardPack");
        }

        FlashcardPack? flashcardPack = await _context.FlashcardPacks
            .Include(pack => pack.Flashcards)
            .FirstOrDefaultAsync(pack => pack.ID == packId);

        if (flashcardPack == null)
        {
            // Handle the case where the flashcardPack is null
            throw new Exception("FlashcardPack not found");
        }

        return flashcardPack;
    }

    public async Task<List<FlashcardPack>> LoadFlashcardPacksAsync(Guid userID)
    {
        // Check if the user has access to any flashcard packs
        List<Guid> accessiblePackIds = await _context.UserFlashcardPacks
            .Where(ufp => ufp.UserID == userID)
            .Select(ufp => ufp.PackID)
            .ToListAsync();

        List<FlashcardPack> flashcardPacks = await _context.FlashcardPacks
            .Where(pack => accessiblePackIds.Contains(pack.ID))
            .Include(pack => pack.Flashcards)
            .ToListAsync();

        return flashcardPacks;
    }

    public async Task SaveFlashcardPackAsync(FlashcardPack flashcardPack, Guid userId, Func<FlashcardPack, bool> validationFunction = null)
    {
        if (validationFunction != null && !validationFunction(flashcardPack))
        {
            // Validation failed, do not save the flashcard
            throw new Exception("Validation failed: FlashcardPack is invalid");
        }

        var existingPack = await _context.FlashcardPacks
            .FirstOrDefaultAsync(pack => pack.ID == flashcardPack.ID);

        if (existingPack == null)
        {
            _context.FlashcardPacks.Add(flashcardPack);

            // Create a new entry in UserFlashcardPacks for the user and the added flashcard pack
            var userFlashcardPack = new UserFlashcardPacks
            {
                UserID = userId,
                PackID = flashcardPack.ID
            };
            _context.UserFlashcardPacks.Add(userFlashcardPack);
        }
        else
        {
            existingPack.Name = flashcardPack.Name;
        }

        await _context.SaveChangesAsync();
        OnFlashcardPackChanged(new FlashcardPackEventArgs(flashcardPack, "Saved"));
    }

    public async Task RemoveFlashcardPackAsync(Guid ID)
    {
        var packToDelete = await _context.FlashcardPacks
            .Include(pack => pack.Flashcards)
            .FirstOrDefaultAsync(pack => pack.ID == ID);

        if (packToDelete == null)
        {
            // Handle the case where the flashcardPack is null
            throw new FlashcardPackNotFoundException($"FlashcardPack with ID {ID} not found");
        }

        // Remove the associated entry in UserFlashcardPacks
        var userFlashcardPackToDelete = await _context.UserFlashcardPacks
            .FirstOrDefaultAsync(ufp => ufp.PackID == ID);

        if (userFlashcardPackToDelete != null)
        {
            _context.UserFlashcardPacks.Remove(userFlashcardPackToDelete);
        }

        _context.FlashcardPacks.Remove(packToDelete!);
        _context.Flashcards.RemoveRange(packToDelete.Flashcards!);

        await _context.SaveChangesAsync();
        OnFlashcardPackChanged(new FlashcardPackEventArgs(packToDelete, "Deleted"));
    }


    public virtual void OnFlashcardPackChanged(FlashcardPackEventArgs e)
    {
        if(FlashcardPackChanged != null)
        {
            FlashcardPackChanged(this, e);
        }
    }
}