using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels
{
    public class MonitorViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string Resolution { get; set; }
        [Required]
        [Display(Name = "Refresh rate(Hz)")]
        public string Refreshening { get; set; }
        public int? CategoryId { get; set; }
        public bool DefaultPhoto { get; set; } = true;
        public IFormFile? MonitorPhoto { get; set; }
    }
}
