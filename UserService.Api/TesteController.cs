using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Api;

[ApiController]
[Route("api/[controller]")] // api/teste[]
public class TesteController : ControllerBase
{
    [HttpGet]
    [Authorize]
    public IActionResult Get()
    {
        return Ok(User.Claims.Select(c => new {c.Type, c.Value}));
    }
}