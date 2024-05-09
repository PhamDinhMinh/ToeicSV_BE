using AutoMapper;
using Do_An_Tot_Nghiep.Dto.Question;
using Do_An_Tot_Nghiep.Models;
using NewProject.Services.Common;

namespace Do_An_Tot_Nghiep.Services.Question;

public class QuestionService : IQuestionService
{
    private readonly IDbServices _dbService;
    private PublicContext context = new PublicContext();
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public QuestionService(IDbServices dbService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _dbService = dbService;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }


    public async Task<object> CreateQuestionSingle(CreateQuestionSingleDto parameters)
    {
        var question = _mapper.Map<QuestionToeic>(parameters);

        await context.QuestionToeics.AddAsync(question);
        await context.SaveChangesAsync();

        foreach (var answerDto in parameters.Answers)
        {
            var answer = new AnswerToeic
            {
                IdQuestion = question.Id,
                IsBoolean = answerDto.IsBoolean,
                Content = answerDto.Content,
                STTAnswer = answerDto.STTAnswer,
                Transcription = answerDto.Transcription
            };

            await context.AnswerToeics.AddAsync(answer);
        }

        await context.SaveChangesAsync();

        return DataResult.ResultSuccess( "Tạo câu hỏi đơn thành công");
    }

    public async Task<object> CreateQuestionGroup(CreateQuestionGroupDto parameters)
    {
        var groupQuestion = _mapper.Map<GroupQuestion>(parameters);
        await context.GroupQuestions.AddAsync(groupQuestion);
        await context.SaveChangesAsync();
        
        foreach (var questionDto in parameters.Question)
        {
            var question = new QuestionToeic
            {
                NumberSTT = questionDto.NumberSTT,
                Index = questionDto.Index,
                Transcription = questionDto.Transcription
            };
            await context.QuestionToeics.AddAsync(question);

            foreach (var answerDto in questionDto.Answers)
            {
                var answer = new AnswerToeic
                {
                    IdQuestion = question.Id,
                    IsBoolean = answerDto.IsBoolean,
                    Content = answerDto.Content,
                    STTAnswer = answerDto.STTAnswer,
                    Transcription = answerDto.Transcription
                };

                await context.AnswerToeics.AddAsync(answer);
            }
        }

        await context.SaveChangesAsync();

        return DataResult.ResultSuccess( "Tạo nhóm câu hỏi đơn thành công");
    }
}