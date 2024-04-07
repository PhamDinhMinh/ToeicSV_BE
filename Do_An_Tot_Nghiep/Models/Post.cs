using System.ComponentModel.DataAnnotations.Schema;
using Do_An_Tot_Nghiep.Enums.Post;

namespace Do_An_Tot_Nghiep.Models;

[Table("post", Schema = "do_an")]
public class Post
{
    public int Id { get; set; }
    public string ContentPost { get; set; }
    public List<string> ImageUrls { get; set; }
    public int? BackGroundId { get; set; }
    public int? EmotionId { get; set; }
    public ESTATE_OF_POST? State { get; set; } = ESTATE_OF_POST.Verified;
    public int CreatorUserId { get; set; }
    public int SharedPostId { get; set; }
    public DateTime? CreationTime { get; set; }
}