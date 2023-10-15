using System.Text.Json;
using SEProject.Models;

namespace SEProject.Services;

public class FlashcardFileIOService : IFlashcardDataHandler
{
    private readonly string _flashcardPath = @"Data/Flashcards/";
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };

    public Flashcard LoadFlashcard(Guid id) {
        var filepath = _flashcardPath + id.ToString() + ".json";
        var flashcardJson = File.ReadAllText(path: filepath);
        var flashcard = JsonSerializer.Deserialize<Flashcard>(flashcardJson);
        return flashcard;
    }

    private static Flashcard LoadFlashcard(string filepath)
    {
        var flashcardJson = File.ReadAllText(path: filepath);
        var flashcard = JsonSerializer.Deserialize<Flashcard>(flashcardJson);
        if(flashcard != null)
        {
            return flashcard;
        }else
        {
            throw new Exception("Failed to deserialize the Flashcard.");
        }
    }

    public List<Flashcard> LoadFlashcards()
    {
        var flashcardFilepaths = Directory.GetFiles(_flashcardPath);
        var flashcards = flashcardFilepaths.Select(LoadFlashcard).ToList();
        return flashcards;
    }

    public void SaveFlashcard(Flashcard flashcard)
    {
        var filepath = _flashcardPath + flashcard.ID.ToString() + ".json";
        var flashcardJson = JsonSerializer.Serialize(flashcard, _jsonSerializerOptions);

        File.WriteAllText(path: filepath, contents: flashcardJson);
    }

    public void SaveFlashcards(List<Flashcard> flashcards)
    {
        foreach (Flashcard flashcard in flashcards) {
            SaveFlashcard(flashcard);
        }
    }

    public void RemoveFlashcard(Guid IDToRemove)
    {
        var filepath = _flashcardPath + IDToRemove.ToString() + ".json";
        File.Delete(filepath);
    }
}