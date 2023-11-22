using SEProject.Exceptions;
using SEProject.Models;

namespace SEProject.Services;

public class FlashcardDataService : IFlashcardDataService {
    private readonly DatabaseContext _context;

    public FlashcardDataService(DatabaseContext context) {
        _context = context;
    }
    
    public async Task SaveFlashcard(Flashcard flashcard) {
        _context.Flashcards.Update(flashcard);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFlashcard(Guid id) {
        var flashcard = await FetchFlashcard(id);
        _context.Flashcards.Remove(flashcard);
        await _context.SaveChangesAsync();
    }

    public async Task<Flashcard> FetchFlashcard(Guid id) {
        return await _context.Flashcards.FindAsync(id)
            ?? throw new FlashcardNotFoundException();
    }
}