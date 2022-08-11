using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
