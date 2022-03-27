using System.ComponentModel.DataAnnotations.Schema;

namespace Shop.Models
{
    public class CartProduct
    {
        public int Id { get; set; }
        public string CartToken { get; set; }
        public string Name { get; set; }
        public string Photo { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public Product Product { get; set; }
        public int ProductId { get; set; }
    }
}
