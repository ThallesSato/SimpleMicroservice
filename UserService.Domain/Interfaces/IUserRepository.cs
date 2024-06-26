﻿using UserService.Domain.Models;

namespace UserService.Domain.Interfaces;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();
    Task<User?> GetByUsernameAsync(string username);
    Task<User> AddAsync(User user);
    void Update(User user);
    void Delete(User user);
}