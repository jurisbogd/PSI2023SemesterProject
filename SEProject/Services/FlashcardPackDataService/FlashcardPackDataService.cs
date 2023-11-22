using Microsoft.EntityFrameworkCore;
using SEProject.Exceptions;
using SEProject.Models;

namespace SEProject.Services;

public class FlashcardPackDataService : IFlashcardPackDataService {
    private readonly DatabaseContext _context;

    public FlashcardPackDataService(DatabaseContext context) {
        _context = context;
    }

    public async Task SaveFlashcardPack(FlashcardPack pack) {
        _context.FlashcardPacks.Update(pack);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteFlashcardPack(Guid id) {
        var pack = await FetchFlashcardPack(id);
        _context.FlashcardPacks.Remove(pack);
        await _context.SaveChangesAsync();
    }

    public async Task<FlashcardPack> FetchFlashcardPack(Guid id) {
        return await _context.FlashcardPacks.Include(pack => pack.Flashcards).SingleAsync(pack => pack.ID == id)
            ?? throw new FlashcardPackNotFoundException();
    }
}