using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels
{
    public class SmartphoneViewModel
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
        [Display(Name = "Screen Diagonal")]
        public float ScreenDiagonal { get; set; }
        [Required]
        [Display(Name = "Camera Resolution")]
        public int CameraResolution { get; set; }
        [Display(Name = "Default photo")]
        public bool DefaultPhoto { get; set; }
        [Display(Name = "Smartphone photo")]
        public IFormFile? SmartphonePhoto { get; set; }
        [Required]
        public int Stock { get; set; }  
    }
}
