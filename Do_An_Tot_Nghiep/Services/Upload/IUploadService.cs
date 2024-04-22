namespace Do_An_Tot_Nghiep.Services.Upload;

public interface IUploadService
{
    Task<object> UploadImages(List<IFormFile> files);
    Task<object> UploadFiles(List<IFormFile> files);
}