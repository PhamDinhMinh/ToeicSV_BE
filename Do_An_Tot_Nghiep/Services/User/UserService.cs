using System.Net;
using System.Security.Claims;
using AutoMapper;
using Do_An_Tot_Nghiep.Dto.Response;
using Do_An_Tot_Nghiep.Dto.User;
using Do_An_Tot_Nghiep.Services.Auth;
using Microsoft.EntityFrameworkCore;
using NewProject.Services.Common;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Do_An_Tot_Nghiep.Services.User;

public class UserService : IUserService
{
    private readonly IDbServices _dbService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private PublicContext context = new PublicContext();
    private readonly IAuthService _authService;
    private readonly IMapper _mapper;

    public UserService(IDbServices dbService, IMapper mapper, IHttpContextAccessor httpContextAccessor,
        IAuthService authService)
    {
        _dbService = dbService;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _authService = authService;
    }

    public async Task<object> Update(UserUpdateDto input)
    {
        try
        {
            if (_httpContextAccessor.HttpContext == null)
            {
                return DataResult.ResultFail("Không xác định được người dùng", (int)HttpStatusCode.Unauthorized);
            }

            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id"));
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return DataResult.ResultFail("Không tìm thấy người dùng", (int)HttpStatusCode.NotFound);
            }
            context.Entry(user).CurrentValues.SetValues(input);
            await context.SaveChangesAsync();
            return DataResult.ResultSuccess(user, "Update thành công");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<object> ChangePassword(UserChangePasswordDto input)
    {
        if (_httpContextAccessor.HttpContext == null)
        {
            return DataResult.ResultFail("Không xác định được người dùng", (int)HttpStatusCode.Unauthorized);
        }

        var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id"));
        var user = await (from p in context.Users where (p.Id == userId) select p).FirstOrDefaultAsync();
        if (user == null)
        {
            return DataResult.ResultFail("Không tìm thấy người dùng", (int)HttpStatusCode.NotFound);
        }

        var passwordVerified = await _authService.VerifyPassword(input.CurrentPassword, user.Password);
        if (!passwordVerified)
        {
            return DataResult.ResultFail("Mật khẩu hiện tại không chính xác", (int)HttpStatusCode.BadRequest);
        }

        user.Password = await _authService.HashPassword(input.NewPassword);
        context.Users.Update(user);
        await context.SaveChangesAsync();

        return DataResult.ResultSuccess(true, "Thay đổi mật khẩu thành công", (int)HttpStatusCode.OK);
    }

    public async Task<object> Delete(int id)
    {
        try
        {
            var user = await (from p in context.Users where (p.Id == id) select p).FirstOrDefaultAsync();
            if (user == null)
            {
                return DataResult.ResultFail("Không tìm thấy người dùng", (int)HttpStatusCode.NotFound);
            }

            if (_httpContextAccessor.HttpContext != null)
            {
                if (id == int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id")))
                {
                    context.Remove(user);
                    await context.SaveChangesAsync();
                    return DataResult.ResultSuccess(true, "Xoá tài khoản thành công", (int)HttpStatusCode.OK);
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