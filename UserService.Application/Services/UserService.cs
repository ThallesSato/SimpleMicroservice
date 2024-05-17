﻿using UserService.Application.Dto;
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
        return await _repository.GetAllAsync();
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _repository.GetByUsernameAsync(username);
    }

    public async Task<User> AddAsync(User user)
    {
        return await _repository.AddAsync(user);
    }

    public bool Update(User user)
    {
        _repository.Update(user);
        return true;
    }

    public bool Delete(User user)
    {
        _repository.Delete(user);
        return true;
    }
}