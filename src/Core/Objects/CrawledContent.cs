using System.Net;

namespace Core.Objects
{
    public class CrawledContent
    {
        public CrawledContent(string url, string content, string host, string scheme)
        {
            Url = url;
            Content = content;

            Host = host;
            Scheme = scheme;
        }

        public string Url { get; private set; }
        public string Content { get; private set; }

        public string Host { get; private set; }
        public string Scheme { get; private set; }
        public string Header
        {
            get { return string.Format("{0}://{1}", Scheme, Host); }
        }
    }
}
