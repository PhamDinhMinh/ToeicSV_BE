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
    [HttpPost("CreateQuestionSingle")]
    public async Task<IActionResult> CreateQuestionSingle([FromBody] CreateQuestionSingleDto input)
    {
        var result =  await _questionService.CreateQuestionSingle(input);

        return Ok(result);
    }
    
    [Authorize("Admin")]
    [HttpPost("CreateQuestionGroup")]
    public async Task<IActionResult> CreateQuestionGroup([FromBody] CreateQuestionGroupDto input)
    {
        var result =  await _questionService.CreateQuestionGroup(input);

        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("GetListQuestionSingle")]
    public async Task<IActionResult> GetListQuestionSingle([FromQuery] GetListQuestionSingleDto parameters)
    {
        var result =  await _questionService.GetListQuestionSingle(parameters);

        return Ok(result);
    }   
    
    [Authorize]
    [HttpGet("GetListQuestionGroup")]
    public async Task<IActionResult> GetListQuestionGroup([FromQuery] GetListQuestionGroupDto parameters)
    {
        var result =  await _questionService.GetListQuestionGroup(parameters);

        return Ok(result);
    }
}