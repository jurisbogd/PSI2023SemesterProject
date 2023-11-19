﻿using Microsoft.EntityFrameworkCore;
using SEProject.Models;
using SEProject.EventArguments;
using SEProject.Exceptions;

namespace SEProject.Services;

public class FlashcardPackFileIOService : IFlashcardPackDataHandler
{
    public event EventHandler<FlashcardPackEventArgs>? FlashcardPackChanged;

    private DatabaseContext _context;
    public FlashcardPackFileIOService(DatabaseContext context)
    {
        this._context = context;
    }

    public async Task<FlashcardPack<Flashcard>>? LoadFlashcardPackAsync(Guid ID)
    {
        FlashcardPack<Flashcard>? flashcardPack = await _context.FlashcardPacks
            .Include(pack => pack.Flashcards)
            .FirstOrDefaultAsync(pack => pack.ID == ID);
         if (flashcardPack == null)
        {
            // Handle the case where the flashcardPack is null
            throw new Exception("FlashcardPack not found");
        }

        return flashcardPack;
    }

    public async Task<List<FlashcardPack<Flashcard>>> LoadFlashcardPacksAsync()
    {
        List<FlashcardPack<Flashcard>> flashcardPacks = await _context.FlashcardPacks
        .Include(pack => pack.Flashcards)
        .ToListAsync();

        return flashcardPacks;
    }

    public async Task SaveFlashcardPackAsync(FlashcardPack<Flashcard> flashcardPack, Func<FlashcardPack<Flashcard>, bool> validationFunction = null)
    {
        if (validationFunction != null && !validationFunction(flashcardPack))
        {
            // Validation failed, do not save the flashcard
            return;
        }
        var existingPack = await _context.FlashcardPacks
            .FirstOrDefaultAsync(pack => pack.ID == flashcardPack.ID);

        if (existingPack == null)
        {
            _context.FlashcardPacks.Add(flashcardPack);
        }
        else
        {
            existingPack.Name = flashcardPack.Name;
        }
        
        await _context.SaveChangesAsync();
        OnFlashcardPackChanged(new FlashcardPackEventArgs(flashcardPack, "Saved"));
    }

    public async Task RemoveFlashcardPackAsync(Guid ID)
    {
        var packToDelete = await _context.FlashcardPacks
        .Include(pack => pack.Flashcards)
        .FirstOrDefaultAsync(pack => pack.ID == ID);

        if (packToDelete == null)
        {
            // Handle the case where the flashcardPack is null
            throw new FlashcardPackNotFoundException($"FlashcardPack with ID {ID} not found");
        }

        _context.FlashcardPacks.Remove(packToDelete!);
        _context.Flashcards.RemoveRange(packToDelete.Flashcards!);

        await _context.SaveChangesAsync();
        OnFlashcardPackChanged(new FlashcardPackEventArgs(packToDelete, "Deleted"));
    }

    public virtual void OnFlashcardPackChanged(FlashcardPackEventArgs e)
    {
        if(FlashcardPackChanged != null)
        {
            FlashcardPackChanged(this, e);
        }
    }
}