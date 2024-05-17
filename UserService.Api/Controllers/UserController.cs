using Microsoft.AspNetCore.Authorization;
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
    public async Task<IActionResult> Put(UserDto userDto)
    {
        try
        {
            var username = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value ?? "falhou";
            var user = await _userService.GetByUsernameAsync(username);
            
            if (user == null)
                return BadRequest("User not found");
            
            user.Nome = userDto.Nome;
            _userService.Update(user);
            return Ok();
        } 
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpDelete]
    public async Task<IActionResult> Delete()
    {
        try
        {
            var username = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value ?? "falhou";
            var user = await _userService.GetByUsernameAsync(username);
            
            if (user == null)
                return BadRequest("User not found");
            
            _userService.Delete(user);
            return Ok();
        } 
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}