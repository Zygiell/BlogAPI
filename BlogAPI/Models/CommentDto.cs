namespace BlogAPI.Models
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string CommentBody { get; set; }
        public int CommentRating { get; set; } = 0;
        public DateTime CommentCreationDate { get; set; } = DateTime.Now;
    }
}