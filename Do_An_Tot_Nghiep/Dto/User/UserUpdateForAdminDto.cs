namespace Do_An_Tot_Nghiep.Dto.User;

public class UserUpdateForAdminDto
{
    public int Id { get; set; }
    public string? Gender { get; set; }
    public string? EmailAddress { get; set; }
    public string? Name { get; set; }
    public string? PhoneNumber { get; set; }
    public DateTime? DateOfBirth { get; set; }
}