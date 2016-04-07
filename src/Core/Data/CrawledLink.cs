using Newtonsoft.Json;

namespace Core.Data
{
    /// <summary>
    /// The CrawledLink used by the webcrawler to get new links to crawl and ensure that no link is crawled twice
    /// </summary>
    public class CrawledLink
    {
        //Fields
        public string Link { get; set; }

        public bool IsCrawled { get; set; }

        /// <summary>
        /// Constructor of CrawledLink
        /// </summary>
        [JsonConstructor]
        public CrawledLink() { }

        /// <summary>
        /// Constructor of CrawLink
        /// </summary>
        /// <param name="link">Link (URL) to store</param>
        public CrawledLink(string link)
        {
            Link = link;
            IsCrawled = false;
        }

        /// <summary>
        /// Constructor of CrawLink
        /// </summary>
        /// <param name="link">Link (URL) to store</param>
        /// <param name="isCrawled">Boolean value: Has link been crawled true/false</param>
        public CrawledLink(string link, bool isCrawled)
        {
            Link = link;
            IsCrawled = isCrawled;
        }

        /// <summary>
        /// Return link
        /// </summary>
        /// <returns>Link (URL) string</returns>
        public override string ToString()
        {
            return Link;
        }
    }
}
