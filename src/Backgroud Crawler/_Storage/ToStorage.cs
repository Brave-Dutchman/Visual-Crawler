using System;
using System.Collections.Generic;
using System.Threading;
using Core.Data;
using Core.Database;

namespace Backgroud_Crawler._Storage
{
    /// <summary>
    /// Responsible for writing the data to the database
    /// </summary>
    public static class ToStorage
    {
        //Fields
        private static readonly List<Link> LINKS;
        private static readonly List<CrawledLink> CRAWLED_LINKS;
        private static readonly List<string> UPDATED;

        /// <summary>
        /// Constructor of ToStorage
        /// </summary>
        static ToStorage()
        {
            UPDATED = new List<string>();
            CRAWLED_LINKS = new List<CrawledLink>();
            LINKS = new List<Link>();
        }

        /// <summary>
        /// Writes everything to the database, and then closes the database connection 
        /// </summary>
        public static void WriteEnd()
        {
            Write();
            Storage.Disconnect();
        }

        /// <summary>
        /// Writes all the CrawledLinks, updated Links and Links to the database
        /// </summary>
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

        /// <summary>
        /// Add the new links to the CRAWLED_LINKS
        /// </summary>
        /// <param name="crawledLinks">A list with all the found links</param>
        /// <param name="updated">The links that was craled</param>
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

        /// <summary>
        /// Add the new links to the LINKS
        /// </summary>
        /// <param name="links">A list with all the found links</param>
        public static void Add(List<Link> links)
        {
            lock (LINKS)
            {
                LINKS.AddRange(links);
            }
        }
    }
}
