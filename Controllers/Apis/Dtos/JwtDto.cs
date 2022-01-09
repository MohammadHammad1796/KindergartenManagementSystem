using System;

namespace KindergartenManagementSystem.Controllers.Apis.Dtos
{
    public class JwtDto
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}