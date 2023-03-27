using ApiWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiWeb.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
    {
    }

    public DbSet<Game>? Games {get; set;}
    public DbSet<Classification>? Classifications {get; set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Classification
        modelBuilder.Entity<Classification>().HasKey(c => c.ClassificationId);
        
        modelBuilder.Entity<Classification>().Property(c => c.Type)
        .HasMaxLength(100)
         .IsRequired();

        //Game
        modelBuilder.Entity<Game>().HasKey(g => g.GameId);

        modelBuilder.Entity<Game>().Property(g => g.Name)
        .HasMaxLength(100)
         .IsRequired();

        modelBuilder.Entity<Game>().Property(g => g.Price)
        .HasPrecision(6,2);

        //connection
        modelBuilder.Entity<Game>()
        .HasOne<Classification>(c => c.Classification)
         .WithMany(g => g.Game)
          .HasForeignKey(c => c.ClassificationId);

    }

}