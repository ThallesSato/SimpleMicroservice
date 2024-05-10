namespace AuthService.Application.Interfaces;

public interface IAuthService
{
    Task Register(string username, string password);
    Task<bool> Login(string username, string password);
    Task<bool> IsUsernameInDb(string username);
}