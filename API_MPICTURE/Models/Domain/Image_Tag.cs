using System.ComponentModel.DataAnnotations;

namespace API_MPICTURE.Models.Domain
{
    public class Image_Tag
    {
        [Key]
        public int Id { get; set; }

        // Navigation Properties - One image has many image_tags
        public int ImageId { get; set; }
        public Image Image { get; set; }

        // Navigation Properties - One tag has many image_tags
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
