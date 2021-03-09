using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServer.Pages.Search
{
    public class SearchViewModel
    {
        [Required(ErrorMessage = "Please enter a search query")]        
        public string SearchQuery { get; set; }
    }
}
