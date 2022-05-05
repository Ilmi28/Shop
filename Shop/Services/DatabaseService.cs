using Shop.Data;

namespace Shop.Services
{
    public class DatabaseService
    {
        private readonly AppDbContext _context;
        private readonly AppIdentityDbContext _identityContext;
        private readonly ILogger<DatabaseService> _logger;
        public DatabaseService(AppDbContext context, AppIdentityDbContext identityContext, ILogger<DatabaseService> logger)
        {
            _context = context;
            _identityContext = identityContext;
            _logger = logger;
        }
        public Models.Monitor? GetMonitorById(int? id)
        {
            var monitor = _context.Monitors.Where(x => x.Id == id)?.FirstOrDefault();
            if (monitor == null)
            {
                return null;
            }
            return monitor;
        }
        public List<Models.Monitor> GetMonitors()
        {
            var monitors = _context.Monitors.ToList();
            return monitors;
        }
        public Models.Product? GetProductMonitorByIdAndCategory(int? id)
        {
            var product = _context.Products.Where(x => x.ProductNativeId == id && x.ProductCategoryId == 1).FirstOrDefault();
            return product;
        }
        public Models.Product? GetProduct(int id)
        {
            return _context.Products.FirstOrDefault(x => x.Id == id);
        }
        public Models.CartProduct? GetCartProdut(int id)
        {
            return _context.CartProducts.FirstOrDefault(x => x.Id == id);
        }
        public Models.AppUser GetUser(string id)
        {
            var user = _identityContext.Users.FirstOrDefault(x => x.Id == id);
            return user;
        }
        public Models.CartProduct? GetCartProductByProductIdAndCartToken(int? id, string? token)
        {
            return _context.CartProducts.FirstOrDefault(x => x.ProductId == id && x.CartToken == token);
        }
        public List<Models.CartProduct> GetUserCartProducts(string token)
        {
            return _context.CartProducts.Where(x => x.CartToken == token).ToList();
        }
        public string? UploadFile(IFormFile? file, string folderPath, string? fileName)
        {
            if(fileName != null)
            {
                fileName = Guid.NewGuid().ToString() + fileName;
                folderPath = Path.Combine(folderPath, fileName);
                using (var fileStream = new FileStream(folderPath, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                    _logger.LogInformation("File with name {fileName} has been created in {path}", fileName, folderPath);
                }
            }
            return fileName;
        }
        
    }
}
