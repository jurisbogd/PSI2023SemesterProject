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
}