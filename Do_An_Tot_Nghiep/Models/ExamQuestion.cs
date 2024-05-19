using System.ComponentModel.DataAnnotations.Schema;

namespace Do_An_Tot_Nghiep.Models;

[Table("exam_question", Schema = "do_an")]

public class ExamQuestion
{
    public int Id { get; set; }
    public int IdQuestion { get; set; }
    public int IdExam { get; set; }
}