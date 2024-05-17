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
            // Return all users
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
            // Get the username from the claims
            var username = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;

            // Check if the username is null
            if (username == null)
                return Unauthorized();
            
            // Return Ok with the user
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
            // Get the username from the claims
            var username = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;

            // Check if the username is null
            if (username == null)
                return Unauthorized();

            // Get the user details by username asynchronously
            var user = await _userService.GetByUsernameAsync(username);
    
            // Check if the user exists
            if (user == null)
                return BadRequest("User not found");
    
            // Update the user's name with the new value
            user.Nome = userDto.Nome;
        
            // Call the service to update the user
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
            // Get the username from the claims
            var username = User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;

            // Check if the username is null
            if (username == null)
                return Unauthorized();
            
            // Get the user details by username asynchronously
            var user = await _userService.GetByUsernameAsync(username);
        
            // Check if the user is not found
            if (user == null)
                return BadRequest("User not found");
        
            // Delete the user
            _userService.Delete(user);
            return Ok();
        } 
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}