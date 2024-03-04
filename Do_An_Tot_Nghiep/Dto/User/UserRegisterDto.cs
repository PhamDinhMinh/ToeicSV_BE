namespace Do_An_Tot_Nghiep.Dto.User;

public class UserRegisterDto
{
    public string UserName { get; set; }
    public string Password { get; set; }
    public string Role { get; set; } = "User";
    public string? Gender { get; set; }
    public string EmailAddress { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
}