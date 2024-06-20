using System.ComponentModel.DataAnnotations;

namespace Do_An_Tot_Nghiep.Dto.Statistics;

public class GetOrdinalDto
{
    public string? UserName { get; set; }
    [Range(0, 2147483647)] public int SkipCount { get; set; } = 0;
    [Range(1, 2147483647)] public int MaxResultCount { get; set; } = 1000;
}