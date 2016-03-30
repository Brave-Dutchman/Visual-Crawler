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

                
                links = CRAWLED_LINKS.ToList();
                CRAWLED_LINKS.Clear();
            }

            Console.WriteLine("Started writing CrawledLinks to database");
            Storage.WriteLinks(links);
            Console.WriteLine("Wrote CrawledLinks to database");
        }

        public static void Add(List<Link> links)
        {
            List<Link> theLinks;

            lock (LINKS)
            {
                LINKS.AddRange(links);

                if (LINKS.Count <= MAX) return;

                theLinks = LINKS.ToList();
                LINKS.Clear();
            }

            Console.WriteLine("Started writing Links to database");
            Storage.WriteLinks(theLinks);
            Console.WriteLine("Wrote Links to database");
        }
    }
}
