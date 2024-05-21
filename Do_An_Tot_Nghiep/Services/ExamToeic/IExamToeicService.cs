using Do_An_Tot_Nghiep.Dto.ExamToeic;

namespace Do_An_Tot_Nghiep.Services.ExamToeic;

public interface IExamToeicService
{
    Task<object> Create(ExamCreateDto input);
}