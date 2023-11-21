using Microsoft.EntityFrameworkCore;
using SEProject.Models;
using SEProject.EventArguments;
using SEProject.Exceptions;

#pragma warning disable 8981 // Disable warning CS8981 (The type name 'initial' only contains lower-cased ascii characters. Such names may become reserved for the language.)
namespace SEProject.Services;
public class FlashcardIOService : IFlashcardIOService
{
    public event EventHandler<FlashcardEventArgs>? FlashcardChanged;
    private DatabaseContext _context;

    public FlashcardIOService(DatabaseContext context)
    {
        this._context = context;
    }

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
        }
        await _context.SaveChangesAsync();

        OnFlashcardChanged(new FlashcardEventArgs(flashcard, "Saved"));
    }

    public async Task RemoveFlashcard(Flashcard flashcard)
    {
        var flashcardToRemove = await _context.Flashcards
            .FirstOrDefaultAsync(card => card.ID == flashcard.ID);

        if (flashcardToRemove == null)
        {
            throw new FlashcardNotFoundException($"Flashcard with ID {flashcard.ID} not found.)");
        }
        
        _context.Remove(flashcardToRemove!);
        await _context.SaveChangesAsync();

        OnFlashcardChanged(new FlashcardEventArgs(flashcard, "Deleted"));
    }

    public async Task RemoveFlashcardFromPack(Guid packID, Guid flashcardID) {
        try {
            await FetchFlashcardPack(packID);
            var flashcard = await FetchFlashcard(flashcardID);
            
            if (flashcard.PackID == packID)
            {
                _context.Remove(flashcard);
                await _context.SaveChangesAsync();
            }
            else {
                throw new ArgumentException($"Flashcard pack with ID {packID} does not contain flashcard with ID {flashcardID}.");
            }
        }
        catch {
            throw;
        }
    }

    public virtual void OnFlashcardChanged(FlashcardEventArgs e)
    {
        if(FlashcardChanged != null)
        {
            FlashcardChanged(this, e);
        }
    }

    private async Task<Flashcard> FetchFlashcard(Guid id) {
        return await _context.Flashcards.FirstOrDefaultAsync(card => card.ID == id)
            ?? throw new FlashcardNotFoundException($"Flashcard with ID {id} was not found.");
    }

    private async Task<FlashcardPack> FetchFlashcardPack(Guid id) {
        return await _context.FlashcardPacks.FirstOrDefaultAsync(pack => pack.ID == id)
            ?? throw new FlashcardPackNotFoundException($"Flashcard pack with ID {id} was not found.");
    }
}
#pragma warning restore 8981 // Restore warning CS8981 (The type name 'initial' only contains lower-cased ascii characters. Such names may become reserved for the language.)