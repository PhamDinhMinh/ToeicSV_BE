using Do_An_Tot_Nghiep.Enums.Grammar;

namespace Do_An_Tot_Nghiep.Dto.Grammar;

public class GrammarCreateDto
{
    public string Title { get; set; }
    public string Content { get; set; }
    public EGRAMMAR_TYPE Type { get; set; } = EGRAMMAR_TYPE.Basic;
    public int CreatorId { get; set; }
}