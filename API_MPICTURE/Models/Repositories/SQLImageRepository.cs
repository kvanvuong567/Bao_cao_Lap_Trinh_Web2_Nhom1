using API_MPICTURE.Data;
using API_MPICTURE.Models.Domain;
using API_MPICTURE.Models.DTO;
using API_MPICTURE.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace API_MPICTURE.Repositories
{
    public class SQLImageRepository : IImageRepository
    {
        private readonly AppDbContext _context;

        public SQLImageRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<ImageWithCategoryAndTagsDTO> GetAllImages(string? filterOn = null, string? filterQuery = null,
                                                      string? sortBy = null, bool isAscending = true,
                                                      int pageNumber = 1, int pageSize = 1000)
        {
            IQueryable<Image> query = _context.Images
                .Include(i => i.Category)
                .Include(i => i.Image_Tags)
                    .ThenInclude(it => it.Tag);

            // Filtering
            if (!string.IsNullOrWhiteSpace(filterOn) && !string.IsNullOrWhiteSpace(filterQuery))
            {
                if (filterOn.Equals("name", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(i => i.Title.Contains(filterQuery));
                }

                if (filterOn.Equals("description", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(i => i.Description.Contains(filterQuery));
                }
            }

            // Sorting
            if (!string.IsNullOrWhiteSpace(sortBy))
            {
                switch (sortBy.ToLower())
                {
                    case "name":
                        query = isAscending ? query.OrderBy(i => i.Title) : query.OrderByDescending(i => i.Title);
                        break;
                    case "dateadded":
                        query = isAscending ? query.OrderBy(i => i.DateAdded) : query.OrderByDescending(i => i.DateAdded);
                        break;
                    default:
                        query = isAscending ? query.OrderBy(i => i.Id) : query.OrderByDescending(i => i.Id);
                        break;
                }
            }

            // Pagination
            var images = query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(i => new ImageWithCategoryAndTagsDTO
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    Url = i.URL,
                    DateAdded = i.DateAdded,
                    CategoryId = i.CategoryId,
                    CategoryName = i.Category.Name,
                    Tags = i.Image_Tags.Select(it => new TagDTO
                    {
                        Id = it.TagId,
                        Name = it.Tag.Name
                    }).ToList()
                }).ToList();

            return images;
        }


        public ImageWithCategoryAndTagsDTO GetImageById(int id)
        {
            var image = _context.Images
                .Include(i => i.Category)
                .Include(i => i.Image_Tags)
                    .ThenInclude(it => it.Tag)
                .Where(i => i.Id == id)
                .Select(i => new ImageWithCategoryAndTagsDTO
                {
                    Id = i.Id,
                    Title = i.Title,
                    Description = i.Description,
                    Url = i.URL,
                    DateAdded = i.DateAdded,
                    CategoryId = i.CategoryId,
                    CategoryName = i.Category.Name,
                    Tags = i.Image_Tags.Select(it => new TagDTO
                    {
                        Id = it.TagId,
                        Name = it.Tag.Name
                    }).ToList()
                }).FirstOrDefault();

            return image;
        }

        public AddImageRequestDTO AddImage(AddImageRequestDTO addImageRequestDTO)
        {
            var image = new Image
            {
                Title = addImageRequestDTO.Title,
                Description = addImageRequestDTO.Description,
                URL = addImageRequestDTO.Url,
                DateAdded = addImageRequestDTO.DateAdded,
                CategoryId = addImageRequestDTO.CategoryId
            };

            if (addImageRequestDTO.TagIds != null)
            {
                image.Image_Tags = addImageRequestDTO.TagIds.Select(tagId => new Image_Tag { TagId = tagId }).ToList();
            }
            _context.Images.Add(image);
            _context.SaveChanges();
            return addImageRequestDTO;
        }


        public AddImageRequestDTO UpdateImageById(int id, AddImageRequestDTO imageDTO)
        {
            var image = _context.Images
                .Include(i => i.Image_Tags)
                .FirstOrDefault(i => i.Id == id);

            if (image != null)
            {
                image.Title = imageDTO.Title;
                image.Description = imageDTO.Description;
                image.URL = imageDTO.Url;
                image.DateAdded = imageDTO.DateAdded;
                image.CategoryId = imageDTO.CategoryId;

                // Update tags
                image.Image_Tags.Clear();
                if (imageDTO.TagIds != null)
                {
                    foreach (var tagId in imageDTO.TagIds)
                    {
                        image.Image_Tags.Add(new Image_Tag { ImageId = image.Id, TagId = tagId });
                    }
                }

                _context.SaveChanges();
            }

            return imageDTO;
        }

        public Image? DeleteImageById(int id)
        {
            var image = _context.Images.Find(id);
            if (image != null)
            {
                _context.Images.Remove(image);
                _context.SaveChanges();
            }

            return image;
        }
    }
}
