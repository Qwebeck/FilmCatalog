﻿using FilmApi.Models;
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

        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlite("Data Source=blogging.db");
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("FilmApi");
            modelBuilder
                .Entity<Comment>()
                .HasMany(c => c.NextComments)
                .WithOne()
                .HasForeignKey("PreviousCommentID");

            var converter = new EnumToStringConverter<Roles>();
            modelBuilder
                .Entity<User>()
                .Property(u => u.Role)
                .HasConversion(converter);

        }
    }
}
