using Entity.Services;
using Entity.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BookInventoryAndLoanTrackingSystem.Controllers
{
	public class AccountController : Controller
	{
		private readonly IAccountService _service;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAccountService service, ILogger<AccountController> logger)
        {
            _service = service;
            _logger = logger;
        }

        public IActionResult Index()
		{
			return View();
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
			string msg = await _service.FindByNameAsync(model);
			if (msg == "Kullanıcı bulunamadı!")
			{
                _logger.LogWarning("Giriş başarısız: kullanıcı bulunamadı ({UserName})", model.UserName);
                ModelState.AddModelError("", msg);
				return View(model);
			}
			else if (msg == "OK")
			{
                _logger.LogInformation("Kullanıcı giriş yaptı: {UserName}", model.UserName);
               
                return RedirectToAction("Index","Home");
            }
			else
			{
                _logger.LogWarning("Giriş başarısız: şifre hatalı ({UserName})", model.UserName);
                ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı");
			}
			return View(model);
		}
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterViewModel model)
		{
			string msg = await _service.CreateUserAsync(model);
			if (msg == "OK")
			{
				return RedirectToAction("Login");
			}
			else
			{
				ModelState.AddModelError("", msg);
			}
			return View(model);
		}
        public async Task<IActionResult> Logout()
        {
            var userName = User.Identity?.Name ?? "Anonim";
            _logger.LogInformation("Kullanıcı çıkış yaptı: {UserName}", userName);
            await _service.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
