using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Do_An_Tot_Nghiep.Helpers;
using Microsoft.Extensions.Options;

namespace Do_An_Tot_Nghiep.Services.Upload;

public class UploadService : IUploadService
{
    private readonly Cloudinary _cloudinary;

    public UploadService(IOptions<CloudinarySettings> cloudinaryConfig)
    {
        var account = new Account(
            cloudinaryConfig.Value.CloudName,
            cloudinaryConfig.Value.ApiKey,
            cloudinaryConfig.Value.ApiSecret);
        _cloudinary = new Cloudinary(account);
    }
    public async Task<object> UploadImages(List<IFormFile> files)
    {
        var urls = new List<string>();

        foreach (var file in files)
        {
            if (file.Length > 0)
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    Folder = "images" 
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error == null)
                {
                    urls.Add(uploadResult.SecureUrl.ToString());
                }
            }
        }

        return urls;
    }

    public async Task<object> UploadFiles(List<IFormFile> files)
    {
        var urls = new List<string>();

        foreach (var file in files)
        {
            if (file.Length > 0)
            {
                var uploadParams = new RawUploadParams()
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream()),
                    Folder = "files" 
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.Error == null)
                {
                    urls.Add(uploadResult.SecureUrl.ToString());
                }
            }
        }

        return urls;
    }
}