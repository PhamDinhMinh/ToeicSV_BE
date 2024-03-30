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
    
    [Authorize]
    [HttpPut("UpdateWatch")]
    public async Task<IActionResult> UpdateWatch([FromBody] GrammarUpdateWatchDto input)
    {
        var result =  await _grammarService.UpdateWatch(input);

        return Ok(result);
    }    
    
    [Authorize("Admin")]
    [HttpPut("Update")]
    public async Task<IActionResult> Update([FromBody] GrammarUpdateDto input)
    {
        var result =  await _grammarService.Update(input);

        return Ok(result);
    }
    
    [Authorize]
    [HttpGet("GetAll")]
    public async Task<IActionResult> GetAllGrammar([FromQuery] GetGrammarDto parameters)
    {
        var result =  await _grammarService.GetAll(parameters);

        return Ok(result);
    }

    [Authorize("Admin")]
    [HttpDelete("Delete")]
    public async Task<IActionResult> Delete(int id)
    {
        var result =  await _grammarService.Delete(id);

        return Ok(result); 
    }
}