﻿using Microsoft.EntityFrameworkCore;
using DatabaseAPI.Models;
using DatabaseAPI.Exceptions;

#pragma warning disable 8981 // Disable warning CS8981 (The type name 'initial' only contains lower-cased ascii characters. Such names may become reserved for the language.)
namespace DatabaseAPI.Services;
public class FlashcardIOService : IFlashcardIOService
{
    private DatabaseContext _context;

    public FlashcardIOService(DatabaseContext context)
    {
        _context = context;
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
    }
}
#pragma warning restore 8981 // Restore warning CS8981 (The type name 'initial' only contains lower-cased ascii characters. Such names may become reserved for the language.)