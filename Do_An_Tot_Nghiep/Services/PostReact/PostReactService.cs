using System.Net;
using System.Security.Claims;
using AutoMapper;
using Do_An_Tot_Nghiep.Dto.Post;
using Do_An_Tot_Nghiep.Dto.PostReact;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NewProject.Services.Common;

namespace Do_An_Tot_Nghiep.Services.PostReact;

public class PostReactService : IPostReactService
{
    private readonly IDbServices _dbService;
    private PublicContext context = new PublicContext();
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public PostReactService(IDbServices dbService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _dbService = dbService;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<object> GetListReact(GetListPostReactDto parameters)
    {
        var query = (from pReact in context.PostReacts
                join user in context.Users
                    on pReact.CreatorUserId equals user.Id
                select new
                {
                    Id = pReact.Id,
                    PostId = pReact.PostId,
                    CreatorUserId = pReact.CreatorUserId,
                    CreationTime = pReact.CreationTime,
                    ReactState = pReact.ReactState,
                    CommentId = pReact.CommentId,
                    User = new
                    {
                        Id = user.Id,
                        Name = user.Name,
                        CoverImageUrl = user.CoverImageUrl,
                        ImageUrl = user.ImageUrl,
                        CreationTime = user.CreationTime
                    },
                })
            .Where(x => x.PostId == parameters.PostId)
            .Where(x => x.ReactState != null);
        if (parameters.CommentId.HasValue)
        {
            query = query.Where(x => x.CommentId == parameters.CommentId);
        }
        else
        {
            query = query.Where(x => x.CommentId == null);
        }

        query = query.OrderByDescending(x => x.CreationTime);
        var result = query.Skip(parameters.SkipCount).Take(parameters.MaxResultCount).ToList();
        return DataResult.ResultSuccess(result, "", query.Count());
    }

    public async Task<object> CreateOrUpdate(CreatePostReactDto input)
    {
        try
        {
            if (input.IsCancel)
            {
                if (_httpContextAccessor.HttpContext != null)
                {
                    var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id"));
                    var check = await context.PostReacts.FirstOrDefaultAsync(x =>
                        x.CreatorUserId == userId
                        && ((input.PostId.HasValue && x.PostId == input.PostId) ||
                            (input.CommentId.HasValue && x.CommentId == input.CommentId)));
                    if (check != null) context.PostReacts.Remove(check);
                    await context.SaveChangesAsync();
                    return DataResult.ResultSuccess("Success");
                }
                else
                {
                    DataResult.ResultFail("Bạn không có quyền thực hiện thao tác này", (int)HttpStatusCode.Forbidden);
                }
            }

            if (input.PostId.HasValue && input.PostId.Value > 0)
            {
                if (_httpContextAccessor.HttpContext != null)
                {
                    var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == input.PostId);
                    if (post == null) throw new Exception("Post not found");
                    var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id"));
                    var check = await context.PostReacts.FirstOrDefaultAsync(x =>
                        x.PostId == input.PostId && x.CreatorUserId == userId);
                    if (check == null)
                    {
                        var react = new Models.PostReact()
                        {
                            PostId = input.PostId,
                            ReactState = input.ReactState,
                            CreatorUserId = userId,
                            CreationTime = DateTime.Now,
                        };
                        await context.PostReacts.AddAsync(react);
                    }
                    else
                    {
                        check.ReactState = input.ReactState;
                        context.PostReacts.Update(check);
                    }
                }
            }
            else if (input.CommentId > 0)
            {
                var comment = await context.PostComments.FirstOrDefaultAsync(x => x.Id == input.CommentId);
                if (comment == null) throw new Exception("Comment not found");
                if (_httpContextAccessor.HttpContext != null)
                {
                    var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id"));
                    var check = await context.PostReacts.FirstOrDefaultAsync(x =>
                        x.CommentId == input.CommentId && x.CreatorUserId == userId);
                    if (check == null)
                    {
                        var react = new Models.PostReact()
                        {
                            PostId = comment.PostId,
                            ReactState = input.ReactState,
                            CommentId = input.CommentId,
                            CreatorUserId = userId,
                            CreationTime = DateTime.UtcNow,
                        };
                        await context.PostReacts.AddAsync(react);
                    }
                    else
                    {
                        check.ReactState = input.ReactState;
                        context.PostReacts.Update(check);
                    }
                }
            }
            await context.SaveChangesAsync();
            return DataResult.ResultSuccess( "Success");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}