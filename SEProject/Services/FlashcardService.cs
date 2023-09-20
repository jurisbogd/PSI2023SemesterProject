using System.Text.Json;
using SEProject.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SEProject.Services;

public class FlashcardService
{
    public List<Flashcard> LoadFlashcards(String Filename, IWebHostEnvironment _env){
        // Json must be located in project root folder
        string jsonFilePath = System.IO.Path.Combine(_env.ContentRootPath, "flashcards.json");

        // Read the JSON file
        string jsonData = System.IO.File.ReadAllText(jsonFilePath);
        return JsonSerializer.Deserialize<List<Flashcard>>(jsonData);
    }
    public void SaveFlashcards(String Filename, List<Flashcard> Allflashcards){
        string jsonString = JsonSerializer.Serialize(Allflashcards);
        System.IO.File.WriteAllText(Filename, jsonString);
    }
    public void RemoveFlashcard(int idToRemove, List<Flashcard> Allflashcards){
        Allflashcards = Allflashcards.Where(flashcard => flashcard.ID != idToRemove).ToList();
    }
    public void RemoveFlashcard(Flashcard flashcardToRemove, List<Flashcard> Allflashcards){
        Allflashcards.Remove(flashcardToRemove);
    }
    public void AddFlashcard(Flashcard flashcard, List<Flashcard> Allflashcards){
        Allflashcards.Add(flashcard);
    }
    static int FindNextHighestID(List<Flashcard> flashcards)
    {
        int maxID = flashcards.Max(flashcard => flashcard.ID);
        return maxID + 1;
    }
}