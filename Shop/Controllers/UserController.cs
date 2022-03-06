using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Microsoft.AspNetCore.Identity;

namespace Shop.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<UserController> _logger;
        public UserController(UserManager<AppUser> userManager, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            if (ModelState.IsValid)
            {
                AppUser appUser = new AppUser()
                {
                    UserName = user.Name,
                    Email = user.Email,
                    PasswordHash = user.Password
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
