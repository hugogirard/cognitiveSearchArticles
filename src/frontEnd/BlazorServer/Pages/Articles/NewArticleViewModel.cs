using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServer.Pages.Articles
{
    public class NewArticleViewModel
    {
        public string Id { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        [MaxLength(200)]
        public string ShortDescription { get; set; }

        public string Content { get; set; }

        public string FileUri { get; set; }

        public string Filename { get; set; }
    }
}
