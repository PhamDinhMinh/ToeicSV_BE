namespace Do_An_Tot_Nghiep.Dto.User;

public class RefreshTokenDto
{
    public object User { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}