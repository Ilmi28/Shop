using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int? ProductNativeId { get; set; }
        [Required]
        public string ProductPhoto { get; set; }
        [Required]
        public string ProductName { get; set; }
        [Required]
        public float ProductPrice { get; set; }
        [ForeignKey("Category")]
        [Required]
        public int ProductCategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<CartProduct> CartProducts { get; set; }
        [Required]
        public string Creator { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? Created { get; set; }
        [Required]
        public int Stock { get; set; }
    }
}
