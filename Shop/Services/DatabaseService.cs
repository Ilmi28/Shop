using Shop.Data;

namespace Shop.Services
{
    public class DatabaseService
    {
        private readonly AppDbContext _context;
        public DatabaseService(AppDbContext context)
        {
            _context = context;
        }
        public Models.Monitor GetMonitorById(int? id)
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
    }
}
