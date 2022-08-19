using System.ComponentModel.DataAnnotations;

namespace BlogAPI.Models
{
    public class CreateNewCommentDto
    {
        [Required]
        [MaxLength(2000)]
        public string CommentBody { get; set; }
    }
}