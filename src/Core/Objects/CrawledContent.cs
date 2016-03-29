using System.Net;

namespace Core.Objects
{
    public class CrawledContent
    {
        public CrawledContent(string url, string content, WebResponse webResponce)
        {
            Url = url;
            Content = content;

            Host = webResponce.ResponseUri.Host;
            Scheme = webResponce.ResponseUri.Scheme;
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
