using API_MPICTURE.Models.DTO;
using API_MPICTURE.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace API_MPICTURE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagRepository _tagRepository;
        private readonly ILogger<TagsController> _logger;

        public TagsController(ITagRepository tagRepository, ILogger<TagsController> logger)
        {
            _tagRepository = tagRepository;
            _logger = logger;
        }

        [HttpGet("get-all-tags")]
        public IActionResult GetAllTags([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
                                        [FromQuery] string? sortBy, [FromQuery] bool isAscending,
                                        [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            _logger.LogInformation("GetAllTags Action method was invoked");
            var tags = _tagRepository.GetAllTags(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);

            _logger.LogInformation($"Finished GetAllTags request with data {JsonSerializer.Serialize(tags)}");

            return Ok(tags);
        }

        [HttpGet("get-tag-by-id/{id}")]
        public IActionResult GetTagById([FromRoute] int id)
        {
            var tag = _tagRepository.GetTagById(id);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }

        [HttpPost("add-tag")]
        //[Authorize(Roles = "Admin")]
        public IActionResult AddTag([FromBody] TagDTO tagDTO)
        {
            try
            {
                var addedTag = _tagRepository.AddTag(tagDTO);
                return CreatedAtAction(nameof(GetTagById), new { id = addedTag.Id }, addedTag);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update-tag-by-id/{id}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult UpdateTag(int id, [FromBody] TagDTO tagDTO)
        {
            try
            {
                var updatedTag = _tagRepository.UpdateTag(id, tagDTO);
                if (updatedTag == null)
                {
                    return NotFound();
                }
                return Ok(updatedTag);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("delete-tag-by-id/{id}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult DeleteTag(int id)
        {
            try
            {
                var deletedTag = _tagRepository.DeleteTag(id);
                if (deletedTag == null)
                {
                    return NotFound();
                }
                return Ok(deletedTag);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
