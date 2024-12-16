using Microsoft.EntityFrameworkCore;
using WebApplication17.Models;
using WebApplication17.Repositories;


namespace WebApplication17.Repositories
{
    public class EFBookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public EFBookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await _context.Books
                                 .Include(b => b.Category)  // Bao gồm thông tin Category
                                 .ToListAsync();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            return await _context.Books
                                 .Include(b => b.Category)  // Bao gồm thông tin Category
                                 .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task AddAsync(Book book)
        {
            _context.Books.Add(book);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var book = await _context.Books.FindAsync(id);
            if (book != null)
            {
                _context.Books.Remove(book);
                await _context.SaveChangesAsync();
            }
        }
    }
}
