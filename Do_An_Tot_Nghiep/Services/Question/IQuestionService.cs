using Do_An_Tot_Nghiep.Dto.Question;

namespace Do_An_Tot_Nghiep.Services.Question;

public interface IQuestionService
{
    Task<object> CreateQuestionSingle(CreateQuestionSingleDto parameters);
    Task<object> CreateQuestionGroup(CreateQuestionGroupDto parameters);
    
    
}