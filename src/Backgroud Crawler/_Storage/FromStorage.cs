using System;
using System.Collections.Generic;
using Core.Data;
using Core.Database;

namespace Backgroud_Crawler._Storage
{
    public static class FromStorage
    {
        private static Stack<CrawledLink> _linksToCrawl;

        static FromStorage()
        {
            _linksToCrawl = new Stack<CrawledLink>();
        }

        /// <summary>
        /// Gets the 
        /// </summary>
        public static void GetNewLinks()
        {
            lock (_linksToCrawl)
            {
                Console.WriteLine("{0}\n", "Getting new links");
                _linksToCrawl = Storage.ReadNotCrawledLinks();
            }
        }

        /// <summary>
        /// Gets the next crawled link from the stack,
        /// if the stack is empty gets new links from the database
        /// </summary>
        /// <returns>The next Crawledlink to crawl</returns>
        public static CrawledLink GetCrawledLink()
        {
            lock (_linksToCrawl)
            {
                if (_linksToCrawl.Count <= 0)
                {
                    ToStorage.Write();
                    GetNewLinks();
                }

                return _linksToCrawl.Pop();
            }
        }
    }
}
