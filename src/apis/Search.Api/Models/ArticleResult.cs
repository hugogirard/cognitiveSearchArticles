using Azure.Search.Documents.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Search.Api.Models
{
    public class ArticleResult
    {
        public IEnumerable<Article> Articles { get; set; }
        public KeyValuePair<string, IList<FacetResult>> Facets { get; internal set; }

        //public IDictionary<string,string> Facets { get; set; }
    }
}
