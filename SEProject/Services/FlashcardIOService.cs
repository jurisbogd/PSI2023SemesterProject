using Microsoft.EntityFrameworkCore;
using SEProject.Models;

namespace SEProject.Services;
public class FlashcardIOService : IFlashcardIOService
{
    private DatabaseContext _context;

    public FlashcardIOService(DatabaseContext context)
    {
        this._context = context;
    }

    public async Task SaveFlashcard(Flashcard flashcard)
    {
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
    }
        
    public async Task RemoveFlashcard(Flashcard flashcard)
    {
        var flashcardToRemove = await _context.Flashcards
            .FirstOrDefaultAsync(card => card.ID == flashcard.ID);
        _context.Remove(flashcardToRemove);
        await _context.SaveChangesAsync();
    }

}
