using System.ComponentModel.DataAnnotations;
using Do_An_Tot_Nghiep.Enums.Question;

namespace Do_An_Tot_Nghiep.Dto.Question;

public class GetListQuestionSingleDto
{
    public PART_TOEIC? PartId { get; set; } 
    public int? Type { get; set; }
    public string? Keyword { get; set; }
    public bool? OrderBy { get; set; }
    [Range(0, 2147483647)] public int SkipCount { get; set; } = 0;
    [Range(1, 2147483647)] public int MaxResultCount { get; set; } = 1000;
}