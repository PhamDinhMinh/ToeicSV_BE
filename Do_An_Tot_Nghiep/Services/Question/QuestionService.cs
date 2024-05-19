using AutoMapper;
using Do_An_Tot_Nghiep.Dto.Question;
using Do_An_Tot_Nghiep.Enums.Question;
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
        question.CreationTime = DateTime.Now;

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
        
        foreach (var questionDto in parameters.Questions)
        {
            var question = new QuestionToeic
            {
                AudioUrl = parameters.AudioUrl,
                ImageUrl = parameters.ImageUrl,
                NumberSTT = questionDto.NumberSTT,
                Index = questionDto.Index,
                Transcription = questionDto.Transcription,
                Type = questionDto.Type,
                PartId = parameters.PartId,
                IdGroupQuestion = groupQuestion.Id,
                CreationTime = DateTime.Now
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

    public async Task<object> GetListQuestionSingle(GetListQuestionSingleDto parameters)
    {
        try
        {
            var query = from question in context.QuestionToeics
                join answers in context.AnswerToeics on question.Id equals answers.IdQuestion into answersGroup
                select new
                {
                    Id = question.Id,
                    Content = question.Content,
                    PartId = question.PartId,
                    ImageUrl = question.ImageUrl,
                    AudioUrl = question.AudioUrl,
                    Transcription = question.Transcription,
                    NumberSTT = question.NumberSTT,
                    Type = question.Type,
                    IdGroupQuestion = question.IdGroupQuestion,
                    CreationTime = question.CreationTime,
                    Answers = answersGroup.Select(a => new
                    {
                        Id = a.Id,
                        Content = a.Content,
                        IsBoolean = a.IsBoolean,
                        Transcription = a.Transcription
                    }).ToList()
                };
                query = query.Where(x => x.IdGroupQuestion == null);
                query = query.OrderByDescending(x => x.CreationTime);
            if (parameters.Type.HasValue)
            {
                query = query.Where(x => x.Type.Contains(parameters.Type.Value));
            }
            if (parameters.PartId.HasValue)
            {
                query = query.Where(x => x.PartId == parameters.PartId);
            }
            if (!string.IsNullOrEmpty(parameters.Keyword))
            {
                query = query.Where(x => x.Content.Contains(parameters.Keyword));
            }
            
            var result = query.Skip(parameters.SkipCount).Take(parameters.MaxResultCount).ToList();
            
            return DataResult.ResultSuccess(result, "", query.Count());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<object> GetListQuestionGroup(GetListQuestionGroupDto parameters)
    {
        try
        {
            var query = from groupQuestion in context.GroupQuestions
                join question in context.QuestionToeics on groupQuestion.Id equals question.IdGroupQuestion into manyQuestion
                select new
                {
                    Id = groupQuestion.Id,
                    Content = groupQuestion.Content,
                    PartId = groupQuestion.PartId,
                    ImageUrl = groupQuestion.ImageUrl,
                    AudioUrl = groupQuestion.AudioUrl,
                    IdExam = groupQuestion.IdExam,
                    Questions = manyQuestion.Select(q => new
                    {
                        Id = q.Id,
                        NumberSTT = q.NumberSTT,
                        Content = q.Content,
                        Type = q.Type,
                        Transcription = q.Transcription,
                        Answers = context.AnswerToeics.Where(a => a.IdQuestion == q.Id).ToList()
                    }).ToList()
                };
            if (parameters.PartId.HasValue)
            {
                query = query.Where(x => x.PartId == parameters.PartId);
            }
            if (!string.IsNullOrEmpty(parameters.Keyword))
            {
                query = query.Where(x => x.Content.Contains(parameters.Keyword));
            }
            
            var result = query.Skip(parameters.SkipCount).Take(parameters.MaxResultCount).ToList();
            
            return DataResult.ResultSuccess(result, "", query.Count());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task<object> GetListQuestion(GetListQuestionDto parameters)
    {
        throw new NotImplementedException();
    }

    public async Task<object> GetQuestionUser(GetQuestionUserDto parameters)
    {
        try
        {
            if (parameters.PartId == PART_TOEIC.Part1 || parameters.PartId == PART_TOEIC.Part2 ||
                parameters.PartId == PART_TOEIC.Part5)
            {
                var query = from question in context.QuestionToeics
                    join answers in context.AnswerToeics on question.Id equals answers.IdQuestion into answersGroup
                    select new
                    {
                        Id = question.Id,
                        Content = question.Content,
                        PartId = question.PartId,
                        ImageUrl = question.ImageUrl,
                        AudioUrl = question.AudioUrl,
                        NumberSTT = question.NumberSTT,
                        Type = question.Type,
                        IdGroupQuestion = question.IdGroupQuestion,
                        Transcription = question.Transcription,
                        Answers = answersGroup.Select(a => new
                        {
                            Id = a.Id,
                            Content = a.Content,
                            IsBoolean = a.IsBoolean,
                            Transcription = a.Transcription
                        }).ToList(),
                    };

                if (parameters.Type.HasValue)
                {
                    query = query.Where(x => x.Type.Contains(parameters.Type.Value));
                }

                if (parameters.PartId.HasValue)
                {
                    query = query.Where(x => x.PartId == parameters.PartId);
                }

                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    query = query.Where(x => x.Content.Contains(parameters.Keyword));
                }

                query = query.OrderBy(x => x.IdGroupQuestion).ThenBy((x) => Guid.NewGuid());
                var result = query.Take(parameters.MaxResultCount).ToList();
                return DataResult.ResultSuccess(result, "", result.Count);
            }
            
            return DataResult.ResultSuccess( "hehe", "Thành công"); 
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}