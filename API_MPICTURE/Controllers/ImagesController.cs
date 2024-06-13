using API_MPICTURE.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using API_MPICTURE.Models.Interfaces;
using API_MPICTURE.Models.Validate;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace API_MPICTURE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;
        private readonly ILogger<ImagesController> _logger;

        public ImagesController(IImageRepository imageRepository, ILogger<ImagesController> logger)
        {
            _imageRepository = imageRepository;
            _logger = logger;
        }

        [HttpGet("get-all-images")]
        //[Authorize(Roles = "User,Admin")]
        public IActionResult GetAllImages([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
                                  [FromQuery] string? sortBy, [FromQuery] bool isAscending,
                                  [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            _logger.LogInformation("GetAll Image Action method was invoked");
            _logger.LogWarning("This is a warning log");
            _logger.LogError("This is a error log");
            var allImages = _imageRepository.GetAllImages(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);

            //debug
            _logger.LogInformation($"Finished GetAllImage request with data{ JsonSerializer.Serialize(allImages)}"); 

            return Ok(allImages);
        }

        [HttpGet("get-image-by-id/{id}")]
        //[Authorize(Roles = "User,Admin")]
        public IActionResult GetImageById([FromRoute] int id)
        {
            var imageWithIdDTO = _imageRepository.GetImageById(id);
            return Ok(imageWithIdDTO);
        }

        [HttpPost("add-image")]
        //[Authorize(Roles = "Admin")]
        [ValidateModel]
        public IActionResult AddImage([FromBody] AddImageRequestDTO addImageRequestDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imageAdd = _imageRepository.AddImage(addImageRequestDTO);
            return Ok(imageAdd);
        }

        [HttpPut("update-image-by-id/{id}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult UpdateImageById(int id, [FromBody] AddImageRequestDTO imageDTO)
        {
            var updateImage = _imageRepository.UpdateImageById(id, imageDTO);
            return Ok(updateImage);
        }

        [HttpDelete("delete-image-by-id/{id}")]
        //[Authorize(Roles = "Admin")]

        public IActionResult DeleteImageById(int id)
        {
            var deleteImage = _imageRepository.DeleteImageById(id);
            return Ok(deleteImage);
        }

        #region Private methods
        private bool ValidateAddImage(AddImageRequestDTO addImageRequestDTO)
        {
            if (addImageRequestDTO == null)
            {
                ModelState.AddModelError(nameof(addImageRequestDTO), $"Please add image data");
                return false;
            }
            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
