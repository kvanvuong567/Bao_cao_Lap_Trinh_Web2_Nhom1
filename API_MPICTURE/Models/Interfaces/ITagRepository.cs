using API_MPICTURE.Models.DTO;

namespace API_MPICTURE.Models.Interfaces
{
    public interface ITagRepository
    {
        IEnumerable<TagDTO> GetAllTags(string filterOn, string filterQuery, string sortBy, bool isAscending, int pageNumber, int pageSize);
        TagDTO GetTagById(int id);
        TagDTO AddTag(TagDTO tagDTO);
        TagDTO UpdateTag(int id, TagDTO tagDTO);
        TagDTO DeleteTag(int id);
    }
}
