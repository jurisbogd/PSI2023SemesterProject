using SEProject.Models;
using SEProject.EventArguments;

namespace SEProject.Services;

public interface IFlashcardPackDataHandler
{
    event EventHandler<FlashcardPackEventArgs>? FlashcardPackChanged;
    void OnFlashcardPackChanged(FlashcardPackEventArgs e);
    Task<FlashcardPack>? LoadFlashcardPackAsync(Guid ID);
    Task<List<FlashcardPack>> LoadFlashcardPacksAsync();
    Task SaveFlashcardPackAsync(FlashcardPack flashcardPack, Func<FlashcardPack, bool> validationFunction = null);
    Task RemoveFlashcardPackAsync(Guid ID);
}