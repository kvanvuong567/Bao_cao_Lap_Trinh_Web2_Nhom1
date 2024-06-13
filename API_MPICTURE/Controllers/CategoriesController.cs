using API_MPICTURE.Models.DTO;
using API_MPICTURE.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace API_MPICTURE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoriesController> _logger;

        public CategoriesController(ICategoryRepository categoryRepository, ILogger<CategoriesController> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        [HttpGet("get-all-categories")]
        public IActionResult GetAllCategories([FromQuery] string? filterOn, [FromQuery] string? filterQuery,
                                              [FromQuery] string? sortBy, [FromQuery] bool isAscending,
                                              [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 100)
        {
            _logger.LogInformation("GetAllCategories Action method was invoked");
            var categories = _categoryRepository.GetAllCategories(filterOn, filterQuery, sortBy, isAscending, pageNumber, pageSize);

            _logger.LogInformation($"Finished GetAllCategories request with data {JsonSerializer.Serialize(categories)}");

            return Ok(categories);
        }

        [HttpGet("get-category-by-id/{id}")]
        public IActionResult GetCategoryById([FromRoute] int id)
        {
            var category = _categoryRepository.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost("add-category")]
        //[Authorize(Roles = "Admin")]
        public IActionResult AddCategory([FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                var addedCategory = _categoryRepository.AddCategory(categoryDTO);
                return CreatedAtAction(nameof(GetCategoryById), new { id = addedCategory.Id }, addedCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("update-category-by-id/{id}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult UpdateCategory(int id, [FromBody] CategoryDTO categoryDTO)
        {
            try
            {
                var updatedCategory = _categoryRepository.UpdateCategory(id, categoryDTO);
                if (updatedCategory == null)
                {
                    return NotFound();
                }
                return Ok(updatedCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("delete-category-by-id/{id}")]
        //[Authorize(Roles = "Admin")]
        public IActionResult DeleteCategory(int id)
        {
            try
            {
                var deletedCategory = _categoryRepository.DeleteCategory(id);
                if (deletedCategory == null)
                {
                    return NotFound();
                }
                return Ok(deletedCategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
