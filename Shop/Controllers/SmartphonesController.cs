using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.ViewModels;
using Shop.Data;
using Shop.Services;
using System.Security.Claims;

namespace Shop.Controllers
{
    public class SmartphonesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;
        private readonly DatabaseService _databaseService;
        private readonly ILogger<SmartphonesController> _logger;
        private readonly IAuthorizationService _authorizationService;
        public SmartphonesController(AppDbContext context, IWebHostEnvironment env, IConfiguration config, DatabaseService databaseService, 
            ILogger<SmartphonesController> logger, IAuthorizationService authorizationService)
        {
            _context = context;
            _env = env;
            _config = config;
            _databaseService = databaseService;
            _logger = logger;
            _authorizationService = authorizationService;
        }
        public IActionResult Index()
        {
            
            var smartphones = _context.Smartphones.ToList();
            return View(smartphones);
        }
        [Authorize("ProStatusOnly")]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize("ProStatusOnly")]
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Memory,RAM,ScreenDiagonal,CameraResolution,DefaultPhoto,SmartphonePhoto,Stock")] SmartphoneViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            //You can change default photo name in the appsettings.json file
            //After changing photo name in config file you should also upload your own photo file with name which should be the same as in the config
            string defaultSmartphoneImage = _config["DefaultSmartphoneImage"];
            string folderPath = Path.Combine(_env.WebRootPath, "img", "smartphonesImages");
            string? fileName = viewModel.SmartphonePhoto?.FileName;
            string photoFileName = _databaseService.UploadFile(viewModel.SmartphonePhoto, folderPath, fileName);
            if (photoFileName == null)
            {
                photoFileName = defaultSmartphoneImage;
            }
            var user = _databaseService.GetUser(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var product = new Models.Product
            {
                ProductName = viewModel.Name,
                ProductPhoto = photoFileName,
                ProductPrice = viewModel.Price,
                ProductCategoryId = _context.Categories.FirstOrDefault(x => x.Name == "Smartphones").Id,
                Creator = user.Id,
                Created = DateTime.Now,
                Stock = viewModel.Stock,
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            _logger.LogInformation("New product has been created with name {productName} and id {productId}", product.ProductName, product.Id);
            var smartphone = new Models.Smartphone
            {
                Name = product.ProductName,
                Price = product.ProductPrice,
                RAM = viewModel.RAM,
                CameraResolution = viewModel.CameraResolution,
                CategoryId = product.ProductCategoryId,
                DefaultPhoto = viewModel.DefaultPhoto,
                SmartphonePhoto = photoFileName,
                Stock = product.Stock,
                ScreenDiagonal = viewModel.ScreenDiagonal,
                Created = DateTime.Now,
                Creator = user.Id,
                Memory = viewModel.Memory,
                ProductId = product.Id
            };
            if (smartphone.DefaultPhoto == true)
            {
                smartphone.SmartphonePhoto = defaultSmartphoneImage;
            }
            _context.Smartphones.Add(smartphone);
            await _context.SaveChangesAsync();
            product.ProductNativeId = smartphone.Id;
            await _context.SaveChangesAsync();
            return Redirect("/User/MyProfile");
        }
        [Authorize("ProStatusOnly")]
        public async Task<IActionResult> Edit(int? id)
        {
            var smartphone = await _context.Smartphones.FindAsync(id);
            var product = _databaseService.GetProductBySmartphoneNativeId(id);
            var isProductOwner = await _authorizationService.AuthorizeAsync(User, product, "ProductOwnerOnly");
            if (!isProductOwner.Succeeded)
            {
                return Redirect("/Error/AccessDenied");
            }
            if(smartphone == null)
            {
                return NotFound();
            }
            SmartphoneViewModel viewModel = new SmartphoneViewModel
            {
                Id = smartphone.Id,
                Name = smartphone.Name,
                Price = smartphone.Price,
                CameraResolution = smartphone.CameraResolution,
                ScreenDiagonal = smartphone.ScreenDiagonal,
                Memory = smartphone.Memory,
                RAM = smartphone.RAM,
                Stock = smartphone.Stock,
                DefaultPhoto = smartphone.DefaultPhoto,
            };
            return View(viewModel);
        }
        [HttpPost]
        [Authorize("ProStatusOnly")]
        public async Task<IActionResult> Edit([Bind("Id,Name,Price,Memory,RAM,ScreenDiagonal,CameraResolution,DefaultPhoto,SmartphonePhoto,Stock")] SmartphoneViewModel viewModel)
        {
            var smartphone = _context.Smartphones.FirstOrDefault(x => x.Id == viewModel.Id);
            var product = _databaseService.GetProductBySmartphoneNativeId(viewModel.Id);
            var isProductOwner = await _authorizationService.AuthorizeAsync(User, product, "ProductOwnerOnly");
            if (!isProductOwner.Succeeded)
            {
                return Redirect("/Error/AccessDenied");
            }
            string? cartId = Request.Cookies["cartToken"];
            var cartProducts = _context.CartProducts.Where(x => x.ProductId == product.Id).ToList();
            if (!ModelState.IsValid)
            {
                return View();
            }
            string defaultSmartphoneImage = _config["DefaultSmartphoneImage"];
            string oldFilePath = Path.Combine(_env.WebRootPath, "img", "smartphonesImages", smartphone.SmartphonePhoto);
            string newFilePath = Path.Combine(_env.WebRootPath, "img", "smartphonesImages");
            string? fileName = viewModel.SmartphonePhoto?.FileName;
            string defaultFilePath = Path.Combine(_env.WebRootPath, "img", "smartphonesImages", defaultSmartphoneImage);
            string? photoFileName = _databaseService.UploadFile(viewModel.SmartphonePhoto, newFilePath, fileName);

            if (oldFilePath != defaultFilePath && (viewModel.DefaultPhoto || (!viewModel.DefaultPhoto && fileName != null)))
            {
                System.IO.File.Delete(oldFilePath);
                _logger.LogWarning("Photo with name {photoName} from path {photoPath} has been deleted", smartphone.SmartphonePhoto, oldFilePath);
            }
            if (photoFileName == null && viewModel.DefaultPhoto)
            {
                photoFileName = defaultSmartphoneImage;
                _logger.LogInformation("Photo for smartphone with name {monitorName} and id {monitorId} has been updated to default photo", smartphone.Name, smartphone.Id);
            }
            if (photoFileName == null && !viewModel.DefaultPhoto)
            {
                photoFileName = smartphone.SmartphonePhoto;
            }
            smartphone.Name = viewModel.Name;
            smartphone.Price = viewModel.Price;
            smartphone.Memory = viewModel.Memory;
            smartphone.RAM = viewModel.Memory;
            smartphone.Stock = viewModel.Stock;
            smartphone.ScreenDiagonal = viewModel.ScreenDiagonal;
            smartphone.CameraResolution = viewModel.CameraResolution;
            smartphone.DefaultPhoto = viewModel.DefaultPhoto;
            smartphone.SmartphonePhoto = photoFileName;

            product.ProductName = viewModel.Name;
            product.ProductPrice = viewModel.Price;
            product.ProductPhoto = photoFileName;
            product.Stock = viewModel.Stock;

            foreach (var cartProduct in cartProducts)
            {
                cartProduct.Name = viewModel.Name;
                cartProduct.Price = viewModel.Price;
                cartProduct.Photo = photoFileName;
            }
            _logger.LogInformation("Smartphone with id {id} and name {name} has been updated!", smartphone.Id, smartphone.Name);
            await _context.SaveChangesAsync();
            return Redirect("/User/MyProfile");
        }
        public IActionResult Delete(int id)
        {
            var smartphone = _context.Smartphones.FirstOrDefault(x => x.Id == id);
            ViewBag.Name = smartphone.Name;
            return View();
        }
        [HttpPost]
        [Authorize("ProStatusOnly")]
        public async Task<IActionResult> Delete(int? id)
        {
            var smartphone = _context.Smartphones.FirstOrDefault(x => x.Id == id);
            var product = _databaseService.GetProductBySmartphoneNativeId(smartphone.Id);
            var isProductOwner = await _authorizationService.AuthorizeAsync(User, product, "ProductOwnerOnly");
            if (!isProductOwner.Succeeded)
            {
                return Redirect("/Error/AccessDenied");
            }
            string oldFilePath = Path.Combine(_env.WebRootPath, "img", "smartphonesImages", smartphone.SmartphonePhoto);
            string defaultSmartphoneImage = _config["DefaultSmartphoneImage"];
            string defaultFilePath = Path.Combine(_env.WebRootPath, "img", "smartphonesImages", defaultSmartphoneImage);
            if (System.IO.File.Exists(oldFilePath) && oldFilePath != defaultFilePath)
            {
                System.IO.File.Delete(oldFilePath);
                _logger.LogWarning("Photo with name {photoName} from path {path} has been deleted", smartphone.SmartphonePhoto, oldFilePath);
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Changes saved");
            _logger.LogWarning("Smartphone with name {monitorName} and id {monitorId} has been deleted permanently", smartphone.Name, smartphone.Id);
            return Redirect("/User/MyProfile");
        }
        public IActionResult Details(int id)
        {
            var smartphone = _context.Smartphones.FirstOrDefault(x => x.Id == id);
            return View(smartphone);
        }
    }
}
