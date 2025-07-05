using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Service.Services;

namespace BookInventoryAndLoanTrackingSystem.Controllers
{
    public class BookController : Controller
    {
        private readonly BorrowService _borrowService;
        private readonly UserManager<IdentityUser> _userManager;

        public BookController(BorrowService borrowService, UserManager<IdentityUser> userManager)
        {
            _borrowService = borrowService;
            _userManager = userManager;
        }
        //[HttpPost]
        //public async Task<IActionResult> Borrow(int id)
        //{
        //    var userId = _userManager.GetUserId(User);
        //    var success = await _borrowService.BorrowBookAsync(id, userId);

        //    if (!success)
        //        TempData["Error"] = "Book is not available.";
        //    else
        //        TempData["Success"] = "Book borrowed successfully.";

        //    return RedirectToAction("Index");
        //}
        public IActionResult Index()
        {
            return View();
        }
    }
}
