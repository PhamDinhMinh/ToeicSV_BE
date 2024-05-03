using System.Net;
using Do_An_Tot_Nghiep.Dto.User;
using Do_An_Tot_Nghiep.Services.User;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpGet("GetById")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _userService.GetUserById(id);
        return Ok(result);
    }
    
    [Authorize]
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] UserUpdateDto input)
    {
        var result =  await _userService.Update(input);

        return Ok(result);
    }
    
    [Authorize]
    [HttpPut("UpdateAvatar")]
    public async Task<IActionResult> UpdateAvatar([FromBody] AvatarUpdateDto input)
    {
        var result =  await _userService.UpdateAvatar(input);

        return Ok(result);
    }
    
    [Authorize]
    [HttpPut("UpdateCoverAvatar")]
    public async Task<IActionResult> UpdateCoverAvatar([FromBody] CoverAvatarUpdateDto input)
    {
        var result =  await _userService.UpdateCoverAvatar(input);

        return Ok(result);
    }
    
    [Authorize]
    [HttpPost("ChangePassword")]
    public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordDto input)
    {
        var result =  await _userService.ChangePassword(input);
        if (result == null)
        {
            return BadRequest("Mật khẩu hiện tại không đúng!");
        }

        return Ok(result);
    }
    
    [Authorize]
    [HttpDelete("Delete")]
    public async Task<Object> Delete(int id)
    {
        var result =  await _userService.Delete(id);

        return Ok(result);
    }
}