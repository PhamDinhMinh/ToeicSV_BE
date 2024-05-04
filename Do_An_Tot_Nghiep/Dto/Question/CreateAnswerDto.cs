namespace Do_An_Tot_Nghiep.Dto.Question;

public class CreateAnswerDto
{
    public bool IsBoolean { get; set; }
    public string? Content { get; set; }
    public int STTAnswer { get; set; }
    public string? Transcription { get; set; }
}