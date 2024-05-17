using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.Dtos;

public class LoginCredentialsDto
{
    [Required]
    [MinLength(4)]
    public required string Username { get; set; }
    [Required]
    [MinLength(4)]
    public required string Password { get; set; }
}