using Microsoft.EntityFrameworkCore;

namespace SEProject.Models;

public class User_FlashcardPacks
{
    public Guid UserID { get; set; }
    public Guid PackID { get; set; }

    public User_FlashcardPacks()
    {
        
    }
}