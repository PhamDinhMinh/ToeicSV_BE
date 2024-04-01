using System.Net;
using AutoMapper;
using Do_An_Tot_Nghiep.Dto.Grammar;
using Microsoft.EntityFrameworkCore;
using NewProject.Services.Common;

namespace Do_An_Tot_Nghiep.Services.Grammar;

public class GrammarService : IGrammarService
{
    private readonly IDbServices _dbService;
    private PublicContext context = new PublicContext();
    private readonly IMapper _mapper;

    public GrammarService(IDbServices dbService, IMapper mapper)
    {
        _dbService = dbService;
        _mapper = mapper;
    }

    public async Task<object> GetAll(GetGrammarDto parameters)
    {
        var query = context.Grammars.AsQueryable();
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

    public async Task<object> Create(GrammarCreateDto input)
    {
        var newGrammar = _mapper.Map<Models.Grammar>(input);

        newGrammar.CreationTime = DateTime.UtcNow;
        await context.Grammars.AddAsync(newGrammar);
        await context.SaveChangesAsync(); 
        return DataResult.ResultSuccess("Thêm thành công");
    }

    public async Task<object> Update(GrammarUpdateDto input)
    {
        try
        {
            var grammar = await context.Grammars.FindAsync(input.Id);
            if (grammar==null)
            {
                return DataResult.ResultFail($"Grammar with ID {input.Id} not found.");
            }
            _mapper.Map(input, grammar);
            // context.Entry(grammar).CurrentValues.SetValues(input);
            context.Grammars.Update(grammar);
            await context.SaveChangesAsync();
            return DataResult.ResultSuccess(grammar, "Update thành công");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<object> UpdateWatch(GrammarUpdateWatchDto input)
    {
        try
        {
            var grammarUpdateWatch = await context.Grammars.FindAsync(input.Id);
            
            if (grammarUpdateWatch == null)
            {
                return DataResult.ResultFail($"Grammar with ID {input.Id} not found.");
            }
            _mapper.Map(input, grammarUpdateWatch);
            context.Grammars.Update(grammarUpdateWatch);
            await context.SaveChangesAsync();
            return DataResult.ResultSuccess( "Update thành công");
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
            var grammar = await (from p in context.Grammars where (p.Id == id) select p).FirstOrDefaultAsync();

            if (grammar != null)
            {
                context.Grammars.Remove(grammar);
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