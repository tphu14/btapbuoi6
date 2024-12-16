using Microsoft.EntityFrameworkCore;
using WebApplication17.Models;
using WebApplication17.Repositories;


namespace WebApplication17.Repositories
{
    public class EFCategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public EFCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Lấy tất cả các danh mục
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories
                                 .AsNoTracking() // Không theo dõi trạng thái đối tượng (tăng hiệu suất)
                                 .Include(c => c.Books) // Bao gồm danh sách sách
                                 .ToListAsync();
        }

        // Lấy danh mục theo ID
        public async Task<Category> GetByIdAsync(int id)
        {
            return await _context.Categories
                                 .AsNoTracking()
                                 .Include(c => c.Books) // Bao gồm danh sách sách
                                 .FirstOrDefaultAsync(c => c.Id == id);
        }

        // Thêm danh mục mới
        public async Task AddAsync(Category category)
        {
            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
        }

        // Cập nhật danh mục
        public async Task UpdateAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }

        // Xóa danh mục
        public async Task DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        // Lấy danh mục chứa một sách cụ thể
        public async Task<Category> GetByBookIdAsync(int bookId)
        {
            var book = await _context.Books
                                     .Include(b => b.Category)
                                     .FirstOrDefaultAsync(b => b.Id == bookId);
            return book?.Category; // Trả về danh mục chứa sách hoặc null
        }

        // Lấy tất cả danh mục cùng danh sách sách
        public async Task<IEnumerable<Category>> GetAllWithBooksAsync()
        {
            return await _context.Categories
                                 .AsNoTracking()
                                 .Include(c => c.Books) // Bao gồm danh sách sách
                                 .ToListAsync();
        }
    }
}
