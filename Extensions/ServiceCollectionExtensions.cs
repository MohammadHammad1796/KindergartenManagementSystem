using KindergartenManagementSystem.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KindergartenManagementSystem.Extensions
{
    public static class ServiceCollectionExtensions
    {
        private static IConfiguration _configuration;

        public static void ApplyConfigurationToExtensions(this IServiceCollection services)
        {
            _configuration = services.BuildServiceProvider().GetService<IConfiguration>();
        }

        public static void AddJwtAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = _configuration["JWT:ValidAudience"],
                        ValidIssuer = _configuration["JWT:ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                        RoleClaimType = "Role"
                    };
                    options.Events = new JwtBearerEvents
                    {
                        OnTokenValidated = context =>
                        {
                            var dbContext = context.HttpContext.RequestServices.GetRequiredService<InfrastructureDbContext>();
                            var userId = int.Parse(context.Principal.Claims.Single(c => c.Type == "UserId").Value);
                            var invalidTokens = dbContext.AccessTokens.Where(at => at.UserId == userId && !at.IsValid).Select(at => at.Id).ToList();
                            var accessToken = context.HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
                            var tokenHandler = new JwtSecurityTokenHandler();
                            var securityToken = (JwtSecurityToken)tokenHandler.ReadToken(accessToken);
                            var tokenId = Guid.Parse(securityToken.Claims.First(c => c.Type == "Id").Value);

                            if (invalidTokens.Contains(tokenId))
                                context.Fail("Invalid-Token");

                            return Task.CompletedTask;

                        }
                    };
                });
        }
    }
}
