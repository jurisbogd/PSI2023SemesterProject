using Microsoft.AspNetCore.Mvc;
using System;
using SEProject.Models;
using SEProject.Services;

namespace SEProject.Controllers;

public class FlashcardPackController : Controller
{
    private readonly IWebHostEnvironment _env;
    private FlashcardPackService _flashcardPackService;

    public FlashcardPackController(IWebHostEnvironment env)
    {
        _env = env;
        _flashcardPackService = new FlashcardPackService();
    }
    public IActionResult CreateSampleFlashcardPack() // NOTE: this will be executed every time you reload the page
    {

        List<FlashcardPack> allFlaschardPacks = _flashcardPackService.LoadFlashcardPacks(_env);
        foreach (var flashcardPack in allFlaschardPacks)
            {
                Console.WriteLine($"Flashcard Pack Name: {flashcardPack.name}");
                Console.WriteLine($"Flashcard Pack ID: {flashcardPack.ID}");

                // Iterate through the flashcards in the pack
                foreach (var flashcard in flashcardPack.flaschard)
                {
                    Console.WriteLine("Flashcard:");
                    Console.WriteLine($"  ID: {flashcard.ID}");
                    Console.WriteLine($"  Question: {flashcard.question}");
                    Console.WriteLine($"  Answer: {flashcard.answer}");
                    Console.WriteLine($"  Difficulty Level: {flashcard.difficultyLevel}");
                    Console.WriteLine();
                }
            }
        return View(allFlaschardPacks);
    }
}