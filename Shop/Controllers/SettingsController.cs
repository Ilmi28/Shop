using Microsoft.AspNetCore.Mvc;
using Shop.Data;
using Shop.Models;
using Microsoft.AspNetCore.Authorization;
using Shop.ViewModels;
using Shop.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Text.Encodings.Web;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace Shop.Controllers
{
    [Authorize]
    public class SettingsController : Controller
    {
        private readonly DatabaseService _databaseService;
        private readonly AppIdentityDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly CryptographyService _cryptographyService;
        private ILogger<SettingsController> _logger;
        public SettingsController(AppIdentityDbContext context, 
            DatabaseService databaseService, 
            ILogger<SettingsController> logger, 
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            IEmailSender emailSender,
            CryptographyService cryptographyService) 
        {
            _databaseService = databaseService;
            _context = context;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _cryptographyService = cryptographyService;
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
        public IActionResult DeleteAccount()
        {
            return View();
        }
        public async Task<IActionResult> DeleteUserAccount(string token)
        {
            string deleteUserToken = Request.Cookies["deleteUserToken"];
            string decryptedToken = _cryptographyService.Decrypt(_cryptographyService.GetKey(), _cryptographyService.GetIV(), deleteUserToken);
            if(decryptedToken == token)
            {
                var user = _databaseService.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier));
                _signInManager.SignOutAsync();
                await _emailSender.SendEmailAsync(user.Email, "Account deleted", "Your account has been deleted successfully;(");
                _context.Users.Remove(user); 
                _context.SaveChanges();
                return Redirect($"/Settings/SuccessfulAccountDeleting/{decryptedToken}");
            }
            return NotFound();
        }
        [AllowAnonymous]
        public IActionResult SuccessfulAccountDeleting(string id)
        {
            string deleteUserToken = Request.Cookies["deleteUserToken"];
            if(deleteUserToken == null)
            {
                return NotFound();
            }
            string decryptedToken = _cryptographyService.Decrypt(_cryptographyService.GetKey(), _cryptographyService.GetIV(), deleteUserToken);
            if (decryptedToken == id)
            {
                CookieOptions options = new CookieOptions();
                options.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Append("deleteUserToken", id, options);
                return View();
            }
            return NotFound();
        }
        public async Task<RedirectResult> SendDeleteAccountEmail()
        {
            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddMinutes(15);
            var user = _databaseService.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier));
            string deleteUserToken = Guid.NewGuid().ToString();
            string originalToken = deleteUserToken;
            string encryptedToken = _cryptographyService.Encrypt(_cryptographyService.GetKey(), _cryptographyService.GetIV(), deleteUserToken);
            Response.Cookies.Append("deleteUserToken", encryptedToken, options);
            var url = Url.Action("DeleteUserAccount", "Settings", new { token = originalToken }, protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(user.Email, "Confirm deleting your account", $"<h3>Hello {user.Name}!</h3>" +
                $"<a href='{HtmlEncoder.Default.Encode(url)}'>Click here</a> to delete permanently your account");
            return Redirect("ConfirmAccountDeleting");
        }
        public IActionResult ConfirmAccountDeleting()
        {
            return View();
        }
        public async Task<IActionResult> ConfirmEmail(string userId)
        {
            var user = _databaseService.GetUser(userId);
            if(user != null)
            {
                return View();
            }
            return NotFound();
        }
        public async Task<IActionResult> SendEmailConfirmation(string userId)
        {
            var user = _databaseService.GetUser(userId);
            if(user == null)
            {
                return NotFound();
            }
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var url = Url.Action("ConfirmationEmail", "Settings", new { userId = userId, code = code }, protocol: Request.Scheme);
            await _emailSender.SendEmailAsync(user.Email, "Confirm your email", $"Click <a href='{HtmlEncoder.Default.Encode(url)}'>this link</a> to confirm your email!");
            return RedirectToAction("ConfirmEmail", new { userId = userId });
        }
        public async Task<IActionResult> ConfirmationEmail(string userId, string code)
        {
            var user = _databaseService.GetUser(userId);
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return RedirectToAction("SuccessfulEmailConfirmation", new { userId = userId });
            }
            return NotFound();
        }
        public IActionResult SuccessfulEmailConfirmation(string userId)
        {
            var user = _databaseService.GetUser(userId);
            if(user != null)
            {
                return View();
            }
            return NotFound();
        }
    }
}
