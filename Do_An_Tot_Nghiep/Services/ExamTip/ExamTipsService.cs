using AutoMapper;
using Do_An_Tot_Nghiep.Dto.ExamTips;
using Microsoft.EntityFrameworkCore;
using NewProject.Services.Common;

namespace Do_An_Tot_Nghiep.Services.ExamTip;

public class ExamTipsService : IExamTipsService
{
    private readonly IDbServices _dbService;
    private PublicContext context = new PublicContext();
    private readonly IMapper _mapper;

    public ExamTipsService(IDbServices dbService, IMapper mapper)
    {
        _dbService = dbService;
        _mapper = mapper;
    }
    public async Task<object> Create(ExamTipsCreateDto input)
    {
        try
        {
            var newExamTips = _mapper.Map<Models.ExamTip>(input);

            newExamTips.CreationTime = DateTime.UtcNow;
            await context.ExamTips.AddAsync(newExamTips);
            await context.SaveChangesAsync(); 
            return DataResult.ResultSuccess("Thêm thành công");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<object> Update(ExamTipsUpdateDto input)
    {
        try
        {
            var examTip = await context.ExamTips.FindAsync(input.Id);
            if (examTip==null)
            {
                return DataResult.ResultFail($"Grammar with ID {input.Id} not found.");
            }
            _mapper.Map(input, examTip);
            context.ExamTips.Update(examTip);
            await context.SaveChangesAsync();
            return DataResult.ResultSuccess(examTip, "Update thành công");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<object> GetAll(ExamTipGetDto parameters)
    {
        try
        {
            {
                var query = context.ExamTips.AsQueryable();
                if (parameters.Type.HasValue)
                {
                    query = query.Where(x => x.Type == parameters.Type);
                }

                if (!string.IsNullOrEmpty(parameters.Keyword))
                {
                    query = query.Where(x => x.Title.Contains(parameters.Keyword));
                }
                query = query.OrderByDescending(x => x.CreationTime);
                var result = query.Skip(parameters.SkipCount).Take(parameters.MaxResultCount).ToList();

                return DataResult.ResultSuccess(result, "", query.Count());;
            }
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
            var examTips = await (from p in context.ExamTips where (p.Id == id) select p).FirstOrDefaultAsync();
    
            if (examTips != null)
            {
                context.ExamTips.Remove(examTips);
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