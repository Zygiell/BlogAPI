using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
