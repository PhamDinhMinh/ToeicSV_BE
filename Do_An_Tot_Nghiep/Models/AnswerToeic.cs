using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Do_An_Tot_Nghiep.Models;

[Table("answer_toeic")]
public class AnswerToeic
{
    [Key]
    public int Id { get; set; }
    public int  IdQuestion { get; set; }
    public bool IsBoolean { get; set; }
    public string? Content { get; set; }
    public int? STTAnswer { get; set; }
    public string? Transcription { get; set; }
}