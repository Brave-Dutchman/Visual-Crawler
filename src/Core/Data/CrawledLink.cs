using Newtonsoft.Json;

namespace Core.Data
{
    /// <summary>
    /// The crawledlink used by the webcrawler to get new links to crawl and ensure that no link is crawled twice
    /// </summary>
    public class CrawledLink
    {
        public string Link { get; set; }

        public bool IsCrawled { get; set; }

        [JsonConstructor]
        public CrawledLink() { }

        public CrawledLink(string link)
        {
            Link = link;
            IsCrawled = false;
        }

        public CrawledLink(string link, bool isCrawled)
        {
            Link = link;
            IsCrawled = isCrawled;
        }

        public override string ToString()
        {
            return Link;
        }
    }
}
