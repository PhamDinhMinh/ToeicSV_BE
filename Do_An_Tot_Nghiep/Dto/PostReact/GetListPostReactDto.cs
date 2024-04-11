using System.ComponentModel.DataAnnotations;
using Do_An_Tot_Nghiep.Enums.PostReact;

namespace Do_An_Tot_Nghiep.Dto.PostReact;

public class GetListPostReactDto
{
    public ESTATE_POST_REACT ReactState { get; set; }
    public int? CommentId { get; set; }
    public int PostId { get; set; }
    [Range(0, 2147483647)] public int SkipCount { get; set; } = 0;
    [Range(1, 2147483647)] public int MaxResultCount { get; set; } = 1000;
}