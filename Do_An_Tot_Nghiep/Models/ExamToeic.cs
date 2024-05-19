using System.ComponentModel.DataAnnotations.Schema;

namespace Do_An_Tot_Nghiep.Models;

[Table("exam_toeic", Schema = "do_an")]

public class ExamToeic
{
    public int Id { get; set; }
    public string NameExam { get; set; }
    public int CreatorId { get; set; }
    public DateTime? CreationTime { get; set; } 
}