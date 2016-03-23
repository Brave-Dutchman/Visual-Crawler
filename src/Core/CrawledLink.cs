namespace Core
{
    public class CrawledLink
    {
        public string Link { get; set; }
        public bool Crawled { get; set; }

        private CrawledLink()
        {
            Crawled = false;
        }

        public CrawledLink(string link) : this()
        {
            Link = link;
        }
    }
}
