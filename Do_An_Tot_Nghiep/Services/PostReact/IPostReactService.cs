using Do_An_Tot_Nghiep.Dto.PostReact;

namespace Do_An_Tot_Nghiep.Services.PostReact;

public interface IPostReactService
{
    Task<object> GetListReact(GetListPostReactDto parameters);

    Task<object> CreateOrUpdate(CreatePostReactDto input);
}