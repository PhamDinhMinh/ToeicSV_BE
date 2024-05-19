using Do_An_Tot_Nghiep.Dto.Question;

namespace Do_An_Tot_Nghiep.Services.Question;

public interface IQuestionService
{
    Task<object> CreateQuestionSingle(CreateQuestionSingleDto input);
    Task<object> CreateQuestionGroup(CreateQuestionGroupDto input);
    Task<object> GetListQuestionSingle(GetListQuestionSingleDto parameters);
    Task<object> GetListQuestionGroup(GetListQuestionGroupDto parameters);
    Task<object> GetListQuestion(GetListQuestionDto parameters);
    
    // User
    Task<object> GetQuestionUser(GetQuestionUserDto parameters);
}