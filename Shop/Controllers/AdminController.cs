using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Microsoft.AspNetCore.Identity;

namespace Shop.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<AdminController> _logger;
        public AdminController(UserManager<AppUser> userManager, ILogger<AdminController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser()
                {
                    UserName = user.Name,
                    Email = user.Email
                };
                IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    _logger.LogError("Cannot create user with name {name} and email {}", user.Name, user.Email);
                }
            }
            return View(user);
        }
    }
}
