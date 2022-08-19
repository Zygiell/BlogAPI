using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models
{
    public class CreateNewPostDto
    {
        [Required]
        [MaxLength(300)]
        public string PostTitle { get; set; }

        public string PostBody { get; set; }
        public bool CanComment { get; set; }
    }
}