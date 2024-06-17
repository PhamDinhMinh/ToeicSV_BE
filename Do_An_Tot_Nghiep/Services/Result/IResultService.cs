using Do_An_Tot_Nghiep.Dto.Result;

namespace Do_An_Tot_Nghiep.Services.Result;

public interface IResultService
{
    Task<object> Submit(SubmitQuestionDto input);
    Task<object> GetById(int id);
}