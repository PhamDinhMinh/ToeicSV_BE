using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Do_An_Tot_Nghiep.Enums.ExamTips;

namespace Do_An_Tot_Nghiep.Models;

[Table("exam_tips")]

public class ExamTip
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public string[] Description { get; set; }
    public EEXAM_TIPS_TYPE Type { get; set; } = EEXAM_TIPS_TYPE.Part1;
    public int CreatorId { get; set; }
    public DateTime? CreationTime { get; set; }
}