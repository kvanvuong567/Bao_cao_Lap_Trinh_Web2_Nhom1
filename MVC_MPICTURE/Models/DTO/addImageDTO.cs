using System.ComponentModel.DataAnnotations;

namespace MVC_MPICTURE.Models.DTO
{
    public class addImageDTO
    {
        [Required]
        [MinLength(10)]
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? Url { get; set; }
        public DateTime DateAdded { get; set; }
        public int CategoryId { get; set; }
        public List<int>? TagIds { get; set; }
    }
}
