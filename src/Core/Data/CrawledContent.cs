namespace Core.Data
{
    /// <summary>
    /// The object that the WebCrawler shares with ProcessCrawler
    /// </summary>
    public class CrawledContent
    {
        /// <summary>
        /// Constructor of CrawledContent
        /// </summary>
        /// <param name="url">Url</param>
        /// <param name="content">Content</param>
        /// <param name="host">Host</param>
        /// <param name="scheme">Scheme</param>
        public CrawledContent(string url, string content, string host, string scheme)
        {
            Url = url;
            Host = host;
            Scheme = scheme;
            Content = content;
        }

        //Fields
        public string Url { get; private set; }
        public string Host { get; private set; }
        public string Scheme { get; private set; }
        public string Content { get; private set; }

        /// <summary>
        /// Return header
        /// </summary>
        public string Header
        {
            get { return string.Format("{0}://{1}", Scheme, Host); }
        }
    }
}
