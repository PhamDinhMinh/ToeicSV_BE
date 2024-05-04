using Do_An_Tot_Nghiep.Enums.Question;

namespace Do_An_Tot_Nghiep.Dto.Question;

public class CreateQuestionDto
{
    public string? Content { get; set; }
    public PART_TOEIC PartId { get; set; }
    public int? Type { get; set; }
    public string[]? ImageUrl { get; set; }
    public string? AudioUrl { get; set; }
    public CreateGroupQuestionDto GroupQuestion { get; set; }
    public string? Transcription { get; set; }
    public int? Index { get; set; } = 1;
    public int? NumberSTT { get; set; }
    public int? IdExam { get; set; }
    public List<CreateAnswerDto>? Answers { get; set; }
}