using Microsoft.EntityFrameworkCore;

namespace SEProject.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Flashcard> Flashcards { get; set; }
        public DbSet<FlashcardPack<Flashcard>> FlashcardPacks { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
            
        }

    }
}
