using Do_An_Tot_Nghiep.Dto.User;

namespace Do_An_Tot_Nghiep.Services.User;

public interface IUserService
{
    Task<Models.User> Update(Models.User user);
    Task<Models.User> ChangePassword(UserChangePasswordDto userChangePassword);
    Task<object> Delete(int id);
}