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
        // Create Login
        var loginCredential = new LoginCredential 
        {
            Username = username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password) // Encrypt password
        };
    
        // Register the new user in the database
        await _context.Register(loginCredential);
    }
    
    public async Task<bool> Login(string username, string password)
    {
        // Get Login credentials from the context
        var credential = await _context.GetCredentialByUsername(username);
    
        // Verify if Login credentials exist
        if (credential == null)
            return false;
    
        // Verify the password using BCrypt hashing
        return BCrypt.Net.BCrypt.Verify(password, credential.PasswordHash);
    }

    public async Task<bool> IsUsernameInDb(string username)
    {
        return await _context.IsUsernameInDb(username); // Verify if username is already in use
    }
}