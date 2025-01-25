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
            base.OnModelCreating(modelBuilder);

            // Konfigurasi nama tabel
            modelBuilder.Entity<Member>().ToTable("member");
            modelBuilder.Entity<BukuTamu>().ToTable("bukutamu");

            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Role).HasMaxLength(20);
            });

            modelBuilder.Entity<BukuTamu>(entity =>
            {
                entity.HasOne(d => d.Member)
                    .WithMany(p => p.BukuTamus)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
} 