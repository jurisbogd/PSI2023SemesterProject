using Microsoft.EntityFrameworkCore;
using SEProject.Models;

namespace SEProject.Services;

public class FlashcardPackFileIOService : IFlashcardPackDataHandler
{

    private DatabaseContext _context;
    public FlashcardPackFileIOService(DatabaseContext context)
    {
        this._context = context;
    }

    public FlashcardPack<Flashcard> LoadFlashcardPack(Guid ID)
    {
        FlashcardPack<Flashcard> flashcardPack = _context.FlashcardPacks
        .Include(pack => pack.Flashcards)
        .FirstOrDefault(pack => pack.ID == ID);

        return flashcardPack;
    }

    public List<FlashcardPack<Flashcard>> LoadFlashcardPacks()
    {
        List<FlashcardPack<Flashcard>> flashcardPacks = _context.FlashcardPacks
        .Include(pack => pack.Flashcards)
        .ToList();

        return flashcardPacks;
    }

    public void SaveFlashcardPack(FlashcardPack<Flashcard> flashcardPack)
    {
        var existingPack = _context.FlashcardPacks.FirstOrDefault(pack => pack.ID == flashcardPack.ID);
        if (existingPack == null)
        {
            _context.FlashcardPacks.Add(flashcardPack);
        }
        else
        {
            existingPack.Name = flashcardPack.Name;
        }
        _context.SaveChanges();
    }

    public void RemoveFlashcardPack(Guid ID)
    {
        var packToDelete = _context.FlashcardPacks
        .Include(pack => pack.Flashcards)
        .FirstOrDefault(pack => pack.ID == ID);

        _context.FlashcardPacks.Remove(packToDelete);
        _context.Flashcards.RemoveRange(packToDelete.Flashcards);
        _context.SaveChanges();
    }
}