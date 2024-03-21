namespace Do_An_Tot_Nghiep.Dto.Response;

public class ResponseDto
{
    public object? Data { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; }
    public int? TotalRecord { get; set; } = 0;
}