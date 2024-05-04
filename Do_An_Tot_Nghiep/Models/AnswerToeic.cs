using System.ComponentModel.DataAnnotations.Schema;

namespace Do_An_Tot_Nghiep.Models;

[Table("answer_toeic", Schema = "do_an")]
public class AnswerToeic
{
    public int Id { get; set; }
    public int  IdQuestion { get; set; }
    public bool IsBoolean { get; set; }
    public string? Content { get; set; }
    public int STTAnswer { get; set; }
    public string? Transcription { get; set; }
}