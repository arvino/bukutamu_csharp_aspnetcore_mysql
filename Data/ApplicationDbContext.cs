using Microsoft.EntityFrameworkCore;
using BukuTamuApp.Models;

namespace BukuTamuApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<BukuTamu> BukuTamus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Member>()
                .HasIndex(m => m.Email)
                .IsUnique();

            modelBuilder.Entity<BukuTamu>()
                .HasOne(b => b.Member)
                .WithMany(m => m.BukuTamus)
                .HasForeignKey(b => b.MemberId);
        }
    }
} 