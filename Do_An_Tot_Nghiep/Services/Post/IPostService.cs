using Do_An_Tot_Nghiep.Dto.Post;

namespace Do_An_Tot_Nghiep.Services.Post;

public interface IPostService
{
    Task<object> Create(CreatePostDto input);
    Task<object> GetListPost(GetListPostDto parameters);
    Task<object> GetUserWallPost(int id);
    Task<object> Delete(int id);
}