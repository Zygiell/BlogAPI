using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogAPI.Models
{
    public class CreateNewCommentDto
    {
        [Required]
        [MaxLength(2000)]
        public string CommentBody { get; set; }       


    }
}
