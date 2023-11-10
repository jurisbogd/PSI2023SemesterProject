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

    public void SaveFlashcard(Flashcard flashcard)
    {
        var existingFlashcard = _context.Flashcards
            .FirstOrDefault(card => card.ID == flashcard.ID);
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
        _context.SaveChanges();
    }

    public void RemoveFlashcard(Flashcard flashcard)
    {
        var flashcardToRemove = _context.Flashcards
            .FirstOrDefault(card => card.ID == flashcard.ID);
        _context.Remove(flashcardToRemove);
        _context.SaveChanges();
    }

}
