using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels
{
    public class MonitorViewModel
    {        
        public int Id { get; set; }        
        public string Name { get; set; }        
        public float Price { get; set; }         
        public string Resolution { get; set; }        
        [Display(Name = "Refresh rate")]        
        public string Refreshening { get; set; }
        public int? CategoryId { get; set; }
        public bool DefaultPhoto { get; set; } = true;
        public IFormFile? MonitorPhoto { get; set; }
    }
}
