using Microsoft.IdentityModel.Tokens;
using ScrumStandUpTrackerProject.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ScrumStandUpTrackerProject.Services
{

    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config; // access appsettings.json config values like token key, issuer and audience.

        public TokenService(IConfiguration config)
        {
            _config = config; //config has imp JWT settings - Key,Issuer,Audience
        }

        public string CreateToken(Developer developer)
        {
            var claims = new[]
            {

                //new Claim("uid",developer.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, developer.Id.ToString()), // stores user ID
                new Claim(ClaimTypes.Name, developer.UserName) // This stores username
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtSettings:Key"]));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["JwtSettings:Issuer"],
                audience: _config["JwtSettings:Audience"],
                claims: claims, // Data which we want in token
                expires: DateTime.UtcNow.AddDays(1),  
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}