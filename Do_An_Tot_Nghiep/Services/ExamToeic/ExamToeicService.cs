using System.Security.Claims;
using AutoMapper;
using Do_An_Tot_Nghiep.Dto.ExamToeic;
using NewProject.Services.Common;

namespace Do_An_Tot_Nghiep.Services.ExamToeic;

public class ExamToeicService : IExamToeicService
{
    private readonly IDbServices _dbService;
    private PublicContext context = new PublicContext();
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExamToeicService(IDbServices dbService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _dbService = dbService;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<object> Create(ExamCreateDto input)
    {
        try
        {
            var newExamToeic = _mapper.Map<Models.ExamToeic>(input);

            newExamToeic.CreationTime = DateTime.Now;
            newExamToeic.CreatorId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id"));
            await context.ExamToeics.AddAsync(newExamToeic);
            await context.SaveChangesAsync();
            return DataResult.ResultSuccess("Tạo đề thi thành công");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}