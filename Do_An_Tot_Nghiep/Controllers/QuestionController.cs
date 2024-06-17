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
    
    [Authorize("Admin")]
    [HttpGet("GetListQuestionSingle")]
    public async Task<IActionResult> GetListQuestionSingle([FromQuery] GetListQuestionSingleDto parameters)
    {
        var result =  await _questionService.GetListQuestionSingle(parameters);

        return Ok(result);
    }   
    
    [Authorize("Admin")]
    [HttpGet("GetListQuestionGroup")]
    public async Task<IActionResult> GetListQuestionGroup([FromQuery] GetListQuestionGroupDto parameters)
    {
        var result =  await _questionService.GetListQuestionGroup(parameters);

        return Ok(result);
    }
    
    [Authorize("Admin")]
    [HttpPut("UpdateQuestionSingle")]
    public async Task<IActionResult> UpdateQuestionSingle([FromBody] UpdateQuestionSingleDto parameters)
    {
        var result =  await _questionService.UpdateQuestionSingle(parameters);

        return Ok(result);
    }
    
    [Authorize("Admin")]
    [HttpDelete("DeleteQuestionSingle")]
    public async Task<IActionResult> DeleteQuestionSingle([FromQuery] int id)
    {
        var result =  await _questionService.DeleteQuestionSingle(id);

        return Ok(result);
    }
    
    [Authorize("Admin")]
    [HttpPut("UpdateQuestionGroup")]
    public async Task<IActionResult> UpdateQuestionGroup([FromBody] UpdateQuestionGroupDto parameters)
    {
        var result =  await _questionService.UpdateQuestionGroup(parameters);

        return Ok(result);
    }
    
    [Authorize("Admin")]
    [HttpDelete("DeleteQuestionGroup")]
    public async Task<IActionResult> DeleteQuestionGroup([FromQuery] int id)
    {
        var result =  await _questionService.DeleteQuestionGroup(id);

        return Ok(result);
    }
    
    [Authorize("Admin")]
    [HttpPost("ImportExcelQuestionSingle")]
    public async Task<IActionResult> ImportExcelQuestionSingle([FromForm] ImportExcelDto  parameters)
    {
        var result =  await _questionService.ImportExcelQuestionSingle(parameters);

        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("GetQuestionUser")]
    public async Task<IActionResult> GetQuestionUser([FromQuery] GetQuestionUserDto parameters)
    {
        var result =  await _questionService.GetQuestionUser(parameters);

        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("GetQuestionById")]
    public async Task<IActionResult> GetQuestionById([FromQuery] int id)
    {
        var result =  await _questionService.GetQuestionById(id);

        return Ok(result);
    }
}