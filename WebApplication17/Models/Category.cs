using System.ComponentModel.DataAnnotations;
using WebApplication17.Models;

namespace WebApplication17.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; } = string.Empty; // Giá trị mặc định

        public List<Book>? Books { get; set; }
    }

}
