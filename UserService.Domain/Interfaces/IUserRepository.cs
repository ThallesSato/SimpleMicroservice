using UserService.Domain.Models;

namespace UserService.Domain.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User> AddAsync(User user);
    void UpdateAsync(User user);
    void DeleteAsync(User user);
}