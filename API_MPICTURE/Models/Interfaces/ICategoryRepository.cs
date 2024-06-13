using API_MPICTURE.Models.DTO;

namespace API_MPICTURE.Models.Interfaces
{
    public interface ICategoryRepository
    {
        IEnumerable<CategoryDTO> GetAllCategories(string filterOn, string filterQuery, string sortBy, bool isAscending, int pageNumber, int pageSize);
        CategoryDTO GetCategoryById(int id);
        CategoryDTO AddCategory(CategoryDTO categoryDTO);
        CategoryDTO UpdateCategory(int id, CategoryDTO categoryDTO);
        CategoryDTO DeleteCategory(int id);
    }
}
