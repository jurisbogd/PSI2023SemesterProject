using Microsoft.EntityFrameworkCore;

namespace DatabaseAPI.Models
{
    public class DatabaseContext : DbContext
    {
        public DbSet<Flashcard> Flashcards { get; set; }
        public DbSet<FlashcardPack> FlashcardPacks { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserFlashcardPacks> UserFlashcardPacks { get; set; }

        public DatabaseContext(DbContextOptions options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FlashcardPack>()
                .HasMany(pack => pack.Flashcards)
                .WithOne()
                .HasForeignKey(flashcard => flashcard.PackID);
            
            modelBuilder.Entity<UserFlashcardPacks>().HasKey(ufp => new { ufp.UserID, ufp.PackID });
        }
    }
}
