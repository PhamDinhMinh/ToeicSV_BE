using Do_An_Tot_Nghiep.Enums.Question;

namespace Do_An_Tot_Nghiep.Dto.Question;

public class CreateGroupQuestionDto
{
    public string? Content { get; set; }
    public PART_TOEIC PartId { get; set; }
    public int? Type { get; set; }
    public string[]? ImageUrl { get; set; }
    public string? AudioUrl { get; set; }
    public int? IdExam { get; set; }
}