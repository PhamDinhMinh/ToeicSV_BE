using Do_An_Tot_Nghiep.Dto.Question;

namespace Do_An_Tot_Nghiep.Services.Question;

public class QuestionService : IQuestionService
{
    private readonly IDbServices _dbService;
    private PublicContext context = new PublicContext();
    private readonly IHttpContextAccessor _httpContextAccessor;

    public QuestionService(IDbServices dbService, IHttpContextAccessor httpContextAccessor)
    {
        _dbService = dbService;
        _httpContextAccessor = httpContextAccessor;
    }


    public Task<object> CreateQuestion(CreateQuestionDto parameters)
    {
        throw new NotImplementedException();
    }
}