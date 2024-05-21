using Do_An_Tot_Nghiep.Dto.ExamToeic;
using Do_An_Tot_Nghiep.Dto.Grammar;
using Do_An_Tot_Nghiep.Services.ExamToeic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Do_An_Tot_Nghiep.Controllers;

[ApiController]
[Route("api/services/app/[controller]")]

public class ExamToeicController : Controller
{
    private readonly IConfiguration _config;
    private readonly IExamToeicService _examToeicService;
    public ExamToeicController(IConfiguration config, IExamToeicService examToeicService)
    {
        _config = config;
        _examToeicService = examToeicService;
    }
    
    [Authorize("Admin")]
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] ExamCreateDto input)
    {
        var result =  await _examToeicService.Create(input);

        return Ok(result);
    }
}