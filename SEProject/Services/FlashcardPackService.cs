using System.Text.Json;
using System;
using SEProject.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;

namespace SEProject.Services;

public class FlashcardPackService
{
    public List<FlashcardPack> LoadFlashcardPacks(IWebHostEnvironment _env)
    {
        // Json must be located in project root folder
        string jsonFilePath = Path.Combine(_env.ContentRootPath, "flashcardPacks.json");

        // Read the JSON file
        string jsonData = File.ReadAllText(jsonFilePath);
        return JsonSerializer.Deserialize<List<FlashcardPack>>(jsonData);
    }

    public void SaveFlashcardPacks(List<FlashcardPack> AllflashcardPacks)
    {
        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
        };

        // Serialize the flashcards with proper formatting
        string jsonString = JsonSerializer.Serialize(AllflashcardPacks, jsonOptions);
        System.IO.File.WriteAllText("flashcardPacks.json", jsonString);
    }

    public void RemoveFlashcard(Guid idToRemove, FlashcardPack FlashcardPackToBeChanged)
    {
        // Find the index of the flashcard with the specified ID
        int indexToRemove = FlashcardPackToBeChanged.flashcard.FindIndex(flashcard => flashcard.ID == idToRemove);

        // If the flashcard is found (index >= 0), remove it from the list
        if (indexToRemove >= 0)
        {
            FlashcardPackToBeChanged.flashcard.RemoveAt(indexToRemove);
        }

    }
}