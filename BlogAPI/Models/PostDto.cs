namespace BlogAPI.Models
{
    public class PostDto
    {
        public int Id { get; set; }
        public string PostTitle { get; set; }
        public string PostBody { get; set; }
        public int PostRating { get; set; } = 0;
        public DateTime PostCreationDate { get; set; } = DateTime.Now;
        public bool CanComment { get; set; } = true;
        public int PostCommentsCount { get; set; }
        public List<CommentDto> Comments { get; set; }
    }
}