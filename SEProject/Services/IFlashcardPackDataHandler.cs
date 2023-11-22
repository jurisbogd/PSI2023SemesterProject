using SEProject.Models;
using SEProject.EventArguments;
using static SEProject.Services.FlashcardPackIOService;

namespace SEProject.Services;

public interface IFlashcardPackDataHandler
{
    event FlashcardPackChangedEventHandler? FlashcardPackChanged;
    void OnFlashcardPackChanged(FlashcardPackEventArgs e);
    Task<FlashcardPack>? LoadFlashcardPackAsync(Guid ID);
    Task<List<FlashcardPack>> LoadFlashcardPacksAsync();
    Task SaveFlashcardPackAsync(FlashcardPack flashcardPack, Func<FlashcardPack, bool> validationFunction = null);
    Task RemoveFlashcardPackAsync(Guid ID);
}