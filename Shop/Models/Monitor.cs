using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    public class Monitor
    {
        [Required]
        public int Id { get; set; }
        [Display(Name = "Monitor Name")]
        [Required]
        public string Name { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string Resolution { get; set; }
        [Required]
        public string Refreshening { get; set; }
        public Category Category { get; set; }
        [Required]
        public int CategoryId { get; set; }
        [Required]
        public bool DefaultPhoto { get; set; }
        [RegularExpression(@"/.*\.(gif|jpe?g|bmp|png)$/igm", ErrorMessage = "Only Image files allowed.")]
        public string? MonitorPhoto { get; set; }
        [Required]
        public string Creator { get; set; }
    }
}
