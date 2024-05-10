using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Do_An_Tot_Nghiep.Enums.Question;

namespace Do_An_Tot_Nghiep.Models;

[Table("question_toeic")]
public class QuestionToeic
{
    [Key]
    public int Id { get; set; }
    public string? Content { get; set; }
    public PART_TOEIC? PartId { get; set; }
    public List<int>? Type { get; set; }
    public string[]? ImageUrl { get; set; }
    public string? AudioUrl { get; set; }
    public int? IdGroupQuestion { get; set; }
    public string? Transcription { get; set; }
    public int? Index { get; set; } = 1;
    public int? NumberSTT { get; set; }
    public int? IdExam { get; set; }
}