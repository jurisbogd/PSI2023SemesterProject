using Microsoft.EntityFrameworkCore;

namespace SEProject.Models;

public class UserFlashcardPacks
{
    public Guid UserID { get; set; }
    public Guid PackID { get; set; }

    public UserFlashcardPacks()
    {
        
    }
}