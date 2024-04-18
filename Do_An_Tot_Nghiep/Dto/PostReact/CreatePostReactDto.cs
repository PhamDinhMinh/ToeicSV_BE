using Do_An_Tot_Nghiep.Enums.PostReact;

namespace Do_An_Tot_Nghiep.Dto.PostReact;

public class CreatePostReactDto
{
    public DateTime? CreationTime { get; set; }
    public ESTATE_POST_REACT? ReactState { get; set; }
    public int? CommentId { get; set; }
    public int? PostId { get; set; }
    public bool IsCancel { get; set; }
    public int CreatorUserId { get; set; }
}