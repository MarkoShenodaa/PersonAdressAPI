global using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PersonAdressAPI.Models;

public class Person
{
    private readonly Tokens _tokenKey;
    public Person(IOptions<Tokens> tokens)
    {
        _tokenKey = tokens.Value;
    }
    public int Id { get; set; }

    public string Name { get => string.Format("{0} {1}", FirstName, LastName); }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Address { get; set; }

    [JsonIgnore]
    public string UserName { get; set; } = string.Empty;

    [JsonIgnore]
    public string Password { get; set; } = string.Empty;

    public string GenerateJwtToken()
    {
        // generate token that is valid for 7 days
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_tokenKey.TokenKey);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[] { new Claim("id", Id.ToString()) }),

            Expires = DateTime.UtcNow.AddDays(7),

            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}