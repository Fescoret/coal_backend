using coal_backend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace coal_backend.Utils;

public class TokenFactory
{
    public string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim("firstName", user.FirstName),
            new Claim("lastName", user.LastName),
            new Claim("email", user.Email),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("my security string"));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                issuer: "me",
                audience: "you",
                signingCredentials: creds
            );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }
}
