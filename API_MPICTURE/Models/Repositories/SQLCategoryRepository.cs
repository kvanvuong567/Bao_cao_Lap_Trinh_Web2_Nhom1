using API_MPICTURE.Data;
using API_MPICTURE.Models.Domain;
using API_MPICTURE.Models.DTO;
using API_MPICTURE.Models.Interfaces;

namespace API_MPICTURE.Repositories
{
    public class SQLCategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public SQLCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<CategoryDTO> GetAllCategories(string filterOn, string filterQuery, string sortBy, bool isAscending, int pageNumber, int pageSize)
        {
            var query = _context.Categories.AsQueryable();

            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                if (filterOn.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(c => c.Name.Contains(filterQuery));
                }
                // Add more filter conditions here if needed
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    query = isAscending ? query.OrderBy(c => c.Name) : query.OrderByDescending(c => c.Name);
                }
                // Add more sort conditions here if needed
            }

            var categories = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name
                    // Map other properties here
                })
                .ToList();

            return categories;
        }

        public CategoryDTO GetCategoryById(int id)
        {
            var category = _context.Categories
                .Where(c => c.Id == id)
                .Select(c => new CategoryDTO
                {
                    Id = c.Id,
                    Name = c.Name
                    // Map other properties here
                })
                .FirstOrDefault();

            return category;
        }

        public CategoryDTO AddCategory(CategoryDTO categoryDTO)
        {
            var category = new Category
            {
                Name = categoryDTO.Name
                // Map other properties here
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            categoryDTO.Id = category.Id;
            return categoryDTO;
        }

        public CategoryDTO UpdateCategory(int id, CategoryDTO categoryDTO)
        {
            var category = _context.Categories.Find(id);

            if (category != null)
            {
                category.Name = categoryDTO.Name;
                // Update other properties here

                _context.SaveChanges();
            }

            return categoryDTO;
        }

        public CategoryDTO DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);

            if (category != null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();
            }

            return new CategoryDTO { Id = id };
        }
    }
}
