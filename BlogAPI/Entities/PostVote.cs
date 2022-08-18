using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
