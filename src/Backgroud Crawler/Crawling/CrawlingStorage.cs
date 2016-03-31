using System;
using System.Collections.Generic;
using Core;
using Core.Objects;

namespace Backgroud_Crawler.Crawling
{
    public static class CrawlingStorage
    {
        private static Stack<CrawledLink> _linksToCrawl;

        static CrawlingStorage()
        {
            _linksToCrawl = Storage.ReadNotCrawledLinks();
        }

        public static void GetNewLinks()
        {
            lock (_linksToCrawl)
            {
                _linksToCrawl = Storage.ReadNotCrawledLinks();
            }
        }

        public static CrawledLink GetCrawledLink()
        {
            lock (_linksToCrawl)
            {
                if (_linksToCrawl.Count <= 0)
                {
                    Console.WriteLine("\n{0}", "Saving the old links");
                    ToDbStorage.Write();
                    Console.WriteLine("{0}\n", "Getting new links");
                    GetNewLinks();
                }

                return _linksToCrawl.Pop();
            }
        }
    }
}
