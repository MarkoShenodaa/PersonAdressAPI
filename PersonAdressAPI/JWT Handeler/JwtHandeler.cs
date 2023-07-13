using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace PersonAdressAPI.JWT_Handeler;

public class JwtHandeler
{
    private readonly RequestDelegate _next;
    private readonly Tokens _tokenKey;

    public JwtHandeler(RequestDelegate next, IOptions<Tokens> tokenKey)
    {
        _next = next;
        _tokenKey = tokenKey.Value;
    }

    public async Task Invoke(HttpContext context, IPerson userService)
    {
        var token = (Person)context.Items["GetPersonByLoginData"];

        if (token != null)
            attachUserToContext(context, userService, token.GenerateJwtToken());

        await _next(context);
    }

    private async void attachUserToContext(HttpContext context, IPerson userService, string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_tokenKey.TokenKey);
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;
            var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);
            context.Items["User"] = await userService.GetModelByID(userId);
        }
        catch
        {
            // do nothing if jwt validation fails
            // user is not attached to context so request won't have access to secure routes
        }
    }
}