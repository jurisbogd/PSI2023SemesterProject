namespace DatabaseAPI.Models;

public class UserFlashcardPacks
{
    public Guid UserID { get; set; }
    public Guid PackID { get; set; }

    public UserFlashcardPacks()
    {
        
    }

    public UserFlashcardPacks(Guid userID, Guid packID)
    {
        this.UserID = userID;
        this.PackID = packID;
    }
}