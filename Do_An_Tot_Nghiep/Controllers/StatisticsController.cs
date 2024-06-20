using Do_An_Tot_Nghiep.Dto.Statistics;
using Do_An_Tot_Nghiep.Services.Upload;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Do_An_Tot_Nghiep.Controllers;

[ApiController]
[Route("api/services/app/[controller]")]

public class StatisticsController : Controller
{
    private readonly IConfiguration _config;
    private readonly IStatisticsService _statisticsService;

    public StatisticsController(IConfiguration config, IStatisticsService statisticsService)
    {
        _config = config;
        _statisticsService = statisticsService;
    }

    [Authorize("Admin")]
    [HttpGet("StatisticsUser")]
    public async Task<IActionResult> StatisticsUser([FromQuery] int NumberRange)
    {
        var result = await _statisticsService.StatisticsUser(NumberRange);

        return Ok(result);
    }
    
    [Authorize("Admin")]
    [HttpGet("StatisticsQuestion")]
    public async Task<IActionResult> StatisticsQuestion([FromQuery] int NumberRange)
    {
        var result = await _statisticsService.StatisticsQuestion(NumberRange);

        return Ok(result);
    }
    
    [Authorize("Admin")]
    [HttpGet("StatisticsPost")]
    public async Task<IActionResult> StatisticsPost([FromQuery] int NumberRange)
    {
        var result = await _statisticsService.StatisticsPost(NumberRange);

        return Ok(result);
    }
    
    [Authorize("Admin")]
    [HttpGet("StatisticsCorrectQuestion")]
    public async Task<IActionResult> StatisticsCorrectQuestion()
    {
        var result = await _statisticsService.StatisticsCorrectQuestion();

        return Ok(result);
    }
    
    [Authorize()]
    [HttpGet("StatisticCorrectQuestionUser")]
    public async Task<IActionResult> StatisticCorrectQuestionUser()
    {
        var result = await _statisticsService.StatisticCorrectQuestionUser();

        return Ok(result);
    }
    
    [Authorize()]
    [HttpGet("StatisticsOrdinal")]
    public async Task<IActionResult> StatisticsOrdinal([FromQuery]GetOrdinalDto input)
    {
        var result = await _statisticsService.StatisticsOrdinal(input);

        return Ok(result);
    }
}