namespace Shop.Models
{
    public class OrderedProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        public Order Order { get; set; }
        public int OrderId { get; set; }
    }
}
