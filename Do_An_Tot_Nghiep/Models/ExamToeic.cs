using System.ComponentModel.DataAnnotations.Schema;

namespace Do_An_Tot_Nghiep.Models;

[Table("exam_toeic", Schema = "do_an")]

public class ExamToeic
{
    public int Id { get; set; }
    public string NameExam { get; set; }
    public List<int>? ListQuestionPart1 { get; set; }
    public List<int>? ListQuestionPart2 { get; set; }
    public List<int>? ListQuestionPart3 { get; set; }
    public List<int>? ListQuestionPart4 { get; set; }
    public List<int>? ListQuestionPart5 { get; set; }
    public List<int>? ListQuestionPart6 { get; set; }
    public List<int>? ListQuestionPart7 { get; set; }
    public int? CreatorId { get; set; }
    public DateTime? CreationTime { get; set; } 
}