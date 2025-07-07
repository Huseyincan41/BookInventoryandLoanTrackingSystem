using Data.Identity;
using Entity.Services;
using Entity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookInventoryAndLoanTrackingSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ILogger<RoleController> _logger;
        public RoleController(IAccountService accountService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<RoleController> logger)
        {
            _accountService = accountService;
            this.userManager = userManager;
            this.signInManager = signInManager;
            _logger = logger;
        }
        //[Authorize(Roles = "SuperAdmin,Admin")]
        public async Task<IActionResult> Index()
        {
            var roles = await _accountService.GetAllRoles();
            return View(roles);
        }
        public IActionResult Login(string? ReturnUrl)
        {
            LoginViewModel model = new LoginViewModel()
            {
                ReturnUrl = ReturnUrl
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            string msg = await _accountService.FindByNameAsync(model);
            if (msg == "Kullanıcı bulunamadı!")
            {
                _logger.LogWarning("Giriş başarısız: Kullanıcı bulunamadı. Kullanıcı adı: {UserName}", model.UserName);
                ModelState.AddModelError("", msg);
                return View(model);
            }
            else if (msg == "OK")
            {
                _logger.LogInformation("Giriş başarılı. Kullanıcı adı: {UserName}", model.UserName);
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }
            else
            {
                _logger.LogWarning("Giriş başarısız: Şifre hatalı. Kullanıcı adı: {UserName}", model.UserName);
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
            }
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel model)
        {
            string msg = await _accountService.CreateRoleAsync(model);
            if (msg == "OK")
            {
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", msg);
            }
            return View(model);
        }
        public async Task<IActionResult> Edit(string id)
        {
            var model = await _accountService.GetAllUsersWithRole(id);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EditRoleViewModel model)
        {
            string msg = await _accountService.EditRoleListAsync(model);
            if (msg != "OK")
            {
                ModelState.AddModelError("", msg);
                return View(model);
            }
            return RedirectToAction("Edit", "Role", new { id = model.RoleId, area = "Admin" });
        }
        public async Task<IActionResult> Users()
        {
            var users = await _accountService.GetAllUsers();
            return View(users);
        }
        public async Task<IActionResult> EditUser(int id)
        {
            var model = await _accountService.GetUserByIdAsync(id);
            if (model == null)
            {
                TempData["ErrorMessage"] = "Kullanıcı bulunamadı!";
                return RedirectToAction("Users");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
            string result = await _accountService.UpdateUserAsync(model);
            if (result != "OK")
            {
                _logger.LogWarning("Kullanıcı güncellenemedi. Id: {UserId}, Hata: {Error}", model.Id, result);
                ModelState.AddModelError("", result);
                return View(model);
            }
            _logger.LogInformation("Kullanıcı güncellendi. Id: {UserId}, Kullanıcı Adı: {UserName}", model.Id, model.UserName);
            TempData["SuccessMessage"] = "Kullanıcı başarıyla güncellendi.";
            return RedirectToAction("Users");
        }

        public async Task<IActionResult> DeleteUser(int id)
        {
            string result = await _accountService.DeleteUserAsync(id);
            if (result == "OK")
            {
                _logger.LogInformation("Kullanıcı silindi. Id: {UserId}", id);
                TempData["SuccessMessage"] = "Kullanıcı silindi.";
            }
            else
            {
                _logger.LogWarning("Kullanıcı silinemedi. Id: {UserId}, Hata: {Error}", id, result);
                TempData["ErrorMessage"] = result;
            }

            return RedirectToAction("Users");
        }
    }
}
