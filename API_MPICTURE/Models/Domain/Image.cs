using System.ComponentModel.DataAnnotations;

namespace API_MPICTURE.Models.Domain
{
    public class Image
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public DateTime DateAdded { get; set; }

        // Navigation Properties - One category has many images
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // Navigation Properties - One image has many image_tags
        public List<Image_Tag> Image_Tags { get; set; }
    }
}
