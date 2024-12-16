using WebApplication17.Models;

namespace WebApplication17.Models
{
    public class BookImage
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty; // Giá trị mặc định
        public int BookId { get; set; }
        public Book? Book { get; set; }
    }

}
