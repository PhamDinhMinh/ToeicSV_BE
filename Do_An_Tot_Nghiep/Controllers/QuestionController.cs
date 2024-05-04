using Do_An_Tot_Nghiep.Dto.Post;
using Do_An_Tot_Nghiep.Dto.Question;
using Do_An_Tot_Nghiep.Services.Question;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Do_An_Tot_Nghiep.Controllers;

[ApiController]
[Route("api/services/app/[controller]")]

public class QuestionController : Controller
{
    private readonly IConfiguration _config;
    private readonly IQuestionService _questionService;
    public QuestionController(IConfiguration config, IQuestionService questionService)
    {
        _config = config;
        _questionService = questionService;
    }
    
    [Authorize("Admin")]
    [HttpPost("Create")]
    public async Task<IActionResult> CreatePost([FromBody] CreateQuestionDto input)
    {
        var result =  await _questionService.CreateQuestion(input);

        return Ok(result);
    }
}