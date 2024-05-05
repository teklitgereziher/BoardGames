using BoardGames.DataContract.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BoardGames.DataContract.DatabContext
{
  public class BoardGamesDbContext : IdentityDbContext<BoardGameUser>
  {
    public DbSet<BoardGame> BoardGames => Set<BoardGame>();
    public DbSet<Domain> Domains => Set<Domain>();
    public DbSet<Mechanic> Mechanics => Set<Mechanic>();
    public DbSet<BoardGames_Domains> BoardGames_Domains => Set<BoardGames_Domains>();
    public DbSet<BoardGames_Mechanics> BoardGames_Mechanics => Set<BoardGames_Mechanics>();
    public DbSet<Publisher> Publishers => Set<Publisher>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<BoardGames_Categories> BoardGames_Categories => Set<BoardGames_Categories>();

    public BoardGamesDbContext(
      DbContextOptions<BoardGamesDbContext> options) : base(options)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      // Keys of Identity tables are mapped in OnModelCreating method
      // of IdentityDbContext and if this method is not called, you will
      // 'The entity type 'IdentityUserLogin<string>' requires a primary key to be defined.
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<BoardGames_Domains>()
        .HasKey(i => new { i.BoardGameId, i.DomainId });
      modelBuilder.Entity<BoardGames_Domains>()
        .HasOne(x => x.BoardGame)
        .WithMany(y => y.BoardGames_Domains)
        .HasForeignKey(f => f.BoardGameId)
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);
      modelBuilder.Entity<BoardGames_Domains>()
        .HasOne(o => o.Domain)
        .WithMany(m => m.BoardGames_Domains)
        .HasForeignKey(f => f.DomainId)
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<BoardGames_Mechanics>()
        .HasKey(i => new { i.BoardGameId, i.MechanicId });
      modelBuilder.Entity<BoardGames_Mechanics>()
        .HasOne(x => x.BoardGame)
        .WithMany(y => y.BoardGames_Mechanics)
        .HasForeignKey(f => f.BoardGameId)
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);
      modelBuilder.Entity<BoardGames_Mechanics>()
        .HasOne(o => o.Mechanic)
        .WithMany(m => m.BoardGames_Mechanics)
        .HasForeignKey(f => f.MechanicId)
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);

      modelBuilder.Entity<BoardGames_Categories>()
        .HasKey(i => new { i.BoardGameId, i.CategoryId });
      //modelBuilder.Entity<BoardGames_Categories>()
      //  .HasOne(x => x.BoardGame)
      //  .WithMany(y => y.BoardGames_Categories)
      //  .HasForeignKey(f => f.BoardGameId)
      //  .IsRequired()
      //  .OnDelete(DeleteBehavior.Cascade);
      modelBuilder.Entity<BoardGames_Categories>()
        .HasOne(o => o.Category)
        .WithMany(m => m.BoardGames_Categories)
        .HasForeignKey(f => f.CategoryId)
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
