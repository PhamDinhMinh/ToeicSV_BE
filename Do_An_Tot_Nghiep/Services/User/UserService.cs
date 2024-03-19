using System.Security.Claims;
using AutoMapper;
using Do_An_Tot_Nghiep.Dto.User;
using Microsoft.EntityFrameworkCore;
using NewProject.Services.Common;

namespace Do_An_Tot_Nghiep.Services.User;

public class UserService : IUserService
{
    private readonly IDbServices _dbService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private PublicContext context = new PublicContext();

    private readonly IMapper _mapper;
    
    public UserService(IDbServices dbService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _dbService = dbService;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public Task<Models.User> Update(Models.User user)
    {
        throw new NotImplementedException();
    }
    
    public Task<Models.User> ChangePassword(UserChangePasswordDto userChangePassword)
    {
        throw new NotImplementedException();
    }

    public async Task<object> Delete(int id)
    {
        try
        {
            var user = await (from p in context.Users where (p.Id == id) select p).FirstOrDefaultAsync();
            if (_httpContextAccessor.HttpContext != null)
            {
                if (id == int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id")))
                {
                    context.Remove(user);
                    await context.SaveChangesAsync();
                    return DataResult.ResultSuccess(true, "Xoá tài khoản thành công");
                }
                else
                {
                    return DataResult.ResultFail("Bạn không có quyền thực hiện thao tác này");
                }
            }
            
            if (user == null)
            {
                return DataResult.ResultFail("Không tồn tại");
            }
            return DataResult.ResultFail("Bạn không có quyền thực hiện thao tác này"); 
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}