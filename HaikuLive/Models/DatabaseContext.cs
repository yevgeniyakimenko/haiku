using Microsoft.EntityFrameworkCore;
using HaikuLive.Models;

namespace HaikuLive.Models;

public class DatabaseContext : DbContext
{
  public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Author>()
      .Property(e => e.CreatedAt)
      .HasColumnType("TIMESTAMPTZ")
      .HasDefaultValueSql("current_timestamp");

    modelBuilder.Entity<Author>()
      .Property(e => e.Name)
      .HasColumnType("VARCHAR(50)");

    
    modelBuilder.Entity<Topic>()
      .Property(e => e.CreatedAt)
      .HasColumnType("TIMESTAMPTZ")
      .HasDefaultValueSql("current_timestamp");

    modelBuilder.Entity<Topic>()
      .Property(e => e.Name)
      .HasColumnType("VARCHAR(50)");


    modelBuilder.Entity<Haiku>()
      .Property(e => e.CreatedAt)
      .HasColumnType("TIMESTAMPTZ")
      .HasDefaultValueSql("current_timestamp");

    modelBuilder.Entity<Haiku>()
      .Property(e => e.Line1)
      .HasColumnType("VARCHAR(83)");
    
    modelBuilder.Entity<Haiku>()
      .Property(e => e.Line2)
      .HasColumnType("VARCHAR(83)");

    modelBuilder.Entity<Haiku>()
      .Property(e => e.Line3)
      .HasColumnType("VARCHAR(83)");

    modelBuilder.Entity<Haiku>()
      .Property(e => e.Liked)
      .HasDefaultValue(0);

    modelBuilder.Entity<Haiku>()
      .Property(e => e.AuthorName)
      .HasColumnType("VARCHAR(50)");

    modelBuilder.Entity<Haiku>()
      .HasIndex(h => h.TopicId);
  }

  public DbSet<Author> Authors => Set<Author>();
  public DbSet<Haiku> Haikus => Set<Haiku>();
  public DbSet<Topic> Topics => Set<Topic>();
}