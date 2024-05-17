using Microsoft.EntityFrameworkCore;
using UserService.Domain.Interfaces;
using UserService.Domain.Models;
using UserService.Infra.Context;

namespace UserService.Infra.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<List<User>> GetAllAsync()
    {
        // Return all users
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        // Return user by username
        return await _context.Users.FirstOrDefaultAsync(x => x.Username == username);
    }

    public async Task<User> AddAsync(User user)
    {
        // Add user to db
        var result = await _context.Users.AddAsync(user);
        return result.Entity;
    }

    public void Update(User user)
    {
        // Update user in db
        _context.Users.Update(user);
    }

    public void Delete(User user)
    {
        // Delete user in db
        _context.Users.Remove(user);
    }
}