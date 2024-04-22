using CloudinaryDotNet;
using Do_An_Tot_Nghiep.Helpers;
using Do_An_Tot_Nghiep.Services.Upload;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Do_An_Tot_Nghiep.Controllers;

[ApiController]
[Route("api/services/app/[controller]")]
public class UploadController : Controller
{
    private readonly Cloudinary _cloudinary;
    private readonly IUploadService _uploadService;
    
    public UploadController(IOptions<CloudinarySettings> cloudinaryConfig, IUploadService uploadService)
    {
        var account = new Account(
            cloudinaryConfig.Value.CloudName,
            cloudinaryConfig.Value.ApiKey,
            cloudinaryConfig.Value.ApiSecret);
        _cloudinary = new Cloudinary(account);
        _uploadService = uploadService;
    }
    
    [HttpPost("UploadImages")]
    public async Task<IActionResult> UploadImage(List<IFormFile> files)
    {
        var result = await _uploadService.UploadImages(files);

        return Ok(result);
    }
    
    [HttpPost("UploadFiles")]
    public async Task<IActionResult> UploadFile(List<IFormFile> files)
    {
        var result = await _uploadService.UploadFiles(files);

        return Ok(result);
    }
}