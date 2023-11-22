using SEProject.Models;

namespace SEProject.Services;

public interface IFlashcardDataService {
    public Task SaveFlashcard(Flashcard card);
    public Task DeleteFlashcard(Guid id);
    public Task<Flashcard> FetchFlashcard(Guid id);
}