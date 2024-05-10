using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Application.Interfaces;
using AuthService.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Application.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string GenerateToken(string username)
    {
        var issuer = _configuration["Jwt:Issuer"] ?? // pega o issuer do jwt
                     throw new InvalidConfigurationException("Invalid jwt issuer configuration");
        
        var audience = _configuration["Jwt:Audience"] ?? // pega o audience do jwt
                       throw new InvalidConfigurationException("Invalid jwt audience configuration");
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"] ?? // pega a chave do jwt
                throw new InvalidConfigurationException("Invalid jwt key configuration")));
        
        // cria o token com as claims, issuer, audience e o tempo de expiração
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Claims = new Dictionary<string, object>
            {
                {"username", username}
            },
            Expires = DateTime.UtcNow.Add(GetTokenLifetime()),
            SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature),
            Issuer = issuer,
            Audience = audience
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor); // cria o token
        return tokenHandler.WriteToken(token);
    }
    
    private TimeSpan GetTokenLifetime()
    {
        string tokenLifetimeString = _configuration["Jwt:TokenLifetime"] ?? "erro"; // pega o tempo de expiração do jwt
        string format = @"hh\:mm\:ss"; // formato do tempo de expiração
        
        IFormatProvider formatProvider = CultureInfo.InvariantCulture; // formatação do tempo de expiração para cultura
        
        // converte o tempo de expiração para TimeSpan
        if (TimeSpan.TryParseExact(tokenLifetimeString,format, formatProvider, out TimeSpan tokenLifetime))
        {
            return tokenLifetime;
        }
        throw new InvalidConfigurationException("Invalid token lifetime configuration");
    }

}