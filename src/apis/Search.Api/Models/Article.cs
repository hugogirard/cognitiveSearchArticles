using Azure.Search.Documents.Indexes;
using Azure.Search.Documents.Indexes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Search.Api.Models
{
    public class Article
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string Text { get; set; }

        public string Content { get; set; }

        public string Category { get; set; }

        public DateTimeOffset Created { get; set; }
    }
}
