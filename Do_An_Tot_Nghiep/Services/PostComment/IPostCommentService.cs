using Do_An_Tot_Nghiep.Dto.PostComment;

namespace Do_An_Tot_Nghiep.Services.PostComment;

public interface IPostCommentService
{
    Task<object> GetAll(GetPostCommentDto parameter);
    Task<object> Create(CreatePostCommentDto input);
    Task<object> Update(UpdatePostCommentDto input);
    Task<object> Delete(int id);
}