using System.ComponentModel.DataAnnotations;

namespace Do_An_Tot_Nghiep.Dto.PostComment;

public class GetPostCommentDto
{
    public int PostId { get; set; }
    public int? ParentCommentId { get; set; }
    public string? Keyword { get; set; }
    [Range(0, 2147483647)] public int SkipCount { get; set; } = 0;
    [Range(1, 2147483647)] public int MaxResultCount { get; set; } = 1000;
}