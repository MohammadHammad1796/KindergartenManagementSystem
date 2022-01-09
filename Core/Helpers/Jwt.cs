using System;

namespace KindergartenManagementSystem.Core.Helpers
{
    public class Jwt
    {
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}