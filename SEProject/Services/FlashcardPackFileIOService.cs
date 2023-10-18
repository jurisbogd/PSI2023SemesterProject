using SEProject.Models;
using System.Text.Json;

namespace SEProject.Services;

public class FlashcardPackFileIOService : IFlashcardPackDataHandler
{
    private readonly string _flashcardPackPath = @"Data/FlashcardPacks/";
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };

    public FlashcardPack<Flashcard> LoadFlashcardPack(Guid id)
    {
        var filepath = _flashcardPackPath + id.ToString() + ".json";
        return LoadFlashcardPack(filepath);
    }

    private FlashcardPack<Flashcard> LoadFlashcardPack(string filepath)
    {
        if (File.Exists(filepath))
        {
            using FileStream fileStream = File.OpenRead(filepath);
            return fileStream.Deserialize<FlashcardPack<Flashcard>>()
                ?? throw new Exception("Failed to deserialize the FlashcardPack.");
        }
        else
        {
            throw new FileNotFoundException("Flashcard pack file not found.");
        }
    }

    public List<FlashcardPack<Flashcard>> LoadFlashcardPacks()
    {
        var flashcardPackFilepaths = Directory.GetFiles(_flashcardPackPath);
        var flashcardPacks = new List<FlashcardPack<Flashcard>>();

        foreach (var filepath in flashcardPackFilepaths)
        {
            flashcardPacks.Add(LoadFlashcardPack(filepath));
        }

        return flashcardPacks;
    }

    public void SaveFlashcardPack(FlashcardPack<Flashcard> flashcardPack)
    {
        var filepath = _flashcardPackPath + flashcardPack.ID.ToString() + ".json";

        using var fileStream = File.Create(filepath);
        fileStream.Serialize(flashcardPack, _jsonSerializerOptions);
    }


    public void SaveFlashcardPacks(List<FlashcardPack<Flashcard>> flashcardPacks)
    {
        foreach (var flashcardPack in flashcardPacks)
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