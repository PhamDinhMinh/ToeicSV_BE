using Do_An_Tot_Nghiep.Dto.ExamTips;

namespace Do_An_Tot_Nghiep.Services.ExamTip;

public interface IExamTipsService
{
    Task<object> Create(ExamTipsCreateDto input);

    Task<object> GetAll(ExamTipGetDto input);
    Task<object> Update(ExamTipsUpdateDto input);
    Task<object> Delete(int id);
}