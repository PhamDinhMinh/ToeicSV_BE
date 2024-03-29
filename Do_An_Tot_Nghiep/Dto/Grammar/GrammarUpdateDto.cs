using Do_An_Tot_Nghiep.Enums.Grammar;

namespace Do_An_Tot_Nghiep.Dto.Grammar;

public class GrammarUpdateDto
{
    public string Title { get; set; }
    public string Content { get; set; }
    public Boolean? IsWatched { get; set; } = false;
    public EGRAMMAR_TYPE Type { get; set; }
    public int CreatorId { get; set; }
}