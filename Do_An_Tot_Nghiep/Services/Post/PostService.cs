using System.Net;
using System.Security.Claims;
using AutoMapper;
using Do_An_Tot_Nghiep.Dto.Post;
using Do_An_Tot_Nghiep.Services.PostComment;
using Microsoft.EntityFrameworkCore;
using NewProject.Services.Common;

namespace Do_An_Tot_Nghiep.Services.Post;

public class PostService: IPostService
{
    private readonly IDbServices _dbService;
    private PublicContext context = new PublicContext();
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly IPostCommentService _postCommentService;

    public PostService(IDbServices dbService, IMapper mapper, IPostCommentService postCommentService)
    {
        _dbService = dbService;
        _mapper = mapper;
        _postCommentService = postCommentService;
    }

    public async Task<object> Create(CreatePostDto input)
    {
        var newPost = _mapper.Map<Models.Post>(input);

        newPost.CreationTime = DateTime.UtcNow;
        await context.Posts.AddAsync(newPost);
        await context.SaveChangesAsync(); 
        return DataResult.ResultSuccess(newPost, "Tạo mới thành công");
    }

    public async Task<object> GetListPost(GetListPostDto parameters)
    {
        try
        {
            var query = from post in context.Posts
                join user in context.Users
                    on post.CreatorUserId equals user.Id
                select new
                {
                    Id = post.Id,
                    ContentPost = post.ContentPost,
                    State = post.State,
                    ImageUrls = post.ImageUrls,
                    EmotionId = post.EmotionId,
                    BackGroundId = post.BackGroundId,
                    SharedPostId = post.SharedPostId,
                    CountComment = (from comment in context.PostComments
                        where comment.PostId == post.Id
                        select comment).AsQueryable().Count(),
                    User = new 
                    {
                        Id = user.Id,
                        Name = user.Name,
                        CoverImageUrl = user.CoverImageUrl,
                        ImageUrl = user.ImageUrl,
                        CreationTime = user.CreationTime
                    },
                    CreationTime = post.CreationTime
                };
            
            if (!string.IsNullOrEmpty(parameters.Keyword))
            {
                query = query.Where(x => x.ContentPost.Contains(parameters.Keyword));
            }
            query = query.OrderByDescending(x => x.CreationTime);
            var result = query.Skip(parameters.SkipCount).Take(parameters.MaxResultCount).ToList();
            // foreach (var item in result)
            // {
            //     item.
            //         .Where(x => x.PostId == item.Id && x.CommentId == null && x.ReactState.HasValue)
            //         .Select(x => x.ReactState.Value).ToList().GroupBy(x => x).Select(grp => new StateOrder()
            //         {
            //             State = grp.Key,
            //             Count = grp.Count()
            //         }).ToList();
            //     item.User.FriendshipState = friends.Where(x => x.FriendUserId == item.CreatorUserId).Select(x => x.State).FirstOrDefault();
            //     item.TenantZone = item.TenantId == YootekSession.TenantId ? tenantZone : allTenants.FirstOrDefault(x =>
            // }
            
            
            return DataResult.ResultSuccess(result, "", query.Count());
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<object> GetUserWallPost(int id)
    {
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(p => p.Id == id);
            if (user == null)
                return null;
            var totalPosts = await context.Posts.CountAsync(post => post.CreatorUserId == id);
            var result = new 
            {
                User = user,
                TotalPosts = totalPosts,
            };

            return DataResult.ResultSuccess(result, "");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
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