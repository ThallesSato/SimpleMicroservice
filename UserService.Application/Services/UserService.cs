using UserService.Application.Dto;
using UserService.Application.Interfaces;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;

namespace UserService.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository userRepository)
    {
        _repository = userRepository;
    }
    public async Task<List<User>> GetAllAsync()
    {
        // Return all users
        return await _repository.GetAllAsync();
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        // Return user by username
        return await _repository.GetByUsernameAsync(username);
    }

    public async Task<User> AddAsync(User user)
    {
        // Add user to db
        return await _repository.AddAsync(user);
    }

    public bool Update(User user)
    {
        // Update user in db
        _repository.Update(user);
        return true;
    }

    public bool Delete(User user)
    {
        // Delete user in db
        _repository.Delete(user);
        return true;
    }
}