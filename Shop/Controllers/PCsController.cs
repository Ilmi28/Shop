using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers
{
    public class PCsController : Controller
    {
        private readonly ILogger<PCsController> _logger;
        private readonly AppDbContext _context;
        public PCsController(ILogger<PCsController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }
        public IActionResult Index()
        {
            var PCs = _context.PCs.ToList();
            return View(PCs);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(_context.Categories, "Name", "Id");
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name, Price, Processor, GraphicCard, RAM")] PC model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            int categoryId = _context.Categories.FirstOrDefault(x => x.Name == "PCs").Id;
            model.CategoryId = categoryId;
            _context.PCs.Add(model);
            return RedirectToAction("Index");
        }
    }
}
