using SEProject.Models;

namespace SEProject.Services;

public interface IFlashcardPackDataHandler
{
    Task<FlashcardPack<Flashcard>>? LoadFlashcardPackAsync(Guid ID);
    Task<List<FlashcardPack<Flashcard>>> LoadFlashcardPacksAsync();
    Task SaveFlashcardPackAsync(FlashcardPack<Flashcard> flashcardPack, Func<FlashcardPack<Flashcard>, bool> validationFunction = null);
    Task RemoveFlashcardPackAsync(Guid ID);
}