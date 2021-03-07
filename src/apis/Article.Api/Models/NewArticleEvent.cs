using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Article.Api.Models
{
    public class NewArticleEvent
    {
        public string Id { get;  }

        public string EventType { get; }

        public string Subject { get; }

        public DateTime EventTime { get; }

        public string Data { get; }

        public string DataVersion { get; }

        public NewArticleEvent(int articleId,string title)
        {
            Id = new Guid().ToString();
            EventType = "newArticle";
            Subject = title;
            EventTime = DateTime.UtcNow;
            Data = articleId.ToString();
            DataVersion = "1.0";
        }
    }
}
