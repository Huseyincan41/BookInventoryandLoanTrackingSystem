using Entity.Entities;
using Entity.Models;
using Entity.Services;
using Entity.UnitOfWorks;
using Entity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Service.Services;

namespace BookInventoryAndLoanTrackingSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize(Roles = "SuperAdmin,Admin")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBookService _bookService;
        private readonly IBookTypesService _bookTypesService;

        public HomeController(IBookService bookService, IBookTypesService bookTypesService, IUnitOfWork unitOfWork)
        {
            _bookService = bookService;
            _bookTypesService = bookTypesService;
            _unitOfWork = unitOfWork;
        }

        //public async Task<IActionResult> Index()
        //{
        //    var books = await _bookService.GetAllAsync();
        //    return View(books);
        //}
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
        public async Task<IActionResult> Create()
        {
            var model = new BookViewModel
            {
                AllBookTypes = (await _unitOfWork.GetRepository<BookTypes>().GetAllAsync()).ToList()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.AllBookTypes = (await _unitOfWork.GetRepository<BookTypes>().GetAllAsync()).ToList();
                return View(model);
            }

            await _bookService.AddAsync(model);
            return RedirectToAction("Home", "Admin");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await _bookService.GetByIdAsync(id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BookViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.AllBookTypes = (await _unitOfWork.GetRepository<BookTypes>().GetAllAsync()).ToList();
                return View(model);
            }

            await _bookService.UpdateAsync(model);
            return RedirectToAction("Home", "Admin");
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _bookService.DeleteAsync(id);
            return RedirectToAction("Home", "Admin");
        }

    }
}
