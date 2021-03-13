using Azure.Search.Documents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorServer.Models
{
    public class ArticleResult
    {
        public IEnumerable<Article> Articles { get; set; }
        public Facet Facets { get; set; }

        public ArticleResult()
        {
            Facets = new Facet();
        }

    }

    public class Facet
    {
        public string Key { get; set; }

        public IEnumerable<KeyValue> Value { get; set; }
    }

    public class KeyValue 
    {
        public string Value { get; set; }
    }
}
