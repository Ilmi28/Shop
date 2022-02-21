using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop.Data;
using Shop.Services;
using Shop.ViewModels;

namespace Shop.Controllers
{
    public class MonitorsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ILogger<MonitorsController> _logger;
        private readonly DatabaseService _databaseService;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _configuration;
        public MonitorsController(AppDbContext context, ILogger<MonitorsController> logger, DatabaseService databaseService, IWebHostEnvironment env, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _databaseService = databaseService;
            _env = env;
            _configuration = configuration;
        }
        public IActionResult Index()
        {
            var monitors = _databaseService.GetMonitors();
            _logger.LogInformation("{amountOfMonitors} monitors have been displayed on page", monitors.Count);
            return View(monitors);
        }

        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Resolution,Refreshening,Category,CategoryId,MonitorPhoto,DefaultPhoto")] MonitorViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            //You can change default photo name in the appsettings.json file
            //After changing photo name in config file you should also upload your own photo file with name which should be same as in the config
            string defaultMonitorImage = _configuration["DefaultMonitorImage"];
            string photoFileName = UploadFile(viewModel);
            if (photoFileName == null)
            {
                photoFileName = defaultMonitorImage;
            }
            var monitor = new Models.Monitor
            {
                Name = viewModel.Name,
                Price = viewModel.Price,
                Resolution = viewModel.Resolution,
                Refreshening = viewModel.Refreshening,
                CategoryId = _context.Categories.FirstOrDefault(x => x.Name == "Monitors")?.Id,
                DefaultPhoto = viewModel.DefaultPhoto,
                MonitorPhoto = photoFileName
            };
            if (monitor.DefaultPhoto == true)
            {
                monitor.MonitorPhoto = defaultMonitorImage;
                _logger.LogInformation("Default photo has been set for monitor with name {monitorName} and id {monitorId}", monitor.Name, monitor.Id);
            }
            _context.Monitors.Add(monitor);
            await _context.SaveChangesAsync();
            _logger.LogInformation("New monitor has been created with name {MonitorName} and id {monitorId}", monitor.Name, monitor.Id);
            return RedirectToAction("Index");
        }
        //uploading photo file to wwwroot/img folder
        private string UploadFile(MonitorViewModel viewModel)
        {
            string fileName = null;
            if (viewModel.MonitorPhoto != null)
            {
                string imgPath = Path.Combine(_env.WebRootPath, "img");
                fileName = Guid.NewGuid() + viewModel.MonitorPhoto.FileName;
                string filePath = Path.Combine(imgPath, fileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    viewModel.MonitorPhoto.CopyTo(fileStream);
                    _logger.LogInformation("New photo file has been created with name {photoName} and path {path}", fileName, filePath);
                }
            }
            return fileName;
        }
        public IActionResult Delete(int id)
        {
            var monitor = _databaseService.GetMonitorById(id);
            MonitorViewModel monitorViewModel = new MonitorViewModel
            {
                Name = monitor.Name
            };
            return View(monitorViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id)
        {
            var monitor = _databaseService.GetMonitorById(id.Value);
            string oldFilePath = Path.Combine(_env.WebRootPath, "img", monitor.MonitorPhoto);
            string defaultMonitorImage = _configuration["DefaultMonitorImage"];
            string defaultFilePath = Path.Combine(_env.WebRootPath, "img", defaultMonitorImage);
            if (System.IO.File.Exists(oldFilePath) && oldFilePath != defaultFilePath)
            {
                System.IO.File.Delete(oldFilePath);
                _logger.LogWarning("Photo with name {photoName} from path {path} has been deleted", monitor.MonitorPhoto, oldFilePath);
            }
            _context.Monitors.Remove(monitor);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Changes saved");
            _logger.LogWarning("Monitor with name {monitorName} and id {monitorId} has been deleted permanently", monitor.Name, monitor.Id);
            return RedirectToAction("Index");
        }
        public IActionResult Edit(int? id)
        {
            var monitor = _databaseService.GetMonitorById(id);
            if (monitor == null)
            {
                return NotFound();
            }
            var monitorViewModel = new MonitorViewModel
            {
                Name = monitor.Name,
                Price = monitor.Price,
                Resolution = monitor.Resolution,
                Refreshening = monitor.Refreshening,
                DefaultPhoto = monitor.DefaultPhoto,
            };
            return View(monitorViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Edit([Bind("Id,Name,Price,Resolution,Refreshening,MonitorPhoto,DefaultPhoto")] MonitorViewModel viewModel)
        {
            var monitor = _databaseService.GetMonitorById(viewModel.Id);
            if (!ModelState.IsValid)
            {
                return View();
            }
            string defaultMonitorImage = _configuration["DefaultMonitorImage"];
            string oldFilePath = Path.Combine(_env.WebRootPath, "img", monitor.MonitorPhoto);
            string defaultFilePath = Path.Combine(_env.WebRootPath, "img", defaultMonitorImage);
            string fileName = UploadFile(viewModel);

            if (System.IO.File.Exists(oldFilePath) && oldFilePath != defaultFilePath && viewModel.DefaultPhoto)
            {
                System.IO.File.Delete(oldFilePath);
                _logger.LogWarning("Photo with name {photoName} form path {photoPath}", monitor.MonitorPhoto, oldFilePath);
            }
            if (fileName == null)
            {
                fileName = defaultMonitorImage;
                _logger.LogInformation("Photo for monitor with name {monitorName} and id {monitorId} has been updated to default photo", monitor.Name, monitor.Id);
            }
            if (!viewModel.DefaultPhoto && monitor.MonitorPhoto != defaultMonitorImage)
            {
                fileName = monitor.MonitorPhoto;
            }
            var monitorUpdated = new Models.Monitor
            {
                Id = viewModel.Id,
                Name = viewModel.Name,
                Price = viewModel.Price,
                Resolution = viewModel.Resolution,
                Refreshening = viewModel.Refreshening,
                DefaultPhoto = viewModel.DefaultPhoto,
                MonitorPhoto = fileName
            };
            _context.Monitors.Remove(monitor);
            _context.Monitors.Add(monitorUpdated);
            _logger.LogInformation("Monitor with id {id} and name {name} has been updated!", monitor.Id, monitor.Name);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public IActionResult Details(int? id)
        {
            var monitor = _databaseService.GetMonitorById(id);
            return View(monitor);
        }
    }
}
