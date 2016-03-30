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
            _linksToCrawl = Storage.GetCrawledLinks();
        }

        public static CrawledLink GetCrawledLink()
        { 
            lock (_linksToCrawl)
            {
                if (_linksToCrawl.Count <= 0)
                {
                    _linksToCrawl = Storage.GetCrawledLinks();
                }
                
                try
                {
                    return _linksToCrawl.Pop();
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
    }
}
