using AuthService.Domain.Models;

namespace AuthService.Application.Interfaces;

public interface ITokenService
{
    string GenerateToken(string username);
}