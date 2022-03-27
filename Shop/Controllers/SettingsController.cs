using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;
using Microsoft.AspNetCore.Authorization;
using Shop.ViewModels;
using Shop.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Shop.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly DatabaseService _databaseService;
        private readonly AppIdentityDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private ILogger<SettingsController> _logger;
        public SettingsController(AppIdentityDbContext context, DatabaseService databaseService, ILogger<SettingsController> logger, UserManager<AppUser> userManager) 
        {
            _databaseService = databaseService;
            _context = context;
            _logger = logger;
            _userManager = userManager;
        }
        public IActionResult Index()
        {            
            return View();
        }
        public IActionResult ChangeUsername()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangeUsername([Bind("Name")]ChangeNameViewModel changeNameViewModel)
        {
            var user = _databaseService.GetUser(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            if (ModelState.IsValid)
            {
                user.Name = changeNameViewModel.Name;
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            _logger.LogError("Error");
            return View();
        }
        public IActionResult ChangeEmail()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangeEmail([Bind("Email")]ChangeEmailViewModel changeEmailViewModel)
        {
            var user = _databaseService.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (ModelState.IsValid)
            {
                user.Email = changeEmailViewModel.Email;
                user.NormalizedEmail = changeEmailViewModel.Email.ToUpper();
                user.UserName = changeEmailViewModel.Email;
                user.NormalizedUserName = changeEmailViewModel.Email.ToUpper();
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword([Bind("CurrentPassword, NewPassword, ConfirmPassword")] ChangePasswordViewModel changePasswordViewModel)
        {
            var user = _databaseService.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (ModelState.IsValid)
            {
                var result = await _userManager.ChangePasswordAsync(user, changePasswordViewModel.CurrentPassword, changePasswordViewModel.NewPassword);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User with id {id} has changed password successfully", user.Id);
                    return RedirectToAction("Index");
                }
                ModelState.AddModelError(String.Empty, "Wrong current password");
                _logger.LogError("User with id {id} can't change password", user.Id);
                return View();
            }
            ModelState.AddModelError(String.Empty, "You should correctly fill all fields");
            return View();
        }
    }
}
