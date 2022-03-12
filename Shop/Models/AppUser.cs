using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }
    }
}
