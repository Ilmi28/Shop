using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class PC
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string Processor { get; set; }
        [Required]
        [Display(Name = "Graphic Card")]
        public string GraphicCard { get; set; }
        [Required]
        public int RAM { get; set; }
        public Category? Category { get; set; }
        public int? CategoryId { get; set; }

    }
}
