using System;
using System.Collections.Generic;

namespace Article.Shared
{
    public class ArticleResult
    {
        public IEnumerable<ArticleIndex> Articles { get; set; }
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

        public long? Count { get; set; }
    }
}
