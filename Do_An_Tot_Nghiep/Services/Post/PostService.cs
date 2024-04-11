using System.Net;
using System.Security.Claims;
using AutoMapper;
using Do_An_Tot_Nghiep.Dto.Post;
using Do_An_Tot_Nghiep.Services.PostComment;
using Microsoft.EntityFrameworkCore;
using NewProject.Services.Common;

namespace Do_An_Tot_Nghiep.Services.Post;

public class PostService : IPostService
{
    private readonly IDbServices _dbService;
    private PublicContext context = new PublicContext();
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;
    private readonly IPostCommentService _postCommentService;

    public PostService(IDbServices dbService, IMapper mapper, IPostCommentService postCommentService,
        IHttpContextAccessor httpContextAccessor)
    {
        _dbService = dbService;
        _mapper = mapper;
        _postCommentService = postCommentService;
        _httpContextAccessor = httpContextAccessor;
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
            if (_httpContextAccessor.HttpContext != null)
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
                        CountReact = (from react in context.PostReacts
                            where react.PostId == post.Id
                            select react).AsQueryable().Count(),
                        ReactStates = context.PostReacts
                            .Where(x => x.PostId == post.Id && x.CommentId == null && x.ReactState.HasValue)
                            .GroupBy(x => x.ReactState.Value)
                            .OrderByDescending(g => g.Count())
                            .Take(4)
                            .Select(grp => new {
                                State = grp.Key,
                                Count = grp.Count()
                            }).ToList(),
                        UserReact = context.PostReacts
                            .Where(x => x.PostId == post.Id &&
                                        x.CreatorUserId ==
                                        int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id")) &&
                                        x.CommentId == null)
                            .Select(react => react.ReactState)
                            .FirstOrDefault(),
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

                return DataResult.ResultSuccess(result, "", query.Count());
            }

            return DataResult.ResultFail("Đã có lỗi xảy ra");
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task<object> GetListPostUser(GetListPostUserDto parameters)
    {
        try
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                var query = (from post in context.Posts
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
                            CountReact = (from react in context.PostReacts
                                where react.PostId == post.Id
                                select react).AsQueryable().Count(),
                            UserReact = context.PostReacts
                                .Where(x => x.PostId == post.Id &&
                                            x.CreatorUserId ==
                                            int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id")) &&
                                            x.CommentId == null)
                                .Select(react => react.ReactState)
                                .FirstOrDefault(),
                            ReactStates = context.PostReacts
                                .Where(x => x.PostId == post.Id && x.CommentId == null && x.ReactState.HasValue)
                                .GroupBy(x => x.ReactState.Value)
                                .OrderByDescending(g => g.Count())
                                .Take(4)
                                .Select(grp => new {
                                    State = grp.Key,
                                    Count = grp.Count()
                                }).ToList(),
                            User = new
                            {
                                Id = user.Id,
                                Name = user.Name,
                                CoverImageUrl = user.CoverImageUrl,
                                ImageUrl = user.ImageUrl,
                                CreationTime = user.CreationTime
                            },
                            CreationTime = post.CreationTime
                        })
                    .Where(x => x.User.Id == parameters.UserId);
                query = query.OrderByDescending(x => x.CreationTime);
                var result = query.Skip(parameters.SkipCount).Take(parameters.MaxResultCount).ToList();
                return DataResult.ResultSuccess(result, "", query.Count());
            }

            return DataResult.ResultFail("Đã có lỗi xảy ra");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
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


    public async Task<object> Update(UpdatePostDto input)
    {
        try
        {
            var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == input.Id);
            if (post == null) return DataResult.ResultFail("Không tìm thấy bài viết");
            var newPost = _mapper.Map(input, post);
            context.Posts.Update(newPost);
            await context.SaveChangesAsync();
            return DataResult.ResultSuccess(newPost, "Update thành công");
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