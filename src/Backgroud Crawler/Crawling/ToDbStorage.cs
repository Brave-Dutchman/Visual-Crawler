using System.Collections.Generic;
using Core;
using Core.Objects;

namespace Backgroud_Crawler.Crawling
{
    public static class ToDbStorage
    {
        private static readonly List<Link> LINKS;
        private static readonly List<CrawledLink> CRAWLED_LINKS;
        private static readonly List<string> UPDATED;

        static ToDbStorage()
        {
            CRAWLED_LINKS = new List<CrawledLink>();
            LINKS = new List<Link>();
            UPDATED = new List<string>();
        }

        public static void Write()
        {
            lock (UPDATED)
            {
                lock (CRAWLED_LINKS)
                {
                    lock (LINKS)
                    {
                        Storage.UpdateCrawledLinks(UPDATED);
                        Storage.WriteLinks(LINKS);
                        Storage.WriteLinks(CRAWLED_LINKS);

                        CRAWLED_LINKS.Clear();
                        LINKS.Clear();
                        UPDATED.Clear();
                    }
                }
            }
        }

        public static void Add(List<CrawledLink> crawledLinks, string updated)
        {
            lock (updated)
            {
                UPDATED.Add(updated);
            }

            lock (CRAWLED_LINKS)
            {
                foreach (CrawledLink crawledLink in crawledLinks)
                {
                    if (!CRAWLED_LINKS.ContainsCrawled(crawledLink.Link))
                    {
                        CRAWLED_LINKS.Add(crawledLink);
                    }
                }
            }
        }

        public static void Add(List<Link> links)
        {
            lock (CRAWLED_LINKS)
            {
                foreach (Link link in links)
                {
                    LINKS.Add(link);
                }
            }
        }
    }
}
