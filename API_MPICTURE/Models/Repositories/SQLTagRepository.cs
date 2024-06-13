using API_MPICTURE.Data;
using API_MPICTURE.Models.Domain;
using API_MPICTURE.Models.DTO;
using API_MPICTURE.Models.Interfaces;


namespace API_MPICTURE.Repositories
{
    public class SQLTagRepository : ITagRepository
    {
        private readonly AppDbContext _context;

        public SQLTagRepository(AppDbContext context)
        {
            _context = context;
        }

        public IEnumerable<TagDTO> GetAllTags(string filterOn, string filterQuery, string sortBy, bool isAscending, int pageNumber, int pageSize)
        {
            var query = _context.Tags.AsQueryable();

            if (!string.IsNullOrEmpty(filterOn) && !string.IsNullOrEmpty(filterQuery))
            {
                if (filterOn.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(t => t.Name.Contains(filterQuery));
                }
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortBy.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    query = isAscending ? query.OrderBy(t => t.Name) : query.OrderByDescending(t => t.Name);
                }
            }

            var tags = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new TagDTO
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .ToList();

            return tags;
        }

        public TagDTO GetTagById(int id)
        {
            var tag = _context.Tags
                .Where(t => t.Id == id)
                .Select(t => new TagDTO
                {
                    Id = t.Id,
                    Name = t.Name
                })
                .FirstOrDefault();

            return tag;
        }

        public TagDTO AddTag(TagDTO tagDTO)
        {
            var tag = new Tag
            {
                Name = tagDTO.Name
            };

            _context.Tags.Add(tag);
            _context.SaveChanges();

            tagDTO.Id = tag.Id;
            return tagDTO;
        }

        public TagDTO UpdateTag(int id, TagDTO tagDTO)
        {
            var tag = _context.Tags.Find(id);

            if (tag != null)
            {
                tag.Name = tagDTO.Name;

                _context.SaveChanges();
            }

            return tagDTO;
        }

        public TagDTO DeleteTag(int id)
        {
            var tag = _context.Tags.Find(id);

            if (tag != null)
            {
                _context.Tags.Remove(tag);
                _context.SaveChanges();
            }

            return new TagDTO { Id = id };
        }
    }
}
