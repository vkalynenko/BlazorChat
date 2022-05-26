using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlazorChatApp.BLL.Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BlazorChatApp.BLL.Infrastructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding
                .UTF8.GetBytes(_configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddDays(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey,
                    SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
