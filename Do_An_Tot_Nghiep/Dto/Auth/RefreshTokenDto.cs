namespace Do_An_Tot_Nghiep.Dto.Auth;

public class RefreshTokenDto
{
    public object User { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}