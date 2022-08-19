namespace BlogAPI.Entities
{
    public class PostVote
    {
        public int UserId { get; set; }
        public int PostId { get; set; }
        public bool IsPostUpVotedByUser { get; set; }
        public virtual User User { get; set; }
        public virtual Post Post { get; set; }
    }
}