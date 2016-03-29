using System;
using System.Collections.Generic;
using Core;
using Core.Objects;

namespace Backgroud_Crawler.Crawling
{
    public class ToDbStorage
    {
        private const int MAX = 1000;

        private static readonly List<Link> _links;
        private static readonly List<CrawledLink> _crawledLinks;

        static ToDbStorage()
        {
            _links = new List<Link>();
            _crawledLinks = new List<CrawledLink>();
        }

        public static void Add(List<CrawledLink> crawledLinks)
        {
            lock (_crawledLinks)
            {
                Console.WriteLine("Set lock crawled");

                _crawledLinks.AddRange(crawledLinks);

                //if (_crawledLinks.Count > MAX)
                //{
                //    Storage.WriteLinks(_crawledLinks);
                //    Console.WriteLine("Wrote CrawledLinks to database");
                //    _crawledLinks.Clear();
                //}
            }

            Console.WriteLine("removed lock crawled");
        }

        public static void Add(List<Link> links)
        {
            lock (_links)
            {
                Console.WriteLine("Set lock links");

                _links.AddRange(links);

                //if (_links.Count >= MAX)
                //{
                //    Storage.WriteLinks(_links);
                //    Console.WriteLine("Wrote Links to database");
                //    _links.Clear();
                //}
            }

            Console.WriteLine("removed lock crawled");
        }
    }
}
