using UserService.Application.Dto;
using UserService.Domain.Models;

namespace UserService.Application.Interfaces;

public interface IUserService
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByUsernameAsync(string username);
    Task<User> AddAsync(User user);
    Task<bool> UpdateAsync(string username, UserDto user);
    bool DeleteAsync(string username);
}