using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using Do_An_Tot_Nghiep.Dto.User;

namespace Do_An_Tot_Nghiep.Services.User;

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

    public async Task<Models.User> Register(UserRegisterDto input)
    {
        try
        {
            UserRegisterDto inputHashPass = new UserRegisterDto();
            inputHashPass = input;
            inputHashPass.Password = await HashPassword(input.Password);
            var newRegisterUser = _mapper.Map<Models.User>(inputHashPass);
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
    
    public async Task<string> HashPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

            // Tạo một MemoryStream từ mảng byte
            using (var stream = new MemoryStream(passwordBytes))
            {
                // Băm mật khẩu sử dụng SHA-256
                byte[] passwordHash = await sha256.ComputeHashAsync(stream);

                // Chuyển đổi mảng byte thành chuỗi hex
                string hashedPassword = BitConverter.ToString(passwordHash).Replace("-", "").ToLower();

                return hashedPassword;
            }
        }
    }
}