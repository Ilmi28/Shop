using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels
{
    public class MonitorViewModel
    {        
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string Resolution { get; set; }        
        [Display(Name = "Refresh rate")]
        [Required]
        public string Refreshening { get; set; }
        public bool DefaultPhoto { get; set; } = true;
        public IFormFile? MonitorPhoto { get; set; }
        [Required]
        public int Stock { get; set; }
    }
}
