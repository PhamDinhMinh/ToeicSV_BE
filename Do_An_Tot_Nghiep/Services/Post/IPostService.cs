using Do_An_Tot_Nghiep.Dto.Post;

namespace Do_An_Tot_Nghiep.Services.Post;

public interface IPostService
{
    Task<object> Create(CreatePostDto input);
    Task<object> GetListPost(GetListPostDto parameters);
    Task<object> GetListPostUser(GetListPostUserDto parameters);
    Task<object> GetUserWallPost(int id);
    Task<object> Update(UpdatePostDto input);
    Task<object> Delete(int id);
}