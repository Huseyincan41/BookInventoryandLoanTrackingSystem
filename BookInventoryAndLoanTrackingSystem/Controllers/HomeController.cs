using BookInventoryAndLoanTrackingSystem.Models;
using Data.Identity;
using Entity.Entities;
using Entity.Models;
using Entity.Services;
using Entity.UnitOfWorks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BookInventoryAndLoanTrackingSystem.Controllers
{
    [Authorize]
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

        public async Task<IActionResult> Index(BookFilterModel filter)
        {
            var booksPaged = await _bookService.GetFilteredBooksAsync(filter);
            ViewBag.BookTypes = (await _unitOfWork.GetRepository<BookTypes>().GetAllAsync()).ToList();

            ViewBag.Writers = booksPaged.Items
                .Where(b => !string.IsNullOrWhiteSpace(b.Writer))
                .Select(b => b.Writer)
                .Distinct()
                .ToList();

            ViewBag.CurrentStockOrder = filter.StockOrder;

            return View(booksPaged);
        }


        [HttpPost]
        public async Task<IActionResult> Borrow(int bookId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return RedirectToAction("Login", "Account");
            try
            {
                await _borrowService.BorrowBookAsync(bookId, user.Id);
            }
            catch (InvalidOperationException ex)
            {
                TempData["BorrowError"] = ex.Message;
            }
            var book = await _bookService.GetByIdAsync(bookId);
            if (book == null)
            {
                TempData["ErrorMessage"] = "Kitap bulunamadı.";
                return RedirectToAction("Index");
            }

            if (book.StockCount == 0)
            {
                TempData["ErrorMessage"] = "Bu kitap şu anda stokta yok.";
                return RedirectToAction("Index");
            }
            
            TempData["SuccessMessage"] = "Kitap başarıyla ödünç alındı.";

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
