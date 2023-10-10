using SEProject.Models;
using System.Text.Json;

namespace SEProject.Services;

public class FlashcardPackFileIOService
{
    private readonly string _flashcardPackPath = @"Data/FlashcardPacks/";
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };

    public FlashcardPack LoadFlashcardPack(Guid id)
    {
        var filepath = _flashcardPackPath + id.ToString() + ".json";
        var flashcardPackJson = File.ReadAllText(filepath);
        var flashcardPack = JsonSerializer.Deserialize<FlashcardPack>(flashcardPackJson);
        return flashcardPack;
    }

    private FlashcardPack LoadFlashcardPack(string filepath)
    {
        var flashcardPackJson = File.ReadAllText(filepath);
        var flashcardPack = JsonSerializer.Deserialize<FlashcardPack>(flashcardPackJson);
        return flashcardPack;
    }

    public List<FlashcardPack> LoadFlashcardPacks()
    {
        var flashcardPackFilepaths = Directory.GetFiles(_flashcardPackPath);
        var flashcardPacks = flashcardPackFilepaths.Select(LoadFlashcardPack).ToList();
        return flashcardPacks;
    }

    public void SaveFlashcardPack(FlashcardPack flashcardPack)
    {
        var filepath = _flashcardPackPath + flashcardPack.ID.ToString() + ".json";
        var flashcardPackJson = JsonSerializer.Serialize(flashcardPack, _jsonSerializerOptions);

        File.WriteAllText(filepath, flashcardPackJson);
    }

    public void SaveFlashcardPacks(List<FlashcardPack> flashcardPacks)
    {
        foreach (FlashcardPack flashcardPack in flashcardPacks)
        {
            SaveFlashcardPack(flashcardPack);
        }
    }

    public void RemoveFlashcardPack(Guid IDToRemove)
    {
        var filepath = _flashcardPackPath + IDToRemove.ToString() + ".json";
        File.Delete(filepath);
    }
}