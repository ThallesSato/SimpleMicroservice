﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Application.Dto;
using UserService.Application.Interfaces;

namespace UserService.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        try
        {
            return Ok(await _userService.GetAllAsync());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetUser()
    {
        try
        {
            var username = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value ?? "falhou";
            return Ok(await _userService.GetByUsernameAsync(username));
        } 
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> Put(UserDto user)
    {
        try
        {
            var username = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value ?? "falhou";
            await _userService.UpdateAsync(username, user);
            return Ok();
        } 
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}