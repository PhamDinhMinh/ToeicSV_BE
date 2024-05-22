using System.ComponentModel.DataAnnotations.Schema;

namespace Do_An_Tot_Nghiep.Models;

[Table("result", Schema = "do_an")]

public class Result
{
    public int Id { get; set; }
    public string? Data { get; set; }
    public int? UserId { get; set; }
    public DateTime? TimeStart { get; set; }
    public DateTime? TimeEnd { get; set; }
    public int? IdExam { get; set; }
}