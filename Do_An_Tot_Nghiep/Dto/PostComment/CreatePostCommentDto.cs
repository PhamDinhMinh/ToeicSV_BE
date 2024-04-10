namespace Do_An_Tot_Nghiep.Dto.PostComment;

public class CreatePostCommentDto
{
    public string Comment { get; set; }
    public int? ParentCommentId { get; set; }
    public int PostId { get; set; }
    public int CreatorUserId { get; set; }
}