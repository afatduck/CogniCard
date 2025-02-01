using CogniCard.Model.Flashcards;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogniCard.Model
{
    public class FlashcardContext : DbContext
    {
        public DbSet<Collection> Collections { get; set; }
        public DbSet<Flashcard> Flashcards { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Flashcard>()
                .HasDiscriminator<FlashcardType>("FlashcardType")
                .HasValue<ClassicFlashcard>(FlashcardType.Classic)
                .HasValue<FillTheBlankFlashcard>(FlashcardType.FillTheBlank)
                .HasValue<SingleChoiceFlashcard>(FlashcardType.SingleChoice)
                .HasValue<MultiChoiceFlashcard>(FlashcardType.MultiChoice)
                .HasValue<TrueOrFalseFlashcard>(FlashcardType.TrueOrFalse);

            modelBuilder.Entity<Flashcard>()
                .Property(m => m.LastReview).IsRequired(false);

            modelBuilder.Entity<Flashcard>()
                .Property(m => m.NextReview).IsRequired(false);

            modelBuilder.Entity<Flashcard>()
                .Property(m => m.QueuePriority).HasDefaultValue(0);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CogniCard");
            Directory.CreateDirectory(appDataPath);
            string dbPath = Path.Combine(appDataPath, "flashcards.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
