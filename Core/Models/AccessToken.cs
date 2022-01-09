using System;
using System.ComponentModel.DataAnnotations;

namespace KindergartenManagementSystem.Core.Models
{
    public class AccessToken
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime Expiration { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public bool IsValid { get; set; }

        public AccessToken()
        {
            IsValid = true;
        }
    }
}