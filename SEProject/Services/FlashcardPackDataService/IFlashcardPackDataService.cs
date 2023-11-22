using SEProject.EventArguments;
using SEProject.Models;

namespace SEProject.Services;

public interface IFlashcardPackDataService {
    public event EventHandler<FlashcardPackEventArgs>? FlashcardPackChanged;
    public Task SaveFlashcardPack(FlashcardPack pack);
    public Task DeleteFlashcardPack(Guid id);
    public Task<FlashcardPack> FetchFlashcardPack(Guid id);
    public Task<List<FlashcardPack>> FetchSampleFlashcardPacks();
}