using System.ComponentModel.DataAnnotations;
using Do_An_Tot_Nghiep.Enums.Post;

namespace Do_An_Tot_Nghiep.Dto.Post;

public class GetListPostUserDto
{
    public int UserId { get; set; }
    [Range(0, 2147483647)] public int SkipCount { get; set; } = 0;
    [Range(1, 2147483647)] public int MaxResultCount { get; set; } = 1000;
}