using Microsoft.AspNetCore.Mvc;
using DatabaseAPI.Models;

namespace DatabaseAPI.Controllers;

public class FlashcardPackController : ControllerBase
{
    private readonly DatabaseContext _context;
    public FlashcardPackController(DatabaseContext context)
    {
        _context = context;
    }
}