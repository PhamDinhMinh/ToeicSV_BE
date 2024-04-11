using Do_An_Tot_Nghiep.Dto.PostReact;
using Do_An_Tot_Nghiep.Services.PostReact;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Do_An_Tot_Nghiep.Controllers;


[ApiController]
[Route("api/services/app/[controller]")]
public class PostReactController : Controller
{
    private readonly IConfiguration _config;
    private readonly IPostReactService _postReactService;
    public PostReactController(IConfiguration config, IPostReactService postReactService)
    {
        _config = config;
        _postReactService = postReactService;
    }
    
    [Authorize]
    [HttpPost("CreateOrUpdate")]
    public async Task<IActionResult> Update([FromBody] CreatePostReactDto input)
    {
        var result =  await _postReactService.CreateOrUpdate(input);

        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("GetAllReact")]
    public async Task<IActionResult> GetUserWallPost([FromQuery]GetListPostReactDto parameters)
    {
        var result =  await _postReactService.GetListReact(parameters);

        return Ok(result);
    }
}