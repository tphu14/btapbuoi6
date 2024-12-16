using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using WebApplication17.Models;

public class ApplicationDbContext : DbContext
{
    // Constructor
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // DbSet for each entity (model)
    public DbSet<Book> Books { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<BookImage> BookImages { get; set; }

    // Configure the model using Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure Book entity
        modelBuilder.Entity<Book>(entity =>
        {
            // Set decimal precision for Price
            entity.Property(b => b.Price)
                .HasColumnType("decimal(18,2)");

            // Configure relationships and keys
            entity.HasOne(b => b.Category)
                  .WithMany(c => c.Books)
                  .HasForeignKey(b => b.CategoryId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure Category entity
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Name)
                  .IsRequired()
                  .HasMaxLength(50)
                  .HasColumnType("nvarchar(50)");
        });

        // Configure BookImage entity
        modelBuilder.Entity<BookImage>(entity =>
        {
            entity.HasKey(bi => bi.Id);
            entity.Property(bi => bi.Url)
                  .IsRequired()
                  .HasColumnType("nvarchar(max)");

            entity.HasOne(bi => bi.Book)
                  .WithMany(b => b.Images)
                  .HasForeignKey(bi => bi.BookId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
