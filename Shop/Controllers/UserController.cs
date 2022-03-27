using Microsoft.AspNetCore.Mvc;
using Shop.Models;
using Microsoft.AspNetCore.Identity;
using Shop.ViewModels;
using Shop.Data;

namespace Shop.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<UserController> _logger;  
        private readonly SignInManager<AppUser> _signInManager;        
        public UserController(UserManager<AppUser> userManager, ILogger<UserController> logger, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _logger = logger;
            _signInManager = signInManager;            
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel user)
        {
            if (ModelState.IsValid)
            {
                string cartId = Guid.NewGuid().ToString();
                Response.Cookies.Append("cartToken", cartId);
                AppUser appUser = new AppUser()
                {
                    UserName = user.Email,
                    Name = user.Name,
                    Email = user.Email,
                    PasswordHash = user.Password,
                    CartToken = cartId
                };
                IdentityResult result = await _userManager.CreateAsync(appUser, user.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Use with nam {name} and email {email} has been created", user.Name, user.Email);
                    var res = await _signInManager.PasswordSignInAsync(user.Email, user.Password, false, false);
                    if (!res.Succeeded)
                    {
                        _logger.LogError("User with id {id} cannot log in", appUser.Id);
                    }
                    return Redirect("/Home/Index");
                }
                else
                {
                    _logger.LogError("Cannot create user with name {name} and email {email}", user.Name, user.Email);
                }
            }
            return View();
        }
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(SignInViewModel user)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User with email {email} has logged in", user.Email);
                    return RedirectToAction("SetCookie");
                }
                ModelState.AddModelError(String.Empty, "Wrong email or password");
                _logger.LogInformation("Logging in user {email} has failed", user.Email);
            }
            else
            {
                ModelState.AddModelError(String.Empty, "Please enter your email and password");
            }
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/Home/Index");
        }
        public async Task<RedirectResult> SetCookie()
        {
            Response.Cookies.Append("cartToken", User.FindFirst("CartToken")?.Value);
            return Redirect("/Home/Index");
        }
    }
}
