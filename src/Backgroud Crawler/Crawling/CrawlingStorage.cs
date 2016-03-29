using System;
using System.Collections.Generic;
using Core;
using Core.Objects;

namespace Backgroud_Crawler.Crawling
{
    public static class CrawlingStorage
    {
        private static readonly Stack<CrawledContent> FOUND_CONTENT;
        private static Stack<CrawledLink> _linksToCrawl;

        public static int Count
        {
            get { return FOUND_CONTENT.Count; }
        }

        static CrawlingStorage()
        {
            _linksToCrawl = Storage.GetCrawledLinks();
            FOUND_CONTENT = new Stack<CrawledContent>();
        }

        public static void AddFound(CrawledContent content)
        {
            lock (FOUND_CONTENT)
            {
                FOUND_CONTENT.Push(content);
            }
        }

        public static CrawledContent GetNewContent()
        {
            lock (FOUND_CONTENT)
            {
                return FOUND_CONTENT.Count > 0 ? FOUND_CONTENT.Pop() : null;
            }
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
