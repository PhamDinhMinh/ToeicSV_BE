using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Do_An_Tot_Nghiep.Dto.Auth;
using Do_An_Tot_Nghiep.Helpers;
using Do_An_Tot_Nghiep.Models;
using Do_An_Tot_Nghiep.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Do_An_Tot_Nghiep.Controllers;

[ApiController]
[Route("api/services/app/[controller]")]
public class AuthenticationController : Controller
{
    private readonly IConfiguration _config;
    private readonly IAuthService _authService;
    public AuthenticationController(IConfiguration config, IAuthService authService)
    {
        _config = config;
        _authService = authService;
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
            var user = await _authService.Register(input);
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

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto input)
    {
        try
        {
            var user = await _authService.Login(input);
            if (user == null)
            {
                return BadRequest("Tên đăng nhập hoặc mật khẩu không đúng.");
            }
            var tokenService = new Token(_config);
            var token = tokenService.CreateToken(user);
            var refreshToken = tokenService.GenerateRefreshToken();

            return Ok(new { AccessToken = token, RefreshToken = refreshToken, User = user });
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("GetUserInfo")]
    [Authorize("User")]
    public async Task<IActionResult> GetUserInfo()
    {
        try
        {
            var user = await _authService.GetUserInfo();
            return Ok(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}