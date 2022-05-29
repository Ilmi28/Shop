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
        [MaxLength(9)]
        public float Price { get; set; }
        [Required]
        public string Resolution { get; set; }
        [Required]
        public string Refreshening { get; set; }
        [Required]
        public bool DefaultPhoto { get; set; }
        [RegularExpression(@"/.*\.(gif|jpe?g|bmp|png)$/igm", ErrorMessage = "Only Image files allowed.")]
        public string? MonitorPhoto { get; set; }
        [Required]
        public string Creator { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? Created { get; set; }
        [Required]
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
