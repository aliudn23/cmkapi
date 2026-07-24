using cmkapi.Services.Interfaces;
using cmkapi.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

namespace cmkapi.Services;

public class JwtService(IConfiguration configuration) : IJwtService
{
    private readonly IConfiguration _configuration = configuration;

    public string GenerateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub,user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email,user.Email),
            new Claim(ClaimTypes.Name,user.Name)
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(
                _configuration["Jwt:Key"]!));

        var credential =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(

            issuer:_configuration["Jwt:Issuer"],

            audience:_configuration["Jwt:Audience"],

            claims:claims,

            expires:DateTime.UtcNow.AddMinutes(15),

            signingCredentials:credential
        );

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}