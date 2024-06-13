using System.ComponentModel.DataAnnotations;

namespace API_MPICTURE.Models.Domain
{
    public class Tag
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation Properties - One tag has many image_tags
        public List<Image_Tag> Image_Tags { get; set; }
    }
}
