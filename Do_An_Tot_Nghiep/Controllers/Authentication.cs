using Do_An_Tot_Nghiep.Dto.User;
using Do_An_Tot_Nghiep.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace Do_An_Tot_Nghiep.Controllers;

[ApiController]
[Route("api/services/app/[controller]")]
public class AuthenticationController : Controller
{
    private readonly IConfiguration _config;
    private readonly IAuthService _userService;
    public AuthenticationController(IConfiguration config, IAuthService userService)
    {
        _config = config;
        _userService = userService;
    }
    
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserRegisterDto input)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userService.Register(input);
            if (user != null)
            {
                return Ok("User registered successfully"); 
            }
            else
            {
                return BadRequest("User registration failed");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

}