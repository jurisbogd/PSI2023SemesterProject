using System.Text.Json;
using System;
using SEProject.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text;

namespace SEProject.Services;

public class FlashcardService
{
    public List<Flashcard> LoadFlashcards(IWebHostEnvironment _env)
    {
        // Json must be located in project root folder
        string jsonFilePath = Path.Combine(_env.ContentRootPath, "flashcards.json");

        // Read the JSON file
        string jsonData = File.ReadAllText(jsonFilePath);
        return JsonSerializer.Deserialize<List<Flashcard>>(jsonData);
    }

    public void saveFlashcard(string Filename, Flashcard newFlashcard)
    {
        try
        {
            var jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true,
            };
            string newFlashcardJson = JsonSerializer.Serialize(newFlashcard, jsonOptions);

            using (FileStream fileStream = new FileStream(Filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                fileStream.Seek(0, SeekOrigin.End);
                bool isEmpty = (fileStream.Length == 0);

                if (!isEmpty)
                {
                    // Find the closing curly brace ']'
                    long position = fileStream.Position;
                    byte[] buffer = new byte[1];
                    char lastChar = '\0';

                    while (fileStream.Position > 0 && lastChar != ']')
                    {
                        fileStream.Seek(-1, SeekOrigin.Current);
                        fileStream.Read(buffer, 0, 1);
                        lastChar = (char)buffer[0];
                    }
                    // Move back one more position to be before the ']'
                    fileStream.Seek(-1, SeekOrigin.Current);
                }

                // Write a comma ',' if file is not empty
                if (!isEmpty)
                {
                    fileStream.WriteByte((byte)',');
                }

                // Replace the 'ID' property in the JSON with the correct 'ID'
                string correctedJson = newFlashcardJson.Replace("\"ID\": 0,", $"\"ID\": \"{Guid.NewGuid()}\",");
                
                byte[] jsonBytes = Encoding.UTF8.GetBytes(correctedJson);
                fileStream.Write(jsonBytes, 0, jsonBytes.Length);

                fileStream.WriteByte((byte)']');
            }
        }
        catch (Exception exception)
        {
            // Handle exceptions appropriately
        }
    }


    public void SaveFlashcards(String Filename, List<Flashcard> Allflashcards)
    {
        string jsonString = JsonSerializer.Serialize(Allflashcards);
        System.IO.File.WriteAllText(Filename, jsonString);
    }
    public void RemoveFlashcard(Guid idToRemove, List<Flashcard> Allflashcards)
    {
        Allflashcards = Allflashcards.Where(flashcard => flashcard.ID != idToRemove).ToList();
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