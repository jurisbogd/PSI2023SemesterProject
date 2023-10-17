using SEProject.Models;

namespace SEProject.Services;

public interface IFlashcardPackDataHandler
{
    FlashcardPack<Flashcard> LoadFlashcardPack(Guid id);
    List<FlashcardPack<Flashcard>> LoadFlashcardPacks();
    void SaveFlashcardPack(FlashcardPack<Flashcard> flashcardPack);
    void SaveFlashcardPacks(List<FlashcardPack<Flashcard>> flashcardPacks);
    void RemoveFlashcardPack(Guid id);
}