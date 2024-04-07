using System.Net;
using System.Security.Claims;
using AutoMapper;
using Do_An_Tot_Nghiep.Dto.Post;
using Microsoft.EntityFrameworkCore;
using NewProject.Services.Common;

namespace Do_An_Tot_Nghiep.Services.Post;

public class PostService: IPostService
{
    private readonly IDbServices _dbService;
    private PublicContext context = new PublicContext();
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public PostService(IDbServices dbService, IMapper mapper)
    {
        _dbService = dbService;
        _mapper = mapper;
    }

    public async Task<object> Create(CreatePostDto input)
    {
        var newPost = _mapper.Map<Models.Post>(input);

        newPost.CreationTime = DateTime.UtcNow;
        await context.Posts.AddAsync(newPost);
        await context.SaveChangesAsync(); 
        return DataResult.ResultSuccess(newPost, "Tạo mới thành công");
    }

    public async Task<object> Delete(int id)
    {
        try
        {
            var post = await (from p in context.Posts where (p.Id == id) select p).FirstOrDefaultAsync();
            if (post == null)
            {
                return DataResult.ResultFail("Không tồn tại bài viết");
            }

            if (_httpContextAccessor.HttpContext != null)
            {
                if (post.CreatorUserId == int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id")))
                {
                    context.Posts.Remove(post);
                    await context.SaveChangesAsync();
                    return DataResult.ResultSuccess(true, "Xoá bài viết thành công", (int)HttpStatusCode.OK);
                }
                else
                {
                    return DataResult.ResultFail("Bạn không có quyền thực hiện thao tác này",
                        (int)HttpStatusCode.Forbidden);
                }
            }
            return DataResult.ResultFail("Bạn không có quyền thực hiện thao tác này", (int)HttpStatusCode.Forbidden);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}