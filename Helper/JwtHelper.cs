using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using TraineeManagement.api.Models;

namespace TraineeManagement.api.Helper
{
    public class JwtHelper
    {

        public static string GenerateJwtToken(UserModel user, IConfiguration _config)
        {
            var tokenHandler = new JsonWebTokenHandler();

            var jwtSettings = _config.GetSection("JwtSettings");

            if (jwtSettings == null) throw new Exception("Jwt is not configured!");

            var secretKey = jwtSettings["SecretKey"];

            var key = Encoding.UTF8.GetBytes(secretKey!);

            var expiry = jwtSettings["ExpiryMinutes"];

            var issuer = jwtSettings["Issuer"];

            var audience = jwtSettings["Audience"];

            if (issuer == null || audience == null || expiry == null || secretKey == null) throw new Exception("Jwt is not configured");

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(double.Parse(expiry)),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                Issuer = issuer,
                Audience = audience
            };

            return tokenHandler.CreateToken(tokenDescriptor);

        }
    }
}
