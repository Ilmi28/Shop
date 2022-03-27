using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class AppUser : IdentityUser
    {
        [MinLength(3)]
        [Required]
        public string Name { get; set; }
        [Required]
        public string CartToken { get; set; }
    }
}
