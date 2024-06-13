namespace API_MPICTURE.Models.DTO
{
    public class ImageWithCategoryAndTagsDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public DateTime DateAdded { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public List<TagDTO> Tags { get; set; }
    }
}
