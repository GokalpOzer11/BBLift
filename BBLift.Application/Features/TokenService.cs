using BBLift.Application.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BBLift.Application.Features;

public class TokenService(IConfiguration configuration)
    : ITokenService
{
    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        var handler = new JwtSecurityTokenHandler();

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!));

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = configuration["JWT:ValidIssuer"],
            Audience = configuration["JWT:ValidAudience"],
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(12),
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        };

        var token = handler.CreateToken(descriptor);

        return handler.WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];

        using var rng = RandomNumberGenerator.Create();

        rng.GetBytes(randomNumber);

        return Convert.ToBase64String(randomNumber);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string accessToken)
    {
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = configuration["JWT:ValidIssuer"],
            ValidAudience = configuration["JWT:ValidAudience"],
            ValidateLifetime = false,
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!))
        };

        var handler = new JwtSecurityTokenHandler();

        var pricipal = handler.ValidateToken(accessToken, validationParameters, out SecurityToken securityToken);

        var jwtToken = securityToken as JwtSecurityToken;

        if (jwtToken is null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            throw new SecurityTokenException("Invalid Token");

        return pricipal;
    }
}