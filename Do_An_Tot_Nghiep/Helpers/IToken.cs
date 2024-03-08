using Do_An_Tot_Nghiep.Models;

namespace Do_An_Tot_Nghiep.Helpers;

public interface IToken
{
    public string CreateToken(User user);
    public string  GenerateRefreshToken();

}