using System.Net;
using AutoMapper;
using Do_An_Tot_Nghiep.Dto.Grammar;
using Do_An_Tot_Nghiep.Services.Auth;
using NewProject.Services.Common;

namespace Do_An_Tot_Nghiep.Services.Grammar;

public class GrammarService : IGrammarService
{
    private readonly IDbServices _dbService;
    private PublicContext context = new PublicContext();
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public GrammarService(IDbServices dbService, IMapper mapper, IAuthService authService)
    {
        _dbService = dbService;
        _mapper = mapper;
        _authService = authService;
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

    public Task<object> Update(GrammarUpdateDto input)
    {
        throw new NotImplementedException();
    }

    public Task<object> UpdateWatch(GrammarUpdateWatchDto input)
    {
        throw new NotImplementedException();
    }

    public async Task<object> Delete(int id)
    {
        return true;
    }
}