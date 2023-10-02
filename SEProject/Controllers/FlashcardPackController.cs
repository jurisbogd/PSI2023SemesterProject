using Microsoft.AspNetCore.Mvc;
using System;
using SEProject.Models;
using SEProject.Services;

namespace SEProject.Controllers;

public class FlashcardPackController : Controller
{
    public IActionResult CreateSampleFlashcardPack() // NOTE: this will be executed every time you reload the page
    {

        return View();
    }
}