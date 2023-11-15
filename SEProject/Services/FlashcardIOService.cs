using Microsoft.EntityFrameworkCore;
using SEProject.Models;
using SEProject.Events;

#pragma warning disable 8981 // Disable warning CS8981 (The type name 'initial' only contains lower-cased ascii characters. Such names may become reserved for the language.)
namespace SEProject.Services;
public class FlashcardIOService : IFlashcardIOService
{
    private DatabaseContext _context;

    public FlashcardIOService(DatabaseContext context)
    {
        this._context = context;
    }

    // Event for when a flashcard is saved or updated
    public event EventHandler<FlashcardEventArgs> FlashcardSavedOrUpdated;

    // Event for when a flashcard is removed
    public event EventHandler<FlashcardEventArgs> FlashcardRemoved;

    public async Task<List<Flashcard>> LoadFlashcardsAsync(Guid packID)
    {
        List<Flashcard> flashcards = await _context.FlashcardPacks
            .Where(pack => pack.ID == packID)
            .SelectMany(pack => pack.Flashcards)
            .ToListAsync();

        return flashcards;
    }

    public async Task SaveFlashcard(Flashcard flashcard, Func<Flashcard, bool> validationFunction)
    {
        if (validationFunction != null && !validationFunction(flashcard))
        {
            // Validation failed, do not save the flashcard
            return;
        }
        var existingFlashcard = await _context.Flashcards
            .FirstOrDefaultAsync(card => card.ID == flashcard.ID);
        
        if(existingFlashcard == null) 
        { 
            _context.Flashcards.Add(flashcard);
        }
        else
        {
            foreach (var property in typeof(Flashcard).GetProperties())
            {
                var newValue = property.GetValue(flashcard);
                property.SetValue(existingFlashcard, newValue);
            }
            // Notify subscribers that the flashcard was updated
            OnFlashcardSavedOrUpdated(new FlashcardEventArgs(existingFlashcard));
        }
        // Notify subscribers that a flashcard was saved
        OnFlashcardSavedOrUpdated(new FlashcardEventArgs(flashcard));
        await _context.SaveChangesAsync();
    }
        
    public async Task RemoveFlashcard(Flashcard flashcard)
    {
        var flashcardToRemove = await _context.Flashcards
            .FirstOrDefaultAsync(card => card.ID == flashcard.ID);
        
        // Notify subscribers that a flashcard was removed
        OnFlashcardRemoved(new FlashcardEventArgs(flashcardToRemove));
        _context.Remove(flashcardToRemove!);
        await _context.SaveChangesAsync();
    }
    
    public virtual void OnFlashcardSavedOrUpdated(FlashcardEventArgs e)
    {
        FlashcardSavedOrUpdated?.Invoke(this, e);
    }

    public virtual void OnFlashcardRemoved(FlashcardEventArgs e)
    {
        FlashcardRemoved?.Invoke(this, e);
    }
}
#pragma warning restore 8981 // Restore warning CS8981 (The type name 'initial' only contains lower-cased ascii characters. Such names may become reserved for the language.)