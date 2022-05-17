using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Smartphone
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public int Memory { get; set; }
        [Required]
        public int RAM { get; set; }
        [Required]
        public float ScreenDiagonal { get; set; }
        [Required]
        public int CameraResolution { get; set; }
        public int CategoryId { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
        public bool DefaultPhoto { get; set; }
        [RegularExpression(@"/.*\.(gif|jpe?g|bmp|png)$/igm", ErrorMessage = "Only Image files allowed.")]
        public string? SmartphonePhoto { get; set; }
        public DateTime Created { get; set; }
        public string Creator { get; set; }
        public int Stock { get; set; }
    }
}
