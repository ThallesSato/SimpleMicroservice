using AuthService.Api.Rabbit;
using AuthService.Application.Dtos;
using AuthService.Application.Interfaces;
using AuthService.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _service;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _save;

    public AuthController(IAuthService service, ITokenService tokenService, IUnitOfWork save)
    {
        _service = service;
        _tokenService = tokenService;
        _save = save;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(LoginCredentialsDto dto)
    {
        try
        {
            // Check if the username is already in use
            if (await _service.IsUsernameInDb(dto.Username)) 
                return BadRequest("Username already in use");

            // Register the user with the provided credentials
            await _service.Register(dto.Username, dto.Password);
        
            // Generate a token for the registered user
            var token = _tokenService.GenerateToken(dto.Username);
        
            // Save changes to the database
            await _save.SaveChangesAsync(); 
        
            // Send username to rabbitmq (Create user in UserService)
            SendUsername.Send(dto.Username);

            return Ok(token);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginCredentialsDto dto)
    {
        try
        {
            // Verify if username and password are correct
            if (!await _service.Login(dto.Username, dto.Password)) 
                return BadRequest("Wrong username or password");
            
            // Generate JWT token
            var token = _tokenService.GenerateToken(dto.Username); 
            return Ok(token);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}