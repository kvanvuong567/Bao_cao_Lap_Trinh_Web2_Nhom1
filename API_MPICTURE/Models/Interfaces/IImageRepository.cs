using API_MPICTURE.Models.DTO;
using API_MPICTURE.Models.Domain;
namespace API_MPICTURE.Models.Interfaces
{
    public interface IImageRepository
    {
        List<ImageWithCategoryAndTagsDTO> GetAllImages(string? filterOn = null, string? filterQuery = null, string? sortBy = null,
                                                    bool isAscending = true, int pageNumber = 1, int pageSize = 1000);
        ImageWithCategoryAndTagsDTO GetImageById(int id);
        AddImageRequestDTO AddImage(AddImageRequestDTO addImageRequestDTO);
        AddImageRequestDTO UpdateImageById(int id, AddImageRequestDTO imageDTO);
        Image? DeleteImageById(int id);
    }
}
