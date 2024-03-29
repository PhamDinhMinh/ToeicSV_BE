using System.Net;
using Do_An_Tot_Nghiep.Dto.User;
using Do_An_Tot_Nghiep.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace Do_An_Tot_Nghiep.Controllers;

[ApiController]
[Route("api/services/app/[controller]")]
public class UserController : Controller
{
    private readonly IConfiguration _config;
    private readonly IUserService _userService;
    public UserController(IConfiguration config, IUserService userService)
    {
        _config = config;
        _userService = userService;
    }

    [HttpGet("GetById")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _userService.GetUserById(id);
        return Ok(result);
    }
    
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] UserUpdateDto input)
    {
        var result =  await _userService.Update(input);

        return Ok(result);
    }
    
    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordDto input)
    {
        var result =  await _userService.ChangePassword(input);

        return Ok(result);
    }
    
    [HttpDelete("Delete")]
    public async Task<Object> Delete(int id)
    {
        var result =  await _userService.Delete(id);

        return Ok(result);
    }
}