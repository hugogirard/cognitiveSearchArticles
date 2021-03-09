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
        [SimpleField(IsKey = true)]
        public string Id { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string Title { get; set; }

        [SearchableField(AnalyzerName = LexicalAnalyzerName.Values.EnLucene)]
        public string Text { get; set; }

        [SearchableField(IsFilterable = true)]
        public DateTimeOffset Created { get; set; }
    }
}
