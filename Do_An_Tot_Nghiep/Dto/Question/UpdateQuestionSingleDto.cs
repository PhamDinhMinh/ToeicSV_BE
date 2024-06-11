using Do_An_Tot_Nghiep.Enums.Question;

namespace Do_An_Tot_Nghiep.Dto.Question;

public class UpdateQuestionSingleDto
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public PART_TOEIC? PartId { get; set; }
    public List<int>? Type { get; set; }
    public string[]? ImageUrl { get; set; }
    public string? AudioUrl { get; set; }
    public string? Transcription { get; set; }
    public int? Index { get; set; } = 1;
    public int? NumberSTT { get; set; }
    public List<UpdateAnswerDto>? Answers { get; set; }
}

public class UpdateAnswerDto
{
    public int Id { get; set; }
    public bool IsBoolean { get; set; }
    public string? Content { get; set; }
}