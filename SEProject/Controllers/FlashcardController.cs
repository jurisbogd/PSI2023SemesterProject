using Microsoft.AspNetCore.Mvc;
using SEProject.Models;

using System.Text.Json;
using System.Text.Json.Serialization;
namespace SEProject.Controllers;

public class FlashcardController : Controller
{
    private readonly IWebHostEnvironment _env;
    List<Flashcard> Allflashcards; // this variable holds all flashcards for the time being

    public FlashcardController(IWebHostEnvironment env)
    {
        _env = env;
    }

    public IActionResult CreateSampleFlashcard() // NOTE: this will be executed every time you reload the page
    {
        // To add all flashcards from json file to Allflashcards:
        LoadFlashcards("flashcards.json");

        // To add and remove(by object reference or id) flashcards from the list
        // remove later
        Flashcard newflashcard1 = new Flashcard(FindNextHighestID(Allflashcards), "Is this a test question?", "Yes", Flashcard.DifficultyLevel.Easy);
        Flashcard newflashcard2 = new Flashcard(FindNextHighestID(Allflashcards), "Is this an another test question?", "Yes", Flashcard.DifficultyLevel.Easy);
        AddFlashcard(newflashcard1);
        AddFlashcard(newflashcard2);
        RemoveFlashcard(newflashcard2);

        // To save Allflashcards to a json file. It will overwrite an already existing file.
        SaveFlashcards("flashcards.json");

        return View(Allflashcards);
    }

    public void LoadFlashcards(String Filename){
        // Json must be located in project root folder
        string jsonFilePath = System.IO.Path.Combine(_env.ContentRootPath, "flashcards.json");

        // Read the JSON file
        string jsonData = System.IO.File.ReadAllText(jsonFilePath);
        Allflashcards = JsonSerializer.Deserialize<List<Flashcard>>(jsonData);
    }
    public void SaveFlashcards(String Filename){
        string jsonString = JsonSerializer.Serialize(Allflashcards);
        System.IO.File.WriteAllText(Filename, jsonString);
    }
    public void RemoveFlashcard(int idToRemove){
        Allflashcards = Allflashcards.Where(flashcard => flashcard.ID != idToRemove).ToList();
    }
    public void RemoveFlashcard(Flashcard flashcardToRemove){
        Allflashcards.Remove(flashcardToRemove);
    }
    public void AddFlashcard(Flashcard flashcard){
        Allflashcards.Add(flashcard);
    }
    static int FindNextHighestID(List<Flashcard> flashcards)
    {
        int maxID = flashcards.Max(flashcard => flashcard.ID);
        return maxID + 1;
    }
}