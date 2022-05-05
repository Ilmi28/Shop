using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class AppUser : IdentityUser
    {
        [MinLength(3)]
        [MaxLength(25)]
        [Required]
        public string Name { get; set; }
        [Required]
        public string CartToken { get; set; }
        public bool HasProStatus { get; set; } = false;
        public string UserPhoto { get; set; }
    }
}
