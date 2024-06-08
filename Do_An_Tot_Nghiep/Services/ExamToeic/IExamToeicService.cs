using Do_An_Tot_Nghiep.Dto.ExamToeic;

namespace Do_An_Tot_Nghiep.Services.ExamToeic;

public interface IExamToeicService
{
    Task<object> Create(ExamCreateDto input);
    Task<object> CreateRandom(ExamCreateDto input);
    Task<object> GetAll(GetAllDto parameters);
    Task<object> Update(ExamUpdateDto input);
    Task<object> Delete(int id);
}