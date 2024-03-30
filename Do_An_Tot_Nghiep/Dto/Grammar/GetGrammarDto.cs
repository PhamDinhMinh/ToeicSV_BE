using System.ComponentModel.DataAnnotations;
using Do_An_Tot_Nghiep.Enums.Grammar;

namespace Do_An_Tot_Nghiep.Dto.Grammar;

public class GetGrammarDto
{
    public EGRAMMAR_TYPE? Type { get; set; } 
    public string? Keyword { get; set; }
    [Range(0, 2147483647)] public int SkipCount { get; set; } = 0;
    [Range(1, 2147483647)] public int MaxResultCount { get; set; } = 1000;
}