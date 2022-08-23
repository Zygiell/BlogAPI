namespace BlogAPI.Entities
{
    public class CommentVote
    {
        public int UserId { get; set; }
        public int CommentId { get; set; }
        public bool IsUpvoted { get; set; }
        public virtual User User { get; set; }
        public virtual Comment Comment { get; set; }
    }
}