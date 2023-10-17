using SEProject.Models;
using System.Text.Json;
using System.IO;
using System.Text;

namespace SEProject.Services;

public class FlashcardPackFileIOService : IFlashcardPackDataHandler
{
    private readonly string _flashcardPackPath = @"Data/FlashcardPacks/";
    private readonly JsonSerializerOptions _jsonSerializerOptions = new() { WriteIndented = true };

    public FlashcardPack<Flashcard> LoadFlashcardPack(Guid id)
    {
        var filepath = _flashcardPackPath + id.ToString() + ".json";
        if (File.Exists(filepath))
        {
            using (FileStream fileStream = File.OpenRead(filepath))
            using (StreamReader reader = new StreamReader(fileStream))
            {
                var flashcardPackJson = reader.ReadToEnd();
                var flashcardPack = JsonSerializer.Deserialize<FlashcardPack<Flashcard>>(flashcardPackJson);
                if (flashcardPack != null)
                {
                    return flashcardPack;
                }
                else
                {
                    throw new Exception("Failed to deserialize the FlashcardPack.");
                }
            }
        }
        else
        {
            throw new FileNotFoundException("Flashcard pack file not found.");
        }
    }

    private FlashcardPack<Flashcard> LoadFlashcardPack(string filepath)
    {
        if (File.Exists(filepath))
        {
            using (FileStream fileStream = File.OpenRead(filepath))
            using (StreamReader reader = new StreamReader(fileStream))
            {
                var flashcardPack = JsonSerializer.Deserialize<FlashcardPack<Flashcard>>(reader.ReadToEnd());
                if (flashcardPack != null)
                {
                    return flashcardPack;
                }
                else
                {
                    throw new Exception("Failed to deserialize the FlashcardPack.");
                }
            }
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
            using (FileStream fileStream = File.OpenRead(filepath))
            using (StreamReader reader = new StreamReader(fileStream))
            {
                var flashcardPack = JsonSerializer.Deserialize<FlashcardPack<Flashcard>>(reader.ReadToEnd());
                if (flashcardPack != null)
                {
                    flashcardPacks.Add(flashcardPack);
                }
                else
                {
                    throw new Exception("Failed to deserialize a FlashcardPack.");
                }
            }
        }
        return flashcardPacks;
    }

    public void SaveFlashcardPack(FlashcardPack<Flashcard> flashcardPack)
    {
        var filepath = _flashcardPackPath + flashcardPack.ID.ToString() + ".json";

        using (FileStream fileStream = File.Create(filepath))
        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            var json = JsonSerializer.Serialize(flashcardPack, _jsonSerializerOptions);
            writer.Write(json);
        }
    }


    public void SaveFlashcardPacks(List<FlashcardPack<Flashcard>> flashcardPacks)
    {
        foreach (FlashcardPack<Flashcard> flashcardPack in flashcardPacks)
        {
            var filepath = _flashcardPackPath + flashcardPack.ID.ToString() + ".json";

            using (FileStream fileStream = File.Create(filepath))
            using (StreamWriter writer = new StreamWriter(fileStream))
            {
                var json = JsonSerializer.Serialize(flashcardPack, _jsonSerializerOptions);
                writer.Write(json);
            }
        }
    }

    public void RemoveFlashcardPack(Guid IDToRemove)
    {
        var filepath = _flashcardPackPath + IDToRemove.ToString() + ".json";
        File.Delete(filepath);
    }
}