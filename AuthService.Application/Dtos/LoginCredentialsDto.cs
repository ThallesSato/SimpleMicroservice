using System.ComponentModel.DataAnnotations;

namespace AuthService.Application.Dtos;

public class LoginCredentialsDto
{
    [Required]
    [MinLength(4)]
    public string Username { get; set; }
    [Required]
    [MinLength(4)]
    public string Password { get; set; }
}