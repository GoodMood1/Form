using System.ComponentModel.DataAnnotations;

namespace MvcIntro.Models.ViewModel
{
    public class UserRegisterModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [MinLength(8)]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string PasswordRepeat { get; set; }
    }
}