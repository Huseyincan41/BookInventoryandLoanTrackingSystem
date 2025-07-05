using Entity.Entities;
using Entity.Services;
using Entity.UnitOfWorks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Service.Services;

namespace BookInventoryAndLoanTrackingSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookTypeController : Controller
    {
        private readonly IBookTypesService _bookTypesService;
        private readonly IUnitOfWork _unitOfWork;

        public BookTypeController(IBookTypesService bookTypesService, IUnitOfWork unitOfWork)
        {
            _bookTypesService = bookTypesService;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            var bookTypes = await _bookTypesService.GetAllAsync();
            return View(bookTypes);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(BookTypes bookType)
        {
            //if (!ModelState.IsValid)
            //    return View(bookType);

            await _bookTypesService.AddAsync(bookType);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int id)
        {
            var bookType = await _bookTypesService.GetByIdAsync(id);
            if (bookType == null)
                return NotFound();

            return View(bookType);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(BookTypes bookType)
        {
            //if (!ModelState.IsValid)
            //    return View(bookType);

            await _bookTypesService.UpdateAsync(bookType);
            return RedirectToAction("Index");
        }
    }
}
