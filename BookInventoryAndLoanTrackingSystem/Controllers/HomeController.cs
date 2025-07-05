using BookInventoryAndLoanTrackingSystem.Models;
using Data.Identity;
using Entity.Entities;
using Entity.Services;
using Entity.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookInventoryAndLoanTrackingSystem.Controllers
{
	public class HomeController : Controller
	{
        private readonly IUnitOfWork _unitOfWork;

        private readonly ILogger<HomeController> _logger;
		private readonly IBookService _bookService;
		private readonly IBorrowService _borrowService;
		private readonly UserManager<AppUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IBookService bookService, IBorrowService borrowService, UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _bookService = bookService;
            _borrowService = borrowService;
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index(string search, string writerFilter, int? bookTypeId, string stockOrder)
		{

            

            var allBooks = await _bookService.GetAllAsync();

            // ViewBag'lere filtre uygulanmadan önceki verileri ata
            ViewBag.Writers = allBooks
                .Where(b => !string.IsNullOrWhiteSpace(b.Writer))
                .Select(b => b.Writer)
                .Distinct()
                .ToList();

            ViewBag.BookTypes = (await _unitOfWork.GetRepository<BookTypes>().GetAllAsync()).ToList();

            // Filtreleri uygula
            var books = allBooks;

            if (!string.IsNullOrEmpty(search))
            {
                books = books.Where(b =>
                    !string.IsNullOrEmpty(b.BookName) &&
                    b.BookName.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (!string.IsNullOrEmpty(writerFilter))
            {
                books = books.Where(b =>
                    !string.IsNullOrEmpty(b.Writer) &&
                    b.Writer.Contains(writerFilter, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            if (bookTypeId.HasValue && bookTypeId.Value != 0)
            {
                books = books.Where(b => b.BookTypesId == bookTypeId.Value).ToList();
            }
            if (!string.IsNullOrEmpty(stockOrder))
            {
                if (stockOrder == "asc")
                    books = books.OrderBy(b => b.StockCount).ToList();
                else if (stockOrder == "desc")
                    books = books.OrderByDescending(b => b.StockCount).ToList();
            }

            ViewBag.CurrentStockOrder = stockOrder;
            return View(books);
        }
        
        [HttpPost]
        public async Task<IActionResult> Borrow(int bookId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            await _borrowService.BorrowBookAsync(bookId, user.Id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> ReturnBook(int borrowId)
        {
            await _borrowService.ReturnBookAsync(borrowId);
            return RedirectToAction("MyBooks");
        }
        [HttpGet]
        public async Task<IActionResult> MyBooks()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");

            var borrows = await _borrowService.GetUserBorrowedBooksAsync(user.Id);
            return View(borrows);
        }
        public IActionResult Privacy()
		{
			return View();
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}
