using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Do_An_Tot_Nghiep.Enums;
using Do_An_Tot_Nghiep.Enums.Grammar;

namespace Do_An_Tot_Nghiep.Models;

[Table("grammars")]
public class Grammar
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Boolean? IsWatched { get; set; } = false;
    public EGRAMMAR_TYPE Type { get; set; } = EGRAMMAR_TYPE.Basic;
    public int CreatorId { get; set; }
    public DateTime? CreationTime { get; set; }
}