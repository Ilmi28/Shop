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
        private readonly AppIdentityDbContext _identityContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly CryptographyService _cryptographyService;
        private ILogger<SettingsController> _logger;
        private IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly AppDbContext _context;
        public SettingsController(AppIdentityDbContext identityContext, 
            AppDbContext context,
            DatabaseService databaseService, 
            ILogger<SettingsController> logger, 
            UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            IEmailSender emailSender,
            CryptographyService cryptographyService,
            IWebHostEnvironment env,
            IConfiguration config) 
        {
            _databaseService = databaseService;
            _identityContext = identityContext;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _cryptographyService = cryptographyService;
            _env = env;
            _config = config;
            _context = context;
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
                _identityContext.SaveChanges();
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
        public async Task<IActionResult> ChangeEmail([Bind("Email")]ChangeEmailViewModel changeEmailViewModel)
        {
            var user = _databaseService.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (ModelState.IsValid)
            {
                if(changeEmailViewModel.Email == user.Email)
                {
                    ModelState.AddModelError(String.Empty, "This email is already yours");
                    return View();
                }
                user.Email = changeEmailViewModel.Email;
                user.NormalizedEmail = changeEmailViewModel.Email.ToUpper();
                user.UserName = changeEmailViewModel.Email;
                user.NormalizedUserName = changeEmailViewModel.Email.ToUpper();
                user.EmailConfirmed = false;
                user.HasProStatus = false;
                await _identityContext.SaveChangesAsync();
                await _signInManager.RefreshSignInAsync(user);
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
            string? deleteUserToken = Request.Cookies["deleteUserToken"];
            string decryptedToken = _cryptographyService.Decrypt(_cryptographyService.GetKey(), _cryptographyService.GetIV(), deleteUserToken);
            if(decryptedToken == token)
            {
                var user = _databaseService.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier));
                var userProducts = _context.Products.Where(x => x.Creator == user.Id).ToList();
                var cartProducts = _context.CartProducts.Where(x => x.CartToken == user.CartToken).ToList();
                foreach(var cartProduct in cartProducts)
                {
                    _context.CartProducts.Remove(cartProduct);
                }
                foreach(var product in userProducts)
                {
                    _context.Products.Remove(product);
                }
                await _signInManager.SignOutAsync();
                await _emailSender.SendEmailAsync(user.Email, "Account deleted", "Your account has been deleted successfully;(");
                _identityContext.Users.Remove(user);
                await _context.SaveChangesAsync();
                await _identityContext.SaveChangesAsync();
                return Redirect($"/Settings/SuccessfulAccountDeleting/{decryptedToken}");
            }
            return NotFound();
        }
        [AllowAnonymous]
        public IActionResult SuccessfulAccountDeleting(string id)
        {
            string? deleteUserToken = Request.Cookies["deleteUserToken"];
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
        public IActionResult ConfirmEmail(string userId)
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
                user.HasProStatus = true;
                await _identityContext.SaveChangesAsync();
                await _signInManager.RefreshSignInAsync(user);
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
        public IActionResult ChangeProfileImage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult ChangeProfileImage(UserViewModel userViewModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("User don't fill form properly");
                return View();
            }
            string folderPath = Path.Combine(_env.WebRootPath, "img", "userImages");
            string? userPhoto = userViewModel.UserPhoto?.FileName;
            if(userPhoto == null)
            {
                _logger.LogWarning("User photo was null");
                return View();
            };
            var user = _databaseService.GetUser(User.FindFirstValue(ClaimTypes.NameIdentifier));
            string oldFileName = user.UserPhoto;
            string oldFilePath = Path.Combine(_env.WebRootPath, "img", "userImages", oldFileName);
            string? fileName = _databaseService.UploadFile(userViewModel.UserPhoto, folderPath, userPhoto);
            if(oldFileName != _config["DefaultUserImage"] && System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }
            user.UserPhoto = fileName;
            _identityContext.SaveChanges();
            return Redirect("/User/MyProfile");
        }
    }
}
