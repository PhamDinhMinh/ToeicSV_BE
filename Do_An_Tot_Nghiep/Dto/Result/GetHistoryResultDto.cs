using System.ComponentModel.DataAnnotations;
using Do_An_Tot_Nghiep.Enums.PostReact;

namespace Do_An_Tot_Nghiep.Dto.Result;

public class GetHistoryResultDto
{
    public int? ExamId { get; set; }
    [Range(0, 2147483647)] public int SkipCount { get; set; } = 0;
    [Range(1, 2147483647)] public int MaxResultCount { get; set; } = 1000;
}