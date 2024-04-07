using Do_An_Tot_Nghiep.Dto.Post;
using Do_An_Tot_Nghiep.Services.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Do_An_Tot_Nghiep.Controllers;

[ApiController]
[Route("api/services/app/[controller]")]
public class PostController: Controller
{
    private readonly IConfiguration _config;
    private readonly IPostService _postService;
    public PostController(IConfiguration config,IPostService  postService)
    {
        _config = config;
        _postService = postService;
    }
    
    [Authorize]
    [HttpPost("Create")]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostDto input)
    {
        var result =  await _postService.Create(input);

        return Ok(result);
    }
    
    [Authorize]
    [HttpDelete("Delete")]
    public async Task<Object> Delete(int id)
    {
        var result =  await _postService.Delete(id);

        return Ok(result);
    }
}