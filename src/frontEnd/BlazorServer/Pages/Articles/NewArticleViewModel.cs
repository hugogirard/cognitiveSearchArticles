﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServer.Pages.Articles
{
    public class NewArticleViewModel
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]        
        public string Content { get; set; }
    }
}
