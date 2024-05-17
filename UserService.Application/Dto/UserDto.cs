using System.ComponentModel.DataAnnotations;

namespace UserService.Application.Dto;

public class UserDto
{
    [Required]
    public required string Nome { get; set; }
}