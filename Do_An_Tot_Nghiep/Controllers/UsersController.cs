using Do_An_Tot_Nghiep.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace Do_An_Tot_Nghiep.Controllers;

[ApiController]
[Route("api/services/app/[controller]")]
public class UsersController : Controller
{
    private readonly IConfiguration _config;
    private readonly IUserService _userService;
    public UsersController(IConfiguration config, IUserService userService)
    {
        _config = config;
        _userService = userService;
    }
    
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var result =  await _userService.Delete(id);

        return Ok(result);
    }
}