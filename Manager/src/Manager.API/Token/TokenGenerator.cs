using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace Manager.API.Token
{
    public class TokenGenerator : ITokenGenerator {

        private readonly IConfiguration _configuration;

        public TokenGenerator(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken() {
            
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            // token dsecription
            var tokenDescriptor = new SecurityTokenDescriptor{

                // user info
                Subject = new ClaimsIdentity(new Claim[]{
                    new Claim(ClaimTypes.Name, _configuration["Jwt:Login"]),
                    new Claim(ClaimTypes.Role, "User")
                }),

                Expires = DateTime.UtcNow.AddHours(int.Parse(_configuration["Jwt:HoursToExpire"])),

                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            // token signature (validation)
            return tokenHandler.WriteToken(token);
        }
    }
}