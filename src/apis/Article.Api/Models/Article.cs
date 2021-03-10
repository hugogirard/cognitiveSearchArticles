using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Article.Api.Models
{
    public class Article
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(100)]
        [Required]
        public string Title { get; set; }

        [MaxLength(200)]
        [Required]
        public string ShortDescription { get; set; }

        [MaxLength(4000)]
        [Required]
        public string Text { get; set; }
        
        public DateTime Created { get; set; }

        public string AttachmentUri { get; set; }

        public Article()
        {
            Created = DateTime.UtcNow;
        }
    }
}
