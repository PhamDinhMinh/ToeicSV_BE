using Do_An_Tot_Nghiep.Enums.Question;

namespace Do_An_Tot_Nghiep.Dto.Question;

public class CreateQuestionGroupDto
{
    public string? Content { get; set; }
    public PART_TOEIC PartId { get; set; }
    public string[]? ImageUrl { get; set; }
    public string? AudioUrl { get; set; }
    public int? IdExam { get; set; }
    public List<QuestionOnGroupDto> Questions { get; set; }
}

