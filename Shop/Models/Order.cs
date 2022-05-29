using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class Order
    {
        public int Id { get; set; }
        [Required]      
        public string FirstName { get; set; }
        [Required]        
        public string LastName { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]        
        public string PostCode { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Phone Number")]
        public int? PhoneNumber { get; set; }
        public string PaymentMethod { get; set; }
        public string DeliveryMethod { get;set; }
        [Required]
        public float RawPrice { get; set; }
        public float? DeliveryPrice { get; set; }
        public float? PaymentPrice { get; set; }
        public float? TotalPrice { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? Created { get; set; }
        public bool OrderPaid { get; set; }
        public string OrderToken { get; set; }
    }
}
