using AuthService.Domain.Models;

namespace AuthService.Domain.Interfaces;

public interface ICredentialsRepository
{
    Task Register(LoginCredential loginCredential);
    Task<LoginCredential?> GetCredentialByUsername(string username);
    Task<bool> IsUsernameInDb(string username);
}