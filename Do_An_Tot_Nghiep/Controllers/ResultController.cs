using Do_An_Tot_Nghiep.Dto.Question;
using Do_An_Tot_Nghiep.Dto.Result;
using Do_An_Tot_Nghiep.Services.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Do_An_Tot_Nghiep.Controllers;

[ApiController]
[Route("api/services/app/[controller]")]

public class ResultController : Controller
{
    private readonly IConfiguration _config;
    private readonly IResultService _resultService;
    public ResultController(IConfiguration config, IResultService resultService)
    {
        _config = config;
        _resultService = resultService;
    }
    
    [Authorize]
    [HttpPost("SubmitQuestion")]
    public async Task<IActionResult> CreateQuestionSingle([FromBody] SubmitQuestionDto input)
    {
        var result =  await _resultService.Submit(input);

        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("GetById")]
    public async Task<IActionResult> GetById([FromQuery] int id)
    {
        var result =  await _resultService.GetById(id);

        return Ok(result);
    }
}