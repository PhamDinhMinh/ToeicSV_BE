using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Do_An_Tot_Nghiep.Dto.Auth;
using Microsoft.EntityFrameworkCore;
using NewProject.Services.Common;

namespace Do_An_Tot_Nghiep.Services.Auth;

public class AuthService : IAuthService
{
    private readonly IDbServices _dbService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private PublicContext context = new PublicContext();

    private readonly IMapper _mapper;

    public AuthService(IDbServices dbService, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _dbService = dbService;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Models.User> Login(UserLoginDto input)
    {
        try
        {
            var user = await context.Users.FirstOrDefaultAsync(u =>
                u.UserName == input.UserNameOrEmail || u.EmailAddress == input.UserNameOrEmail);
            if (user == null)
            {
                return null;
            }

            var passwordVerified = await VerifyPassword(input.Password, user.Password);
            if (!passwordVerified)
            {
                return null;
            }

            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Models.User> Register(UserRegisterDto input)
    {
        try
        {
            UserRegisterDto inputHashPass = new UserRegisterDto();
            inputHashPass = input;
            inputHashPass.Password = await HashPassword(input.Password);
            var newRegisterUser = _mapper.Map<Models.User>(inputHashPass);
            newRegisterUser.CreationTime = DateTime.Now;
            context.Users.Add(newRegisterUser);
            await context.SaveChangesAsync();
            return newRegisterUser;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Object> GetUserInfo()
    {
        if (_httpContextAccessor.HttpContext != null)
        {
            var user = _httpContextAccessor.HttpContext.User;
            var userInfo = await (from p in context.Users where (p.Id == int.Parse(user.FindFirstValue("Id"))) select p).FirstOrDefaultAsync();
            return await Task.FromResult(userInfo); 
        }

        return DataResult.ResultFail("Không tìm thấy người dùng", (int)HttpStatusCode.NotFound);;
    }

    public async Task<string> HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            using (var stream = new MemoryStream(passwordBytes))
            {
                byte[] passwordHash = await sha256.ComputeHashAsync(stream);
                string hashedPassword = BitConverter.ToString(passwordHash).Replace("-", "").ToLower();
                return hashedPassword;
            }
        }
    }

    public async Task<bool> VerifyPassword(string password, string storedPasswordHash)
    {
        string inputHashedPassword = await HashPassword(password);
        return inputHashedPassword == storedPasswordHash;
    }

    public async Task<bool> GetByUserName(string userName)
    {
        var userExists =
            await context.Users.AnyAsync(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
        return userExists;
    }
}