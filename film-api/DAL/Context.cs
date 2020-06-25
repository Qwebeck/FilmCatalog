using FilmApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FilmApi.DAL
{
    public class Context: DbContext
    {
        public Context(DbContextOptions<Context> options) : base (options)
        { }
        public DbSet<Film> Films { get; set; } = null!;
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Mark> Marks { get; set; } = null!;
        public DbSet<Comment> Comments { get; set; } = null!;
        public DbSet<Image> Images { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("FilmApi");

            modelBuilder
                .Entity<Comment>()
                .Property(c => c.PublicationDate)
                .HasDefaultValueSql("getdate()");
        }
    }
}
