using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Objects;

namespace Backgroud_Crawler.Crawling
{
    public class ToDbStorage
    {
        private const int MAX = 1000;
        private static readonly List<Link> LINKS;
        private static readonly List<CrawledLink> CRAWLED_LINKS;

        static ToDbStorage()
        {
            LINKS = new List<Link>();
            CRAWLED_LINKS = new List<CrawledLink>();
        }

        public static void Add(List<CrawledLink> crawledLinks)
        {
            List<CrawledLink> links;

            lock (CRAWLED_LINKS)
            {
                CRAWLED_LINKS.AddRange(crawledLinks);

                if (CRAWLED_LINKS.Count <= MAX) return;

                Console.WriteLine("Wrote CrawledLinks to database");
                links = CRAWLED_LINKS.ToList();
                CRAWLED_LINKS.Clear();
            }

            Storage.WriteLinks(links);

        }

        public static void Add(List<Link> links)
        {
            List<Link> theLinks;

            lock (LINKS)
            {
                LINKS.AddRange(links);

                if (LINKS.Count <= MAX) return;

                Console.WriteLine("Wrote Links to database");
                theLinks = LINKS.ToList();
                LINKS.Clear();
            }

            Storage.WriteLinks(theLinks);
        }
    }
}
