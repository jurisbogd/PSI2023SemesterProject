using SEProject.Models;

namespace SEProject.Services;

public interface IFlashcardPackDataHandler
{
    FlashcardPack<Flashcard> LoadFlashcardPack(Guid ID);
    List<FlashcardPack<Flashcard>> LoadFlashcardPacks();
    void SaveFlashcardPack(FlashcardPack<Flashcard> flashcardPack);
    void RemoveFlashcardPack(Guid ID);
}