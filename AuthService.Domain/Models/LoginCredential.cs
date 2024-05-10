namespace AuthService.Domain.Models;

public class LoginCredential
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string PasswordHash { get; set; }
}