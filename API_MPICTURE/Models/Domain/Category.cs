using System.ComponentModel.DataAnnotations;

namespace API_MPICTURE.Models.Domain
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        // Navigation Properties - One category has many images
        public List<Image> Images { get; set; }
    }
}
