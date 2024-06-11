using System.Security.Claims;
using AutoMapper;
using Do_An_Tot_Nghiep.Dto.ExamToeic;
using Do_An_Tot_Nghiep.Dto.Question;
using Do_An_Tot_Nghiep.Enums.Question;
using Do_An_Tot_Nghiep.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

    public async Task<object> CreateRandom(ExamCreateDto input)
    {
        try
        {
            var newExamToeic = new Models.ExamToeic();
            newExamToeic.NameExam = input.NameExam;
            newExamToeic.CreationTime = DateTime.Now;
            newExamToeic.CreatorId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id"));
            newExamToeic.ListQuestionPart1 = context.QuestionToeics
                .Where(q => q.PartId == PART_TOEIC.Part1)
                .OrderBy(q => Guid.NewGuid())
                .Take(6)
                .Select(q => q.Id)
                .ToList();
            newExamToeic.ListQuestionPart2 = context.QuestionToeics
                .Where(q => q.PartId == PART_TOEIC.Part2)
                .OrderBy(q => Guid.NewGuid())
                .Take(25)
                .Select(q => q.Id)
                .ToList();
            var randomPart3 = await context.GroupQuestions
                .Where(q => q.PartId == PART_TOEIC.Part3)
                .OrderBy(q => Guid.NewGuid())
                .Take(13)
                .ToListAsync();
            newExamToeic.ListQuestionPart3 = randomPart3
                .OrderBy(q => q.ImageUrl != null && q.ImageUrl.Length > 0)
                .Select(q => q.Id)
                .ToList();
            var randomPart4 = await context.GroupQuestions
                .Where(q => q.PartId == PART_TOEIC.Part4)
                .OrderBy(q => Guid.NewGuid())
                .Take(10)
                .ToListAsync();
            newExamToeic.ListQuestionPart4 = randomPart4
                .OrderBy(q => q.ImageUrl != null && q.ImageUrl.Length > 0)
                .Select(q => q.Id)
                .ToList();
            newExamToeic.ListQuestionPart5 = context.QuestionToeics
                .Where(q => q.PartId == PART_TOEIC.Part5)
                .OrderBy(q => Guid.NewGuid())
                .Take(30)
                .Select(q => q.Id)
                .ToList();
            newExamToeic.ListQuestionPart6 = context.GroupQuestions
                .Where(q => q.PartId == PART_TOEIC.Part6)
                .OrderBy(q => Guid.NewGuid())
                .Take(4)
                .Select(q => q.Id)
                .ToList();
            var queryGroup = from groupQ in context.GroupQuestions
                where groupQ.PartId == PART_TOEIC.Part7
                select new
                {
                    Id = groupQ.Id,
                    countQuestion = context.QuestionToeics.Count(q => q.IdGroupQuestion == groupQ.Id)
                };
            var groupsWith2Questions = queryGroup
                .Where(g => g.countQuestion == 2)
                .OrderBy(x => Guid.NewGuid())
                .Take(4)
                .Select(g => g.Id)
                .ToList();

            var groupsWith3Questions = queryGroup
                .Where(g => g.countQuestion == 3)
                .OrderBy(x => Guid.NewGuid())
                .Take(3)
                .Select(g => g.Id)
                .ToList();

            var groupsWith4Questions = queryGroup
                .Where(g => g.countQuestion == 4)
                .OrderBy(x => Guid.NewGuid())
                .Take(3)
                .Select(g => g.Id)
                .ToList();

            var groupsWith5Questions = queryGroup
                .Where(g => g.countQuestion == 5)
                .OrderBy(x => Guid.NewGuid())
                .Take(5)
                .Select(g => g.Id)
                .ToList();

            newExamToeic.ListQuestionPart7 = newExamToeic.ListQuestionPart7 = groupsWith2Questions
                .Concat(groupsWith3Questions)
                .Concat(groupsWith4Questions)
                .Concat(groupsWith5Questions)
                .ToList();
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

    public async Task<object> GetAll(GetAllDto parameters)
    {
        try
        {
            var query = context.ExamToeics.AsQueryable();
            if (!string.IsNullOrEmpty(parameters.Keyword))
            {
                query = query.Where(x => x.NameExam.Contains(parameters.Keyword));
            }

            if (parameters.OrderBy.HasValue && parameters.OrderBy.Value)
            {
                query = query.OrderByDescending(x => x.CreationTime);
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

    public async Task<object> GetById(int id)
    {
        var exam = await (from examT in context.ExamToeics
            join user in context.Users
                on examT.CreatorId equals user.Id
            where examT.Id == id
            select new
            {
                Exam = examT,
                CreatorName = user.Name
            }).FirstOrDefaultAsync();

        if (exam == null)
        {
            return DataResult.ResultFail($"Grammar with ID {id} not found.");
        }

        var examDetails = new
        {
            Id = exam.Exam.Id,
            NameExam = exam.Exam.NameExam,
            CreationTime = exam.Exam.CreationTime,
            CreatorName = exam.CreatorName,
            Part1 = await GetQuestionsSingle(exam.Exam.ListQuestionPart1),
            Part2 = await GetQuestionsSingle(exam.Exam.ListQuestionPart2),
            Part3 = await GetQuestionOnGroup(exam.Exam.ListQuestionPart3),
            Part4 = await GetQuestionOnGroup(exam.Exam.ListQuestionPart4),
            Part5 = await GetQuestionsSingle(exam.Exam.ListQuestionPart5),
            Part6 = await GetQuestionOnGroup(exam.Exam.ListQuestionPart6),
            Part7 = await GetQuestionOnGroup(exam.Exam.ListQuestionPart7)
        };
        return DataResult.ResultSuccess(examDetails, "");
    }

    public async Task<object> GetByIdForUser(int id)
    {
        try
        {
            var exam = await context.ExamToeics
                .Where(e => e.Id == id)
                .FirstOrDefaultAsync();

            if (exam == null)
            {
                return DataResult.ResultFail($"Exam with ID {id} not found.");
            }
            var questions = new List<object>();
            questions.AddRange(await GetQuestionsSingle(exam.ListQuestionPart1));
            questions.AddRange(await GetQuestionsSingle(exam.ListQuestionPart2));
            questions.AddRange(await GetQuestionOnGroup(exam.ListQuestionPart3));
            questions.AddRange(await GetQuestionOnGroup(exam.ListQuestionPart4));
            questions.AddRange(await GetQuestionsSingle(exam.ListQuestionPart5));
            questions.AddRange(await GetQuestionOnGroup(exam.ListQuestionPart6));
            questions.AddRange(await GetQuestionOnGroup(exam.ListQuestionPart7));
            
            var examDetails = new
            {
                Id = exam.Id,
                NameExam = exam.NameExam,
                CreationTime = exam.CreationTime,
                QuestionsOnExam = questions  // All questions combined into one property
            };

            return DataResult.ResultSuccess(examDetails, "");

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private async Task<List<object>> GetQuestionsSingle(List<int>? questionIds)
    {
        if (questionIds == null || !questionIds.Any())
            return new List<object>();

        var queryQuestion = from question in context.QuestionToeics
            where questionIds.Contains(question.Id)
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

        var result = await queryQuestion.ToListAsync();
        var orderedResults = result.OrderBy(q => questionIds.IndexOf(q.Id)).ToList();
        return orderedResults.Cast<object>().ToList();
    }

    private async Task<List<object>> GetQuestionOnGroup(List<int>? questionIds)
    {
        if (questionIds == null || !questionIds.Any())
            return new List<object>();

        var queryQuestion = from groupQ in context.GroupQuestions
            where questionIds.Contains(groupQ.Id)
            select new
            {
                Id = groupQ.Id,
                AudioUrl = groupQ.AudioUrl,
                ImageUrl = groupQ.ImageUrl,
                Content = groupQ.Content,
                PartId = groupQ.PartId,
                Transcription = groupQ.Transcription,
                Questions = context.QuestionToeics.Where(q => q.IdGroupQuestion == groupQ.Id).Select(q => new
                {
                    Id = q.Id,
                    Content = q.Content,
                    NumberSTT = q.NumberSTT,
                    Type = q.Type,
                    Transcription = q.Transcription,
                    Answers = context.AnswerToeics.Where(a => a.IdQuestion == q.Id).Select(a => new
                    {
                        Id = a.Id,
                        Content = a.Content,
                        IsBoolean = a.IsBoolean,
                        Transcription = a.Transcription
                    }).ToList()
                }).ToList()
            };

        var result = await queryQuestion.ToListAsync();
        var orderedResults = result.OrderBy(q => questionIds.IndexOf(q.Id)).ToList();
        return orderedResults.Cast<object>().ToList();
    }

    public async Task<object> Update(ExamUpdateDto input)
    {
        var examToeicOld = await context.ExamToeics.FindAsync(input.Id);
        if (examToeicOld == null)
        {
            return DataResult.ResultFail($"Grammar with ID {input.Id} not found.");
        }

        _mapper.Map(input, examToeicOld);
        context.ExamToeics.Update(examToeicOld);
        await context.SaveChangesAsync();
        return DataResult.ResultSuccess(examToeicOld, "Update thành công");
    }

    public async Task<object> Delete(int id)
    {
        try
        {
            var examsToeic = await (from p in context.ExamToeics where (p.Id == id) select p).FirstOrDefaultAsync();

            if (examsToeic != null)
            {
                context.ExamToeics.Remove(examsToeic);
                await context.SaveChangesAsync();
                return DataResult.ResultSuccess(true, "");
            }
            else
            {
                return DataResult.ResultFail("Không tồn tại");
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}