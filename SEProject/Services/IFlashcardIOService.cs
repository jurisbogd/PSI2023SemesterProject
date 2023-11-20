﻿using SEProject.Models;
using SEProject.EventArguments;

namespace SEProject.Services;

public interface IFlashcardIOService
{
    event EventHandler<FlashcardEventArgs>? FlashcardChanged;
    void OnFlashcardChanged(FlashcardEventArgs e);
    Task<List<Flashcard>> LoadFlashcardsAsync(Guid packID);
    Task SaveFlashcard(Flashcard flashcard, Func<Flashcard, bool> validationFunction);
    Task RemoveFlashcard(Flashcard flashcard);
    Task RemoveFlashcard(Guid id);
}

