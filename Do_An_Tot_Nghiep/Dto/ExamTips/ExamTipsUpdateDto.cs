using Do_An_Tot_Nghiep.Enums.ExamTips;

namespace Do_An_Tot_Nghiep.Dto.ExamTips;

public class ExamTipsUpdateDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string[] Description { get; set; }
    public EEXAM_TIPS_TYPE? Type { get; set; }
}