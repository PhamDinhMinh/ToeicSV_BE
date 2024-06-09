using Do_An_Tot_Nghiep.Dto.Question;
using Do_An_Tot_Nghiep.Enums.Question;

namespace Do_An_Tot_Nghiep.Dto.Result;

public class DataResultDto
{
    public int Id { get; set; }
    public string? Content { get; set; }
    public PART_TOEIC? PartId { get; set; }
    public string? ImageUrl { get; set; }
    public string? AudioUrl { get; set; }
    public string? Transcription { get; set; }
    public int? NumberSTT { get; set; }
    public int? IdGroupQuestion { get; set; }
    public DateTime? CreationTime { get; set; }
    public CreateAnswerDto? Answer { get; set; }
}