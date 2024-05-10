using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Do_An_Tot_Nghiep.Enums.PostReact;

namespace Do_An_Tot_Nghiep.Models;

[Table("post_react")]

public class PostReact
{
    [Key]
    public int Id { get; set; }
    public ESTATE_POST_REACT? ReactState { get; set; }
    public int? CommentId { get; set; }
    public int? PostId { get; set; }
    public int? CreatorUserId { get; set; }
    public DateTime? CreationTime { get; set; }
}