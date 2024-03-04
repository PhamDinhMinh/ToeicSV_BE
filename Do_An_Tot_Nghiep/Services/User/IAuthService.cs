using Do_An_Tot_Nghiep.Dto.User;

namespace Do_An_Tot_Nghiep.Services.User;

public interface IAuthService
{
    Task<Models.User> Register(UserRegisterDto userRegister);

    Task<string> HashPassword(string password);
}