using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using SEProject.Models;
using SEProject.Services;
using Microsoft.AspNetCore.Hosting;
using System.Text.Json;
using System.Xml.Linq;

namespace SEProject.Controllers
{
    public class FlashcardPackController : Controller
    {
        private readonly IFlashcardPackDataHandler _flashcardPackDataHandler;

        public FlashcardPackController(IFlashcardPackDataHandler flashcardPackDataHandler)
        {
            _flashcardPackDataHandler = flashcardPackDataHandler;
        }

        public IActionResult CreateSampleFlashcardPack(string name)
        {
            List<FlashcardPack> allFlashcardPacks = _flashcardPackDataHandler.LoadFlashcardPacks();

            return View(allFlashcardPacks);
        }


        public IActionResult ViewFlashcardPack(Guid id)
        {
            List<FlashcardPack> allFlashcardPacks = _flashcardPackDataHandler.LoadFlashcardPacks();
            FlashcardPack? flashcardPackToView = allFlashcardPacks.FirstOrDefault(fpack => fpack.ID == id);

            if (flashcardPackToView == null)
            {
                return NotFound(); // Handle the case when the pack is not found
            }
            foreach (var flashcard in flashcardPackToView.Flashcards)
            {
                Console.WriteLine(flashcard.Question);
            }

            return View(flashcardPackToView);
        }

        [HttpPost]
        public IActionResult AddFlashcardPack(string name)
        {
            var newFlashcardPack = new FlashcardPack(
                name: name,
                id: Guid.NewGuid(),
                flashcards: new List<Flashcard>());

            _flashcardPackDataHandler.SaveFlashcardPack(newFlashcardPack);

            return RedirectToAction("CreateSampleFlashcardPack");
        }


        [HttpPost]
        public IActionResult AddFlashcardToPack(Flashcard viewModel, Guid id)
        {
            List<FlashcardPack> allFlashcardPacks = _flashcardPackDataHandler.LoadFlashcardPacks();
            FlashcardPack? flashcardPackToBeChanged = allFlashcardPacks.FirstOrDefault(fpack => fpack.ID == id);

            if (flashcardPackToBeChanged == null)
            {
                return NotFound(); // Handle the case when the pack is not found
            }

            if (ModelState.IsValid)
            {
                // Create a new Flashcard object from the form data
                var newFlashcard = new Flashcard
                {
                    ID = Guid.NewGuid(),
                    Question = viewModel.Question,
                    Answer = viewModel.Answer,
                    Difficulty = viewModel.Difficulty
                };

                // Add the new flashcard to the pack
                flashcardPackToBeChanged.Flashcards.Add(newFlashcard);

                // Save the updated pack of flashcards to the JSON file
                _flashcardPackDataHandler.SaveFlashcardPack(flashcardPackToBeChanged);

                // Redirect to the view that displays the pack of flashcards
                return RedirectToAction("ViewFlashcardPack", new { id = flashcardPackToBeChanged.ID });
            }

            // If the model is not valid, return to the form view with validation errors
            return View(flashcardPackToBeChanged);
        }

        [HttpPost]
        public IActionResult RemoveFlashcardFromPack(Guid flashcardID, Guid packID)
        {
            var flashcardPack = _flashcardPackDataHandler.LoadFlashcardPack(packID);

            int indexToRemove = flashcardPack.Flashcards.FindIndex(flashcard => flashcard.ID == flashcardID);

            if (indexToRemove >= 0)
            {
                flashcardPack.Flashcards.RemoveAt(indexToRemove);

                _flashcardPackDataHandler.SaveFlashcardPack(flashcardPack);
            }

            // Redirect to the view that displays the pack of flashcards
            return RedirectToAction("ViewFlashcardPack", new { id = flashcardPack.ID });
        }
    }
}
