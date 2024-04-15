using System.Net;
using System.Security.Claims;
using AutoMapper;
using Do_An_Tot_Nghiep.Dto.PostComment;
using Microsoft.EntityFrameworkCore;
using NewProject.Services.Common;

namespace Do_An_Tot_Nghiep.Services.PostComment;

public class PostCommentService : IPostCommentService
{
    private readonly IDbServices _dbService;
    private PublicContext context = new PublicContext();
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMapper _mapper;

    public PostCommentService(IDbServices dbService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _dbService = dbService;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<object> GetAll(GetPostCommentDto parameters)
    {
        try
        {
            var query = (from postComment in context.PostComments
                    join user in context.Users
                        on postComment.CreatorUserId equals user.Id
                    select new
                    {
                        Id = postComment.Id,
                        Comment = postComment.Comment,
                        ParentCommentId = postComment.ParentCommentId,
                        PostId = postComment.PostId,
                        CreatorUserId = postComment.CreatorUserId,
                        CountChildComment =
                            context.PostComments.Count(child => child.ParentCommentId == postComment.Id),
                        CountReact = (from react in context.PostReacts
                            where react.CommentId == postComment.Id &&  react.ReactState != null
                            select react).AsQueryable().Count(),
                        UserReact = context.PostReacts
                            .Where(x => x.CommentId == postComment.Id &&
                                        x.CreatorUserId ==
                                        int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id")))
                            .Select(react => react.ReactState).FirstOrDefault(),
                        User = new
                        {
                            Id = user.Id,
                            Name = user.Name,
                            CoverImageUrl = user.CoverImageUrl,
                            ImageUrl = user.ImageUrl,
                            CreationTime = user.CreationTime
                        },
                        CreationTime = postComment.CreationTime
                    })
                .Where(x => x.PostId == parameters.PostId);

            if (!string.IsNullOrEmpty(parameters.Keyword))
            {
                query = query.Where(x => x.Comment.Contains(parameters.Keyword));
            }

            if (parameters.ParentCommentId.HasValue)
            {
                query = query.Where(x => x.ParentCommentId == parameters.ParentCommentId);
            }
            else
            {
                query = query.Where(x => x.ParentCommentId == null);
            }

            query = query.OrderByDescending(x => x.CreationTime);
            var result = query.Skip(parameters.SkipCount).Take(parameters.MaxResultCount).ToList();
            return DataResult.ResultSuccess(result, "", query.Count());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<object> Create(CreatePostCommentDto input)
    {
        try
        {
            var request = _mapper.Map<Models.PostComment>(input);
            var post = await context.Posts.FirstOrDefaultAsync(p => p.Id == request.PostId);
            if (post == null) throw new Exception("Post not found");
            if (request.ParentCommentId != 0 && request.ParentCommentId != null)
            {
                var parentComment =
                    context.PostComments.FirstOrDefaultAsync(p =>
                        p.Id == request.ParentCommentId && p.PostId == request.PostId);
                if (parentComment == null) throw new Exception("Parent comment not found");
            }
            request.CreatorUserId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id"));
            request.CreationTime = DateTime.Now;
            await context.PostComments.AddAsync(request);
            await context.SaveChangesAsync();
            return DataResult.ResultSuccess(request, "Bình luận thành công");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<object> Update(UpdatePostCommentDto input)
    {
        try
        {
            var comment = await context.PostComments.FindAsync(input.Id);
            if (comment == null)
            {
                return DataResult.ResultFail("Không tìm thấy comment này!");
            }

            if (_httpContextAccessor.HttpContext != null)
            {
                if (comment.CreatorUserId == int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id")))
                {
                    comment.Comment = input.Comment;
                    context.PostComments.Update(comment);
                    await context.SaveChangesAsync();
                    return DataResult.ResultSuccess(comment, "Update thành công");
                }
                else
                {
                    return DataResult.ResultFail("Bạn không có quyền thực hiện thao tác này",
                        (int)HttpStatusCode.Forbidden);
                }
            }

            return DataResult.ResultFail("Bạn không có quyền thực hiện thao tác này",
                (int)HttpStatusCode.Forbidden);
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
            var comment = await (from p in context.PostComments where (p.Id == id) select p).FirstOrDefaultAsync();
            if (comment == null)
            {
                return DataResult.ResultFail("Không tồn tại bình luận này");
            }

            if (_httpContextAccessor.HttpContext != null)
            {
                if (comment.CreatorUserId == int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id")))
                {
                    context.PostComments.Remove(comment);
                    await context.SaveChangesAsync();
                    return DataResult.ResultSuccess(true, "");
                }
                else
                {
                    return DataResult.ResultFail("Bạn không có quyền thực hiện thao tác này",
                        (int)HttpStatusCode.Forbidden);
                }
            }

            return DataResult.ResultFail("Bạn không có quyền thực hiện thao tác này",
                (int)HttpStatusCode.Forbidden);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}