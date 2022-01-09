using KindergartenManagementSystem.Core.Helpers;
using KindergartenManagementSystem.Core.Models;
using KindergartenManagementSystem.Core.Services;
using KindergartenManagementSystem.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KindergartenManagementSystem.Infrastructure.Data
{
    public class SignInManager : ISignInManager
    {
        private readonly IUserManager _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<InfrastructureUser> _signInManager;
        private InfrastructureUser _infrastructureUser;
        private readonly InfrastructureDbContext _dbContext;

        public SignInManager(IUserManager userManager, IConfiguration configuration, SignInManager<InfrastructureUser> signInManager, InfrastructureDbContext dbContext)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _infrastructureUser = new InfrastructureUser();
        }

        public async Task<Jwt> GenerateAuthenticationTokenAsync(User user)
        {
            var userRoles = await _userManager.GetRolesAsync(user);

            var tokenId = Guid.NewGuid();

            var authClaims = new List<Claim>
            {
                new Claim("UserName", user.UserName),
                new Claim("UserId", user.Id.ToString()),
                new Claim("Id", tokenId.ToString()),
            };

            foreach (var userRole in userRoles)
                authClaims.Add(new Claim("Role", userRole));

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(10),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            var accessToken = new AccessToken
            {
                Id = tokenId,
                Expiration = token.ValidTo,
                UserId = user.Id
            };

            _dbContext.AccessTokens.Add(accessToken);

            return new Jwt
            {
                Token = jwt,
                Expiration = accessToken.Expiration
            };

        }

        public bool IsSignInRequireConfirmedEmail()
        {
            return _signInManager.Options.SignIn.RequireConfirmedEmail;
        }

        public async Task<bool> CheckPasswordAsync(User user, string password)
        {
            user.ToInfrastructureUser(_infrastructureUser);
            return await _signInManager.UserManager.CheckPasswordAsync(_infrastructureUser, password);
        }

        public async Task InvalidateToken(string token, User user, bool invalidateAll)
        {
            var validTokensIQueryable = _dbContext.AccessTokens.Where(t => t.IsValid == true);
            if (invalidateAll)
            {
                var tokensToInvalidate = await validTokensIQueryable.Where(v => v.UserId == user.Id).ToListAsync();
                tokensToInvalidate.ForEach(t => t.IsValid = false);
                return;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(token);
            var tokenId = Guid.Parse(securityToken.Claims.First(c => c.Type == "Id").Value);
            var tokenToInvalidate = await validTokensIQueryable.FirstAsync(t => t.Id == tokenId);
            tokenToInvalidate.IsValid = false;
        }

        public async Task RemoveExpiredTokensAsync()
        {
            var expiredTokens = await _dbContext.AccessTokens
                .Where(at => at.Expiration <= DateTime.Now).ToListAsync();
            _dbContext.AccessTokens.RemoveRange(expiredTokens);
        }
    }
}