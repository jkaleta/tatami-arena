using Microsoft.EntityFrameworkCore;
using TatamiArena.Core.Models;

namespace TatamiArena.Data
{
    public class TatamiArenaContext : DbContext
    {
        public DbSet<Competition> Competitions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Competitor> Competitors { get; set; }
        public DbSet<CompetitionParticipant> CompetitionParticipants { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=TatamiArena;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompetitionParticipant>()
                .HasOne(cp => cp.Competition)
                .WithMany(c => c.Participants)
                .HasForeignKey(cp => cp.CompetitionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CompetitionParticipant>()
                .HasOne(cp => cp.Category)
                .WithMany(c => c.Participants)
                .HasForeignKey(cp => cp.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CompetitionParticipant>()
                .HasOne(cp => cp.Competitor)
                .WithMany(c => c.Participations)
                .HasForeignKey(cp => cp.CompetitorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Add unique constraint to prevent duplicate registrations
            modelBuilder.Entity<CompetitionParticipant>()
                .HasIndex(cp => new { cp.CompetitionId, cp.CategoryId, cp.CompetitorId })
                .IsUnique();
        }
    }
} 