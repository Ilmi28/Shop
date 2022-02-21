using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Monitor
    {
        public int Id { get; set; }

        [Display(Name = "Monitor Name")]
        [Required]
        public string Name { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string Resolution { get; set; }

        [Display(Name = "Refreshening(Hz)")]
        public string Refreshening { get; set; }
        public Category? Category { get; set; }
        public int? CategoryId { get; set; }
        public bool DefaultPhoto { get; set; }
        [RegularExpression(@"/.*\.(gif|jpe?g|bmp|png)$/igm", ErrorMessage = "Only Image files allowed.")]
        public string? MonitorPhoto { get; set; }
    }
}
