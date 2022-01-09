using System.ComponentModel.DataAnnotations;

namespace KindergartenManagementSystem.Controllers.Apis.Dtos
{
    public class ResetPasswordDto
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; }
    }
}
