namespace Core.Data
{
    /// <summary>
    /// The object that the WebCrawler shares with ProcessCrawler
    /// </summary>
    public class CrawledContent
    {
        public CrawledContent(string url, string content, string host, string scheme)
        {
            Url = url;
            Host = host;
            Scheme = scheme;
            Content = content;
        }

        public string Url { get; private set; }
        public string Host { get; private set; }
        public string Scheme { get; private set; }
        public string Content { get; private set; }

        public string Header
        {
            get { return string.Format("{0}://{1}", Scheme, Host); }
        }
    }
}
