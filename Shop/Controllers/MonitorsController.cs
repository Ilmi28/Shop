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
            Response.Cookies.Append("previousUrl", Request.Path);
            var monitors = _databaseService.GetMonitors();
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
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Resolution,Refreshening,Category,CategoryId,MonitorPhoto,DefaultPhoto")] MonitorViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            //You can change default photo name in the appsettings.json file
            //After changing photo name in config file you should also upload your own photo file with name which should be the same as in the config
            string defaultMonitorImage = _configuration["DefaultMonitorImage"];
            string folderPath = Path.Combine(_env.WebRootPath, "img", "monitorImages");
            string? fileName = viewModel.MonitorPhoto?.FileName;
            string photoFileName = _databaseService.UploadFile(viewModel.MonitorPhoto, folderPath, fileName);
            if (photoFileName == null)
            {
                photoFileName = defaultMonitorImage;
            }
            var user = _databaseService.GetUser(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var monitor = new Models.Monitor
            {
                Name = viewModel.Name,
                Price = viewModel.Price,
                Resolution = viewModel.Resolution,
                Refreshening = viewModel.Refreshening,
                CategoryId = _context.Categories.FirstOrDefault(x => x.Name == "Monitors").Id,
                DefaultPhoto = viewModel.DefaultPhoto,
                MonitorPhoto = photoFileName,
                Creator = user.Id
            };
            if (monitor.DefaultPhoto == true)
            {
                monitor.MonitorPhoto = defaultMonitorImage;
                _logger.LogInformation("Default photo has been set for monitor with name {monitorName} and id {monitorId}", monitor.Name, monitor.Id);
            }
            _context.Monitors.Add(monitor);
            await _context.SaveChangesAsync();
            var product = new Models.Product
            {
                ProductNativeId = monitor.Id,
                ProductName = monitor.Name,
                ProductPhoto = monitor.MonitorPhoto,
                ProductPrice = monitor.Price,
                ProductCategoryId = monitor.CategoryId,
                Creator = monitor.Creator
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            _logger.LogInformation("New monitor has been created with name {MonitorName} and id {monitorId}", monitor.Name, monitor.Id);
            return RedirectToAction("Index");
        }
        [Authorize("ProStatusOnly")]
        public async Task<IActionResult> Delete(int id)
        {
            var monitor = _databaseService.GetMonitorById(id);
            var product = _databaseService.GetProductMonitorByIdAndCategory(id);
            var isProductOwner = await _authService.AuthorizeAsync(User, product, "ProductOwnerOnly");
            if (!isProductOwner.Succeeded)
            {
                return Redirect("/User/AccessDenied");
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
            var product = _databaseService.GetProductMonitorByIdAndCategory(id.Value);
            var isProductOwner = await _authService.AuthorizeAsync(User, product, "ProductOwnerOnly");
            if (!isProductOwner.Succeeded)
            {
                return Redirect("/User/AccessDenied");
            }
            var cartProduct = _context.CartProducts.FirstOrDefault(x => x.CartToken == cartId && x.ProductId == product.Id);
            string oldFilePath = Path.Combine(_env.WebRootPath, "img", "monitorImages", monitor.MonitorPhoto);
            string defaultMonitorImage = _configuration["DefaultMonitorImage"];
            string defaultFilePath = Path.Combine(_env.WebRootPath, "img", "monitorImages", defaultMonitorImage);
            if (System.IO.File.Exists(oldFilePath) && oldFilePath != defaultFilePath)
            {
                System.IO.File.Delete(oldFilePath);
                _logger.LogWarning("Photo with name {photoName} from path {path} has been deleted", monitor.MonitorPhoto, oldFilePath);
            }
            _context.Monitors.Remove(monitor);
            _context.Products.Remove(product);
            if (cartProduct != null)
            {
                _context.CartProducts.Remove(cartProduct);
            }
            await _context.SaveChangesAsync();
            _logger.LogInformation("Changes saved");
            _logger.LogWarning("Monitor with name {monitorName} and id {monitorId} has been deleted permanently", monitor.Name, monitor.Id);
            return RedirectToAction("Index");
        }
        [Authorize("ProStatusOnly")]
        public async Task<IActionResult> Edit(int? id)
        {
            var monitor = _databaseService.GetMonitorById(id);
            var product = _databaseService.GetProductMonitorByIdAndCategory(id.Value);
            var isProductOwner = await _authService.AuthorizeAsync(User, product, "ProductOwnerOnly");
            if (!isProductOwner.Succeeded)
            {
                return Redirect("/User/AccessDenied");
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
            };
            return View(monitorViewModel);
        }

        [HttpPost]
        [Authorize("ProStatusOnly")]
        public async Task<IActionResult> Edit([Bind("Id,Name,Price,Resolution,Refreshening,MonitorPhoto,DefaultPhoto")] MonitorViewModel viewModel)
        {
            var monitor = _databaseService.GetMonitorById(viewModel.Id);
            var product = _databaseService.GetProductMonitorByIdAndCategory(viewModel.Id);
            var isProductOwner = await _authService.AuthorizeAsync(User, product, "ProductOwnerOnly");
            if (!isProductOwner.Succeeded)
            {
                return Redirect("/User/AccessDenied");
            }
            string? cartId = Request.Cookies["cartToken"];
            var cartProducts = _context.CartProducts.Where(x => x.ProductId == product.Id).ToList();
            if (!ModelState.IsValid)
            {
                return View();
            }
            string defaultMonitorImage = _configuration["DefaultMonitorImage"];
            string oldFilePath = Path.Combine(_env.WebRootPath, "img", "monitorImages", monitor.MonitorPhoto);
            string newFilePath = Path.Combine(_env.WebRootPath, "img", "monitorImages");
            string? fileName = viewModel.MonitorPhoto?.FileName;
            string defaultFilePath = Path.Combine(_env.WebRootPath, "img", "monitorImages", defaultMonitorImage);
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
            var monitorUpdated = new Models.Monitor
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Price = viewModel.Price,
                Resolution = viewModel.Resolution,
                Refreshening = viewModel.Refreshening,
                DefaultPhoto = viewModel.DefaultPhoto,
                CategoryId = monitor.CategoryId,
                MonitorPhoto = photoFileName,
                Creator = monitor.Creator
            };
            if(monitorUpdated.MonitorPhoto == defaultMonitorImage)
            {
                monitorUpdated.DefaultPhoto = true;
            }
            var productUpdated = new Models.Product
            {
                Id = product.Id,
                ProductNativeId = monitorUpdated.Id,
                ProductName = monitorUpdated.Name,
                ProductPhoto = monitorUpdated.MonitorPhoto,
                ProductPrice = monitorUpdated.Price,
                ProductCategoryId = monitorUpdated.CategoryId,
                Creator = monitor.Creator
            };
            _context.Monitors.Remove(monitor);
            _context.Products.Remove(product);
            _context.Monitors.Add(monitorUpdated);
            _context.Products.Add(productUpdated);
            foreach (var item in cartProducts)
            {
                var cartProductUpdated = new Models.CartProduct
                {
                    Name = productUpdated.ProductName,
                    CartToken = item.CartToken,
                    Quantity = item.Quantity,
                    Photo = productUpdated.ProductPhoto,
                    Price = productUpdated.ProductPrice,
                    ProductId = item.ProductId
                };
                _context.CartProducts.Remove(item);
                _context.CartProducts.Add(cartProductUpdated);
            }
            _logger.LogInformation("Monitor with id {id} and name {name} has been updated!", monitor.Id, monitor.Name);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Details(int? id)
        {
            Response.Cookies.Append("previousUrl", Request.Path);
            var monitor = _databaseService.GetMonitorById(id);
            return View(monitor);
        }
    }
}
