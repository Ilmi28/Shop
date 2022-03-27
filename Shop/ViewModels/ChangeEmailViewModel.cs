using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels
{
    public class ChangeEmailViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
