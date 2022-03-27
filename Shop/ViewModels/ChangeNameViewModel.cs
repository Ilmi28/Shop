using System.ComponentModel.DataAnnotations;

namespace Shop.ViewModels
{
    public class ChangeNameViewModel
    {
        [Required]
        [MinLength(3)]
        public string Name { get; set; }
    }
}
