using Do_An_Tot_Nghiep.Dto.Grammar;
using Do_An_Tot_Nghiep.Services.Grammar;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Do_An_Tot_Nghiep.Controllers;

[ApiController]
[Route("api/services/app/[controller]")]
public class GrammarController: Controller
{
    private readonly IConfiguration _config;
    private readonly IGrammarService _grammarService;
    public GrammarController(IConfiguration config, IGrammarService grammarService)
    {
        _config = config;
        _grammarService = grammarService;
    }
    
    [Authorize("Admin")]
    [HttpPost("Create")]
    public async Task<IActionResult> CreateGrammar([FromBody] GrammarCreateDto input)
    {
        var result =  await _grammarService.Create(input);

        return Ok(result);
    }
    
    [Authorize("User")]
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllGrammar([FromQuery] GetGrammarDto parameters)
    {
        var result =  await _grammarService.GetAll(parameters);

        return Ok(result);
    }
}