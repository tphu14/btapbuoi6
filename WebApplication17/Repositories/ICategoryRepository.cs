using WebApplication17.Models;

namespace WebApplication17.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> GetAllAsync(); // Lấy tất cả danh mục
        Task<Category> GetByIdAsync(int id); // Lấy danh mục theo ID
        Task AddAsync(Category category); // Thêm danh mục mới
        Task UpdateAsync(Category category); // Cập nhật danh mục
        Task DeleteAsync(int id); // Xóa danh mục

        // Lấy danh mục có chứa sách cụ thể
        Task<Category> GetByBookIdAsync(int bookId);

        // Lấy tất cả danh mục với danh sách sách
        Task<IEnumerable<Category>> GetAllWithBooksAsync();
    }
}
