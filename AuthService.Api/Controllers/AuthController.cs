using AuthService.Application.Dtos;
using AuthService.Application.Interfaces;
using AuthService.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
            if (await _service.IsUsernameInDb(dto.Username)) // verifica se o username ja existe
                return BadRequest("Username already in use");

            await _service.Register(dto.Username, dto.Password); // registra o usuario
            
            var token = _tokenService.GenerateToken(dto.Username); // gera o JWToken
            
            await _save.SaveChangesAsync(); // salva alteração no bd
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
            if (!await _service.IsUsernameInDb(dto.Username)) // verifica se o username está no banco
                return BadRequest("Username not found");
            if (!await _service.Login(dto.Username, dto.Password)) // verifica se a senha esta correta
                return BadRequest("Invalid password");
            
            var token = _tokenService.GenerateToken(dto.Username); // gera o JWToken
            
            return Ok(token);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}