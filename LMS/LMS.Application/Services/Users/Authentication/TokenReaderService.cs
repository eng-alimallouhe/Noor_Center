using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LMS.Application.Services.Users.Authentication
{
    public class TokenReaderService
    {
        private readonly IConfiguration _configuration;

        public TokenReaderService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private ClaimsPrincipal? GetPrincipalFromToken(string token)
        {
            string secretKey = _configuration.GetSection("Jwt")["SecretKey"]!;
            
            var tokenHandler = new JwtSecurityTokenHandler();
            
            var key = Encoding.UTF8.GetBytes(secretKey);

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = false
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, parameters, out SecurityToken validatedToken);

                Console.WriteLine("All Claims:");
                foreach (var claim in principal.Claims)
                {
                    Console.WriteLine($"Type: {claim.Type}, Value: {claim.Value}");
                }


                return principal;
            }
            catch
            {
                return null;
            }
        }

        public string? GetEmail(string accessToken)
        {
            var principal = GetPrincipalFromToken(accessToken);
            return principal?.FindFirst(ClaimTypes.Email)?.Value;
        }

        public string? GetUserId(string accessToken)
        {
            var principal = GetPrincipalFromToken(accessToken);
            return principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

    }
}
