using SEProject.Models;

namespace SEProject.Services;

public interface IFlashcardPackDataHandler
{
    FlashcardPack LoadFlashcardPack(Guid id);
    List<FlashcardPack> LoadFlashcardPacks();
    void SaveFlashcardPack(FlashcardPack flashcardPack);
    void SaveFlashcardPacks(List<FlashcardPack> flashcardPacks);
    void RemoveFlashcardPack(Guid id);
}