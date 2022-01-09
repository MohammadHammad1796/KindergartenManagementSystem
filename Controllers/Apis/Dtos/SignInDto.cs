using System.ComponentModel.DataAnnotations;

namespace KindergartenManagementSystem.Controllers.Apis.Dtos
{
    public class SignInDto
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }
}