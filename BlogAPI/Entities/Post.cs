using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Entities
{
    public class Post
    {
        
        public int Id { get; set; }
        public int? CreatedByUserId { get; set; }
        public virtual User CreatedByUser { get; set; }
        public string PostTitle { get; set; }
        public string PostBody { get; set; }
        public int PostRating { get; set; } = 0;
        public DateTime PostCreationDate { get; set; } = DateTime.Now;
        public bool CanComment { get; set; } = true;
        public virtual List<Comment> Comments { get; set; }
    }
}
