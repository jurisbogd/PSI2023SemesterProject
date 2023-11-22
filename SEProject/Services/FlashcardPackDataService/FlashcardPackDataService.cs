using Microsoft.EntityFrameworkCore;
using SEProject.EventArguments;
using SEProject.Exceptions;
using SEProject.Models;

namespace SEProject.Services;

public class FlashcardPackDataService : IFlashcardPackDataService {
    private readonly DatabaseContext _context;

    public event EventHandler<FlashcardPackEventArgs>? FlashcardPackChanged;

    public FlashcardPackDataService(DatabaseContext context) {
        _context = context;
    }

    public async Task SaveFlashcardPack(FlashcardPack flashcardPack) {
        if (await _context.FlashcardPacks.AnyAsync(pack => pack.ID == flashcardPack.ID)) {
            _context.FlashcardPacks.Update(flashcardPack);
        }
        else {
            await _context.FlashcardPacks.AddAsync(flashcardPack);
        }
        await _context.SaveChangesAsync();
        OnFlashcardPackChanged(flashcardPack);
    }

    public async Task DeleteFlashcardPack(Guid id) {
        var pack = await _context.FlashcardPacks.Include(pack => pack.Flashcards).FirstOrDefaultAsync(pack => pack.ID == id)
            ?? throw new FlashcardPackNotFoundException();
        _context.FlashcardPacks.Remove(pack);
        _context.Flashcards.RemoveRange(pack.Flashcards);
        await _context.SaveChangesAsync();
        OnFlashcardPackChanged(pack);
    }

    public async Task<FlashcardPack> FetchFlashcardPack(Guid id) {
        return await _context.FlashcardPacks.Include(pack => pack.Flashcards).FirstOrDefaultAsync(pack => pack.ID == id)
            ?? throw new FlashcardPackNotFoundException();
    }

    public async Task<List<FlashcardPack>> FetchSampleFlashcardPacks() {
        return await _context.FlashcardPacks.ToListAsync();
    }

    protected virtual void OnFlashcardPackChanged(FlashcardPack pack) =>
        FlashcardPackChanged?.Invoke(this, new FlashcardPackEventArgs(pack));
}