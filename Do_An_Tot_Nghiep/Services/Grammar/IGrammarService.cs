using Do_An_Tot_Nghiep.Dto.Grammar;

namespace Do_An_Tot_Nghiep.Services.Grammar;

public interface IGrammarService
{
    Task<object> GetAll(GetGrammarDto parameter);
    Task<object> Create(GrammarCreateDto input);
    Task<object> Update(GrammarUpdateDto input);
    Task<object> UpdateWatch(GrammarUpdateWatchDto input);
    Task<object> Delete(int id);
}