using Do_An_Tot_Nghiep.Dto.PostComment;
using Do_An_Tot_Nghiep.Services.PostComment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Do_An_Tot_Nghiep.Controllers;

[ApiController]
[Route("api/services/app/[controller]")]
public class PostCommentController: Controller
{
    private readonly IConfiguration _config;
    private readonly IPostCommentService _postCommentService;
    public PostCommentController(IConfiguration config,IPostCommentService  postCommentService)
    {
        _config = config;
        _postCommentService = postCommentService;
    }
    
    [Authorize]
    [HttpPost("Create")]
    public async Task<IActionResult> CreatePostComment([FromBody] CreatePostCommentDto input)
    {
        var result =  await _postCommentService.Create(input);

        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllPostComment([FromQuery]GetPostCommentDto parameters)
    {
        var result =  await _postCommentService.GetAll(parameters);

        return Ok(result);
    }
    
    [Authorize()]
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] UpdatePostCommentDto input)
    {
        var result =  await _postCommentService.Update(input);

        return Ok(result);
    }
    
    [Authorize]
    [HttpDelete("Delete")]
    public async Task<Object> Delete(int id)
    {
        var result =  await _postCommentService.Delete(id);

        return Ok(result);
    }
}