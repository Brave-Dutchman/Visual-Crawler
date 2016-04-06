﻿using System;
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

        public static void GetNewLinks()
        {
            lock (_linksToCrawl)
            {
                Console.WriteLine("{0}\n", "Getting new links");
                _linksToCrawl = Storage.ReadNotCrawledLinks();
            }
        }

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