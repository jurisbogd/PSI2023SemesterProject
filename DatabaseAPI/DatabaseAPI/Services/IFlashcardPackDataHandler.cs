using DatabaseAPI.Models;
using static DatabaseAPI.Services.FlashcardPackIOService;

namespace DatabaseAPI.Services;

public interface IFlashcardPackDataHandler
{
    Task<FlashcardPack>? LoadFlashcardPackAsync(Guid ID, Guid userID);
    Task<List<FlashcardPack>> LoadFlashcardPacksAsync(Guid userID);
    Task SaveFlashcardPackAsync(FlashcardPack flashcardPack, Guid userID, Func<FlashcardPack, bool> validationFunction = null);
    Task RemoveFlashcardPackAsync(Guid ID);
}