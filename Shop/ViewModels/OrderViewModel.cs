using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels
{
    public class OrderViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        [Display(Name = "Post Code")]
        public string PostCode { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Phone Number(optional)")]
        public int? PhoneNumber { get; set; }
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }
        [Display(Name = "Delivery Method")]
        public string DeliveryMethod { get; set; }
    }
}
