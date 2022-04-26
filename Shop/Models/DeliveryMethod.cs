namespace Shop.Models
{
    public class DeliveryMethod
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public float Price { get; set; }
        public int DurationInHours { get; set; }
    }
}
