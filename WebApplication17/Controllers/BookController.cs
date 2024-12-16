using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebApplication17.Models;
using WebApplication17.Repositories;

namespace WebApplication17.Controllers
{
    public class BookController : Controller
    {
        private readonly IBookRepository _bookRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BookController(IBookRepository bookRepository, ICategoryRepository categoryRepository)
        {
            _bookRepository = bookRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var books = await _bookRepository.GetAllAsync();

            // Tạo thống kê số lượng sách theo chủ đề
            var categoryStats = books
                .GroupBy(b => b.Category?.Name ?? "Uncategorized")
                .Select(g => new
                {
                    CategoryName = g.Key,
                    BookCount = g.Count()
                }).ToList();

            ViewBag.CategoryStats = categoryStats;
            return View(books);
        }

        // Hiển thị danh sách sách theo chủ đề
        public async Task<IActionResult> ByCategory(int categoryId)
        {
            var books = await _bookRepository.GetAllAsync();
            var booksByCategory = books.Where(b => b.CategoryId == categoryId).ToList();

            var category = await _categoryRepository.GetByIdAsync(categoryId);
            ViewBag.CategoryName = category?.Name ?? "Unknown Category";

            return View(booksByCategory);
        }


        // Hiển thị form thêm sách mới
        public async Task<IActionResult> Add()
        {
            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View();
        }

        // Xử lý thêm sách mới
        [HttpPost]
        public async Task<IActionResult> Add(Book book, IFormFile image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    book.Image = await SaveImage(image);
                }

                await _bookRepository.AddAsync(book);
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
            return View(book);
        }

        // Lưu hình ảnh vào thư mục
        private async Task<string> SaveImage(IFormFile image)
        {
            var savePath = Path.Combine("wwwroot/images", image.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName;
        }

        // Hiển thị thông tin chi tiết sách
        public async Task<IActionResult> Display(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // Hiển thị form cập nhật sách
        public async Task<IActionResult> Update(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", book.CategoryId);
            return View(book);
        }

        // Xử lý cập nhật sách
        [HttpPost]
        public async Task<IActionResult> Update(int id, Book book, IFormFile image)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            ModelState.Remove("Image");

            if (ModelState.IsValid)
            {
                var existingBook = await _bookRepository.GetByIdAsync(id);
                if (existingBook == null)
                {
                    return NotFound();
                }

                if (image != null)
                {
                    book.Image = await SaveImage(image);
                }
                else
                {
                    book.Image = existingBook.Image;
                }

                existingBook.Title = book.Title;
                existingBook.Author = book.Author;
                existingBook.Price = book.Price;
                existingBook.Description = book.Description;
                existingBook.CategoryId = book.CategoryId;
                existingBook.Image = book.Image;

                await _bookRepository.UpdateAsync(existingBook);
                return RedirectToAction(nameof(Index));
            }

            var categories = await _categoryRepository.GetAllAsync();
            ViewBag.Categories = new SelectList(categories, "Id", "Name", book.CategoryId);
            return View(book);
        }

        // Hiển thị form xác nhận xóa sách
        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookRepository.GetByIdAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // Xử lý xóa sách
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _bookRepository.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
