using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels
{
    public class SignInViewModel
    {               
        public string Email { get; set; }        
        public string Password { get; set; }
        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
