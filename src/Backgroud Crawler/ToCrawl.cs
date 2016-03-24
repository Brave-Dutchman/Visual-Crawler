using System.Collections.Generic;
using Core;

namespace Backgroud_Crawler
{
    public static class ToCrawl
    {
        private static Stack<CrawledLink> _linksToCrawl;

        static ToCrawl()
        {
            _linksToCrawl = Storage.GetCrawledLinks();
        }

        public static CrawledLink GetCrawledLink()
        { 
            lock (_linksToCrawl)
            {
                if (_linksToCrawl.Count <= 0)
                {
                    new WebCrawler(0).FirstCrawl();
                    _linksToCrawl = Storage.GetCrawledLinks();
                }

                CrawledLink link = _linksToCrawl.Pop();
                return link;
            }
        }

        public static bool ContainsCrawled(this List<CrawledLink> crawled, CrawledLink toFind)
        {
            foreach (CrawledLink link in crawled)
            {
                if (link.Link == toFind.Link)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
