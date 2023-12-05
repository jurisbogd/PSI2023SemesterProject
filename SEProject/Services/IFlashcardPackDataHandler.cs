using SEProject.Models;
using SEProject.EventArguments;
using static SEProject.Services.FlashcardPackIOService;

namespace SEProject.Services;

public interface IFlashcardPackDataHandler
{
    event FlashcardPackChangedEventHandler? FlashcardPackChanged;
    void OnFlashcardPackChanged(FlashcardPackEventArgs e);
    Task<FlashcardPack>? LoadFlashcardPackAsync(Guid ID, Guid userID);
    Task<List<FlashcardPack>> LoadFlashcardPacksAsync(Guid userID);
    Task SaveFlashcardPackAsync(FlashcardPack flashcardPack, Guid userID, Func<FlashcardPack, bool> validationFunction = null);
    Task RemoveFlashcardPackAsync(Guid ID);
}