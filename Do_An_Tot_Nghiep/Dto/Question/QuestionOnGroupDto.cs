namespace Do_An_Tot_Nghiep.Dto.Question;

public class QuestionOnGroupDto
{
    public string? Transcription { get; set; }
    public int? Index { get; set; } = 1;
    public int? NumberSTT { get; set; }
    public int? Content { get; set; }
    public List<CreateAnswerDto>? Answers { get; set; }
}