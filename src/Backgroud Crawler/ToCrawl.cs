using System;
using System.Collections.Generic;
using Core;

namespace Backgroud_Crawler
{
    public static class ToCrawl
    {
        private static Stack<CrawledLink> _linksToCrawl;

        public static int Count
        {
            get { return _linksToCrawl.Count; }
        }


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
