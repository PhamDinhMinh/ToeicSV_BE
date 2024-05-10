using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Do_An_Tot_Nghiep.Models;

[Table("post_comment")]
public class PostComment
{
    [Key]
    public int Id { get; set; }
    public string Comment { get; set; }
    public int? ParentCommentId { get; set; }
    public int PostId { get; set; }
    public int CreatorUserId { get; set; }
    public DateTime? CreationTime { get; set; }
}