using Do_An_Tot_Nghiep.Dto.ExamTips;
using Do_An_Tot_Nghiep.Services.ExamTip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Do_An_Tot_Nghiep.Controllers;

[ApiController]
[Route("api/services/app/[controller]")]

public class ExamTipsController : Controller
{
    private readonly IConfiguration _config;
    private readonly IExamTipsService _examTipsService;
    public ExamTipsController(IConfiguration config, IExamTipsService examTipsService)
    {
        _config = config;
        _examTipsService = examTipsService;;
    }
    
    [Authorize("Admin")]
    [HttpPost("Create")]
    public async Task<IActionResult> CreateExam([FromBody] ExamTipsCreateDto input)
    {
        var result =  await _examTipsService.Create(input);
    
        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllGrammar([FromQuery] ExamTipGetDto parameters)
    {
        var result =  await _examTipsService.GetAll(parameters);

        return Ok(result);
    }
    
    [Authorize("Admin")]
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] ExamTipsUpdateDto input)
    {
        var result =  await _examTipsService.Update(input);

        return Ok(result);
    }
    
    [Authorize("Admin")]
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var result =  await _examTipsService.Delete(id);
    
        return Ok(result); 
    }
}