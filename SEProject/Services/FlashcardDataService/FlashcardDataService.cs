using Microsoft.EntityFrameworkCore;
using SEProject.EventArguments;
using SEProject.Exceptions;
using SEProject.Models;

namespace SEProject.Services;

public class FlashcardDataService : IFlashcardDataService {
    private readonly DatabaseContext _context;

    public event EventHandler<FlashcardEventArgs>? FlashcardChanged;

    public FlashcardDataService(DatabaseContext context) {
        _context = context;
    }
    
    public async Task SaveFlashcard(Flashcard flashcard) {
        if (await _context.Flashcards.AnyAsync(pack => pack.ID == flashcard.ID)) {
            _context.Flashcards.Update(flashcard);
        }
        else {
            await _context.Flashcards.AddAsync(flashcard);
        }
        await _context.SaveChangesAsync();
        OnFlashcardChanged(flashcard);
    }

    public async Task DeleteFlashcard(Guid id) {
        var flashcard = await FetchFlashcard(id);
        _context.Flashcards.Remove(flashcard);
        await _context.SaveChangesAsync();
        OnFlashcardChanged(flashcard);
    }

    public async Task<Flashcard> FetchFlashcard(Guid id) {
        return await _context.Flashcards.FindAsync(id)
            ?? throw new FlashcardNotFoundException();
    }

    protected virtual void OnFlashcardChanged(Flashcard flashcard) =>
        FlashcardChanged?.Invoke(this, new FlashcardEventArgs(flashcard));
}