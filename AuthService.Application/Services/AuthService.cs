using AuthService.Application.Interfaces;
using AuthService.Domain.Interfaces;
using AuthService.Domain.Models;

namespace AuthService.Application.Services;

public class AuthService : IAuthService
{
    private readonly ICredentialsRepository _context;
    
    public AuthService(ICredentialsRepository context)
    {
        _context = context;
    }

    public async Task Register(string username, string password)
    {
        var loginCredential = new LoginCredential // cria um novo login
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password) // encripta a senha
        };
        
        await _context.Register(loginCredential); // registra no bd
    }
    
    public async Task<bool> Login(string username, string password)
    {
        var credential = await _context.GetCredentialByUsername(username); // busca o login pelo username
        return BCrypt.Net.BCrypt.Verify(password, credential?.PasswordHash); // verifica se a senha esta correta com a senha encriptada
    }

    public async Task<bool> IsUsernameInDb(string username)
    {
        return await _context.IsUsernameInDb(username); // retorna true se o username estiver no bd
    }
}