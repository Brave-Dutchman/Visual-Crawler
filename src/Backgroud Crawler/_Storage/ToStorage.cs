using System;
using System.Collections.Generic;
using System.Threading;
using Core.Data;
using Core.Database;

namespace Backgroud_Crawler._Storage
{
    public static class ToStorage
    {
        private static readonly List<Link> LINKS;
        private static readonly List<CrawledLink> CRAWLED_LINKS;
        private static readonly List<string> UPDATED;

        static ToStorage()
        {
            UPDATED = new List<string>();
            CRAWLED_LINKS = new List<CrawledLink>();
            LINKS = new List<Link>();
        }

        public static void WriteEnd()
        {
            Write();
            Storage.Disconnect();
        }

        public static void Write()
        {
            Thread.Sleep(2000);

            Console.WriteLine("\n{0}", "Saving the old links");

            lock (CRAWLED_LINKS)
            {
                Storage.WriteLinks(CRAWLED_LINKS);
                CRAWLED_LINKS.Clear();
                Console.WriteLine("{0}", "Saved the new uncrawled links");
            }

            lock (UPDATED)
            {
                Storage.UpdateCrawledLinks(UPDATED);
                UPDATED.Clear();
                Console.WriteLine("{0}", "Saved the updated crawled links");
            }

            lock (LINKS)
            {
                Storage.WriteLinks(LINKS);
                LINKS.Clear();
                Console.WriteLine("{0}", "Saved the found links");
            }
        }

        public static void Add(List<CrawledLink> crawledLinks, string updated)
        {
            lock (UPDATED)
            {
                UPDATED.Add(updated);
            }

            lock (CRAWLED_LINKS)
            {
                foreach (CrawledLink crawledLink in crawledLinks)
                {
                    if (!CRAWLED_LINKS.ContainsCrawled(crawledLink.Link) &&
                        !Storage.CheckCrawledLinksDouble(crawledLink.Link))
                    {
                        CRAWLED_LINKS.Add(crawledLink);
                    }
                }
            }
        }

        public static void Add(List<Link> links)
        {
            lock (LINKS)
            {
                LINKS.AddRange(links);
            }
        }
    }
}
