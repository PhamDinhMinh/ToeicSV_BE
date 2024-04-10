using System.ComponentModel.DataAnnotations;
using Do_An_Tot_Nghiep.Enums.Post;

namespace Do_An_Tot_Nghiep.Dto.Post;

public class GetListPostDto
{
    public ESTATE_OF_POST? State { get; set; } 
    public string? Keyword { get; set; }
    [Range(0, 2147483647)] public int SkipCount { get; set; } = 0;
    [Range(1, 2147483647)] public int MaxResultCount { get; set; } = 1000;
}