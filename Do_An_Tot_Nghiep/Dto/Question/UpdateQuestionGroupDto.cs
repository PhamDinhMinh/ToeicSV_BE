using Do_An_Tot_Nghiep.Enums.Question;

namespace Do_An_Tot_Nghiep.Dto.Question;

public class UpdateQuestionGroupDto
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public PART_TOEIC PartId { get; set; }
    public string[]? ImageUrl { get; set; }
    public string? AudioUrl { get; set; }
    public int? IdExam { get; set; }
    public string? Transcription { get; set; }
    public List<UpdateQuestionOnGroupDto> Questions { get; set; }
}

public class UpdateQuestionOnGroupDto
{
    public int Id { get; set; }
    public List<int>? Type { get; set; }
    public string? Transcription { get; set; }
    public int? NumberSTT { get; set; }
    public string? Content { get; set; }
    public List<UpdateAnswerDto>? Answers { get; set; }
}