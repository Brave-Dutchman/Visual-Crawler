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

        public static void UpdateStorage()
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
                if (_linksToCrawl.Count <= 0) return null;

                return _linksToCrawl.Pop();
            }
        }
    }
}
