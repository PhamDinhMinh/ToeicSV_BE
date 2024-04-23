using Do_An_Tot_Nghiep.Dto.User;

namespace Do_An_Tot_Nghiep.Services.User;

public interface IUserService
{
    Task<object> GetUserById(int id);
    Task<object> Update(UserUpdateDto user);
    Task<object> UpdateAvatar(AvatarUpdateDto avatarUrl);
    Task<object> ChangePassword(UserChangePasswordDto userChangePassword);
    Task<object> Delete(int id);
}