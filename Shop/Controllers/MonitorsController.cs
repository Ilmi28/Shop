using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop.Data;
using Shop.Services;
using Shop.ViewModels;
using System.Security.Claims;

namespace Shop.Controllers
{
    public class MonitorsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MonitorsController> _logger;
        private readonly DatabaseService _databaseService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        private readonly IAuthorizationService _authService;
        public MonitorsController(AppDbContext context, ILogger<MonitorsController> logger, DatabaseService databaseService, IWebHostEnvironment env, IConfiguration configuration
            , IAuthorizationService authService)
        {
            _context = context;
            _logger = logger;
            _databaseService = databaseService;
            _env = env;
            _configuration = configuration;
            _authService = authService; 
        }
        public IActionResult Index()
        {
            
            var monitors = _context.Monitors.ToList();
            _logger.LogInformation("{amountOfMonitors} monitors have been displayed on page", monitors.Count);
            return View(monitors);
        }
        [Authorize("ProStatusOnly")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize("ProStatusOnly")]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Resolution,Refreshening,MonitorPhoto,DefaultPhoto,Stock")] MonitorViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            //You can change default photo name in the appsettings.json file
            //After changing photo name in config file you should also upload your own photo file with name which should be the same as in the config
            string defaultMonitorImage = _configuration["DefaultMonitorImage"];
            string folderPath = Path.Combine(_env.WebRootPath, "img", "monitorsImages");
            string? fileName = viewModel.MonitorPhoto?.FileName;
            string photoFileName = _databaseService.UploadFile(viewModel.MonitorPhoto, folderPath, fileName);
            if (photoFileName == null)
            {
                photoFileName = defaultMonitorImage;
            }
            var user = _databaseService.GetUser(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var product = new Models.Product
            {
                ProductName = viewModel.Name,
                ProductPhoto = photoFileName,
                ProductPrice = viewModel.Price,
                ProductCategoryId = _context.Categories.FirstOrDefault(x => x.Name == "Monitors").Id,
                Creator = user.Id,
                Created = DateTime.Now,
                Stock = viewModel.Stock,
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            var monitor = new Models.Monitor
            {
                Name = viewModel.Name,
                Price = viewModel.Price,
                Resolution = viewModel.Resolution,
                Refreshening = viewModel.Refreshening,
                CategoryId = _context.Categories.FirstOrDefault(x => x.Name == "Monitors").Id,
                DefaultPhoto = viewModel.DefaultPhoto,
                MonitorPhoto = photoFileName,
                Creator = user.Id,
                Created = DateTime.Now,
                Stock = viewModel.Stock,
                ProductId = product.Id
            };
            if (monitor.DefaultPhoto == true)
            {
                monitor.MonitorPhoto = defaultMonitorImage;
                _logger.LogInformation("Default photo has been set for monitor with name {monitorName} and id {monitorId}", monitor.Name, monitor.Id);
            }
            _context.Monitors.Add(monitor);
            await _context.SaveChangesAsync();
            product.ProductNativeId = monitor.Id;
            await _context.SaveChangesAsync();
            _logger.LogInformation("New monitor has been created with name {MonitorName} and id {monitorId}", monitor.Name, monitor.Id);
            return RedirectToAction("Index");
        }
        [Authorize("ProStatusOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var monitor = _databaseService.GetMonitorById(id);
            var product = _databaseService.GetProductByMonitorNativeId(id);
            var isProductOwner = await _authService.AuthorizeAsync(User, product, "ProductOwnerOnly");
            if (!isProductOwner.Succeeded)
            {
                return Redirect("/Error/AccessDenied");
            }
            if(monitor == null)
            {
                return NotFound();
            }
            MonitorViewModel monitorViewModel = new MonitorViewModel
            {
                Name = monitor.Name
            };
            return View(monitorViewModel);
        }

        [HttpPost]
        [Authorize("ProStatusOnly")]
        public async Task<IActionResult> Delete(int? id)
        {
            string? cartId = Request.Cookies["cartToken"];
            var monitor = _databaseService.GetMonitorById(id.Value);
            var product = _databaseService.GetProductByMonitorNativeId(id.Value);
            var isProductOwner = await _authService.AuthorizeAsync(User, product, "ProductOwnerOnly");
            if (!isProductOwner.Succeeded)
            {
                return Redirect("/Error/AccessDenied");
            }
            var cartProduct = _context.CartProducts.FirstOrDefault(x => x.CartToken == cartId && x.ProductId == product.Id);
            string oldFilePath = Path.Combine(_env.WebRootPath, "img", "monitorsImages", monitor.MonitorPhoto);
            string defaultMonitorImage = _configuration["DefaultMonitorImage"];
            string defaultFilePath = Path.Combine(_env.WebRootPath, "img", "monitorsImages", defaultMonitorImage);
            if (System.IO.File.Exists(oldFilePath) && oldFilePath != defaultFilePath)
            {
                System.IO.File.Delete(oldFilePath);
                _logger.LogWarning("Photo with name {photoName} from path {path} has been deleted", monitor.MonitorPhoto, oldFilePath);
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Changes saved");
            _logger.LogWarning("Monitor with name {monitorName} and id {monitorId} has been deleted permanently", monitor.Name, monitor.Id);
            return Redirect("/User/MyProfile");
        }
        [Authorize("ProStatusOnly")]
        public async Task<IActionResult> Edit(int? id)
        {
            var monitor = _databaseService.GetMonitorById(id);
            var product = _databaseService.GetProductByMonitorNativeId(id);
            var isProductOwner = await _authService.AuthorizeAsync(User, product, "ProductOwnerOnly");
            if (!isProductOwner.Succeeded)
            {
                return Redirect("/Error/AccessDenied");
            }
            if (monitor == null)
            {
                return NotFound();
            }
            var monitorViewModel = new MonitorViewModel
            {
                Id = monitor.Id,
                Name = monitor.Name,
                Price = monitor.Price,
                Resolution = monitor.Resolution,
                Refreshening = monitor.Refreshening,
                DefaultPhoto = monitor.DefaultPhoto,
                Stock = monitor.Stock
            };
            return View(monitorViewModel);
        }

        [HttpPost]
        [Authorize("ProStatusOnly")]
        public async Task<IActionResult> Edit([Bind("Id,Name,Price,Resolution,Refreshening,MonitorPhoto,DefaultPhoto,Stock")] MonitorViewModel viewModel)
        {
            var monitor = _databaseService.GetMonitorById(viewModel.Id);
            var product = _databaseService.GetProductByMonitorNativeId(viewModel.Id);
            var isProductOwner = await _authService.AuthorizeAsync(User, product, "ProductOwnerOnly");
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
            string defaultMonitorImage = _configuration["DefaultMonitorImage"];
            string oldFilePath = Path.Combine(_env.WebRootPath, "img", "monitorsImages", monitor.MonitorPhoto);
            string newFilePath = Path.Combine(_env.WebRootPath, "img", "monitorsImages");
            string? fileName = viewModel.MonitorPhoto?.FileName;
            string defaultFilePath = Path.Combine(_env.WebRootPath, "img", "monitorsImages", defaultMonitorImage);
            string? photoFileName = _databaseService.UploadFile(viewModel.MonitorPhoto, newFilePath, fileName);

            if (oldFilePath != defaultFilePath && (viewModel.DefaultPhoto || (!viewModel.DefaultPhoto && fileName != null)))
            {
                System.IO.File.Delete(oldFilePath);
                _logger.LogWarning("Photo with name {photoName} form path {photoPath}", monitor.MonitorPhoto, oldFilePath);
            }
            if (photoFileName == null && viewModel.DefaultPhoto)
            {
                photoFileName = defaultMonitorImage;
                _logger.LogInformation("Photo for monitor with name {monitorName} and id {monitorId} has been updated to default photo", monitor.Name, monitor.Id);
            }
            if (photoFileName == null && !viewModel.DefaultPhoto)
            {
                photoFileName = monitor.MonitorPhoto;
            }
            monitor.Name = viewModel.Name;
            monitor.Price = viewModel.Price;
            monitor.Stock = viewModel.Stock;
            monitor.Resolution = viewModel.Resolution;
            monitor.Refreshening = viewModel.Refreshening;
            monitor.DefaultPhoto = viewModel.DefaultPhoto;
            monitor.MonitorPhoto = photoFileName;

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
            _logger.LogInformation("Monitor with id {id} and name {name} has been updated!", monitor.Id, monitor.Name);
            await _context.SaveChangesAsync();
            return Redirect("/User/MyProfile");
        }
        public IActionResult Details(int? id)
        {
            
            var monitor = _databaseService.GetMonitorById(id);
            return View(monitor);
        }
    }
}
