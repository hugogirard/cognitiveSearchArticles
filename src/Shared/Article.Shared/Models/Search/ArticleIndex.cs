using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Article.Shared
{
    public class ArticleIndex
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Category { get; set; }

        public string ShortDescription { get; set; }

        public string Content { get; set; }

        public string Text { get; set; }

        public DateTimeOffset Created { get; set; }

        public string AttachmentUri { get; set; }

        public string Metadata_storage_path { get; set; }
        
        public string FilePath => Encoding.UTF8.GetString(Convert.FromBase64String(this.Metadata_storage_path));
    }
}
