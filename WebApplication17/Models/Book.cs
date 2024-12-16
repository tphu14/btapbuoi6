using System.ComponentModel.DataAnnotations;
namespace WebApplication17.Models
{
    public class Book
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Title { get; set; } = string.Empty; // Giá trị mặc định

        [Required, StringLength(100)]
        public string Author { get; set; } = string.Empty; // Giá trị mặc định

        [Range(0.01, 10000.00)]
        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty; // Giá trị mặc định
        public string? Image { get; set; }
        public int CategoryId { get; set; }
        public Category? Category { get; set; }
        public List<BookImage>? Images { get; set; }
    }
}