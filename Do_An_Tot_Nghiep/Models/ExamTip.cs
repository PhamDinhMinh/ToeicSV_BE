using System.ComponentModel.DataAnnotations.Schema;
using Do_An_Tot_Nghiep.Enums.ExamTips;

namespace Do_An_Tot_Nghiep.Models;

[Table("exam_tips", Schema = "do_an")]

public class ExamTip
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string[] Description { get; set; }
    public EEXAM_TIPS_TYPE Type { get; set; } = EEXAM_TIPS_TYPE.Part1;
    public int CreatorId { get; set; }
    public DateTime? CreationTime { get; set; }
}