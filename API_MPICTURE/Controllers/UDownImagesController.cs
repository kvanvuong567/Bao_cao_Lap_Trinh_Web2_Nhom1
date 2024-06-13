using API_MPICTURE.Models.DTO;
using API_MPICTURE.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API_MPICTURE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UDownImagesController : ControllerBase
    {
        private readonly IUDonwImageRepository _iUDonwImageRepository;

        public UDownImagesController(IUDonwImageRepository iUDonwImageRepository)
        {
            _iUDonwImageRepository = iUDonwImageRepository;
        }

        [HttpPost]
        [Route("upload")]
        public IActionResult Upload([FromForm] ImageUploadRequestDTO request)
        {
            ValidateFileUpload(request);
            if (ModelState.IsValid)
            {
                // Convert DTO to Domain model
                var imageDomainModel = new UpDownImage
                {
                    File = request.File,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length,
                    FileName = request.File.FileName,
                    FileDescription = request.FileDescription
                };

                // Use repository to upload image
                _iUDonwImageRepository.Upload(imageDomainModel);
                return Ok(imageDomainModel);
            }
            return BadRequest(ModelState);
        }
        [HttpGet]
        public IActionResult GetAllInfoImage()
        {
            var allImage = _iUDonwImageRepository.GetAllInfoImages();
            return Ok(allImage);
        }

        [HttpGet]
        [Route("Download")]
        public IActionResult DownloadImage(int id)
        {
            var result = _iUDonwImageRepository.DownloadFile(id);
            return File(result.Item1, result.Item2, result.Item3);
        }
        private void ValidateFileUpload(ImageUploadRequestDTO request)
        {
            var allowExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported file extension");
            }

            if (request.File.Length > 1040000)
            {
                ModelState.AddModelError("file", "File size too big, please upload file <10M");
            }
        }
    }
}
