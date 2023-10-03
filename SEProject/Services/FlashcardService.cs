using System.Text.Json;
using System;
using SEProject.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;
using Microsoft.VisualBasic;

namespace SEProject.Services;

public class FlashcardService
{
    private const string _flashcardPath = @"Data\Flashcards\";

    public List<Flashcard> LoadFlashcards(IWebHostEnvironment _env)
    {
        // Json must be located in project root folder
        string jsonFilePath = Path.Combine(_env.ContentRootPath, "flashcards.json");

        // Read the JSON file
        string jsonData = File.ReadAllText(jsonFilePath);
        return JsonSerializer.Deserialize<List<Flashcard>>(jsonData);
    }

    public void SaveFlashcard(Flashcard flashcard)
    {
        var filepath = _flashcardPath + flashcard.ID.ToString() + ".json";

        var flashcardJson = JsonSerializer.Serialize(
            flashcard,
            new JsonSerializerOptions { WriteIndented = true }
        );

        try {
            File.WriteAllText(filepath, flashcardJson);
        }
        catch (Exception exception) {
            // Handle exception
        }
    }

    public void SaveFlashcards(List<Flashcard> flashcards)
    {
        foreach (Flashcard flashcard in flashcards) {
            SaveFlashcard(flashcard);
        }
    }

    public void RemoveFlashcard(Guid idToRemove, List<Flashcard> Allflashcards)
    {
        // Find the index of the flashcard with the specified ID
        int indexToRemove = Allflashcards.FindIndex(flashcard => flashcard.ID == idToRemove);

        // If the flashcard is found (index >= 0), remove it from the list
        if (indexToRemove >= 0)
        {
            Allflashcards.RemoveAt(indexToRemove);
        }

    }
    public void RemoveFlashcard(Flashcard flashcardToRemove, List<Flashcard> Allflashcards)
    {
        Allflashcards.Remove(flashcardToRemove);
    }
    public void AddFlashcard(Flashcard flashcard, List<Flashcard> Allflashcards)
    {
        Allflashcards.Add(flashcard);
    }
}