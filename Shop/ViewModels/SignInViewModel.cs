using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels
{
    public class SignInViewModel
    {
        [Required]        
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
