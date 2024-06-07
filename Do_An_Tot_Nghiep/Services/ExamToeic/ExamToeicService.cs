using System.Security.Claims;
using AutoMapper;
using Do_An_Tot_Nghiep.Dto.ExamToeic;
using Do_An_Tot_Nghiep.Enums.Question;
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
            newExamToeic.ListQuestionPart3 = context.GroupQuestions
                .Where(q => q.PartId == PART_TOEIC.Part3)
                .OrderBy(q => Guid.NewGuid())
                .Take(13)
                .Select(q => q.Id)
                .ToList();
            newExamToeic.ListQuestionPart4 = context.GroupQuestions
                .Where(q => q.PartId == PART_TOEIC.Part4)
                .OrderBy(q => Guid.NewGuid())
                .Take(10)
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