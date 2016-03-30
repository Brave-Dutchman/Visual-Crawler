using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core.Objects;
using Newtonsoft.Json;

namespace Core
{
    public static class StorageJson
    {
        private static string _filePath;

        private const string LINKNAME = "Link.json";                    //Value for Link tablename
        private const string CRAWLED_LINKNAME = "CrawledLink.json";      //Value for CrawledLink tablename

        static StorageJson()
        {
            Enable();
        }

        private static void Enable()
        {
            _filePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\";

            if (!File.Exists(_filePath + LINKNAME))
            {
                File.WriteAllText(_filePath + LINKNAME, JsonConvert.SerializeObject(new List<Link>()));
            }

            if (!File.Exists(_filePath + CRAWLED_LINKNAME))
            {
                File.WriteAllText(_filePath + CRAWLED_LINKNAME, JsonConvert.SerializeObject(new List<Link>()));
            }
        }

        public static void UpdateCrawledLinks(List<string> strings)
        {
            List<CrawledLink> crawled = GetCrawledLinks();

            foreach (string s in strings)
            {
                foreach (CrawledLink crawledLink in crawled)
                {
                    if (crawledLink.Link == s)
                    {
                        crawledLink.IsCrawled = true;
                    }
                }
            }

            File.WriteAllText(_filePath + CRAWLED_LINKNAME, JsonConvert.SerializeObject(crawled));
        }

        /// <summary>
        ///     Write Links to database.
        /// </summary>
        /// <param name="links">List of Links</param>
        public static void WriteLinks(List<Link> links)
        {
            File.WriteAllText(_filePath + LINKNAME, JsonConvert.SerializeObject(GetLinks().Concat(links)));
        }

        /// <summary>
        ///     Write CrawledLinks to database.
        /// </summary>
        /// <param name="links">List of CrawledLinks</param>
        public static void WriteLinks(List<CrawledLink> links)
        {
            File.WriteAllText(_filePath + CRAWLED_LINKNAME, JsonConvert.SerializeObject(GetCrawledLinks().Concat(links)));
        }

        public static List<Link> GetLinks()
        {
            return JsonConvert.DeserializeObject<List<Link>>(File.ReadAllText(_filePath + LINKNAME));
        }

        /// <summary>
        ///     Fetch the CrawledLinks from the database and return them in a list.
        /// </summary>
        /// <returns>List of CrawledLinks</returns>
        public static List<CrawledLink> GetCrawledLinks()
        {
            return JsonConvert.DeserializeObject<List<CrawledLink>>(File.ReadAllText(_filePath + CRAWLED_LINKNAME));
        }

        public static bool CheckCrawledLinksDouble(string itemLink)
        {
            string text = File.ReadAllText(_filePath + CRAWLED_LINKNAME);
            return text.Contains(string.Format("\"Link\":\"{0}\"", itemLink));
        }

        public static Stack<CrawledLink> ReadNotCrawledLinks()
        {
            Stack<CrawledLink> crawled = new Stack<CrawledLink>();

            int count = 0;
            foreach (CrawledLink link in GetCrawledLinks())
            {
                if (!link.IsCrawled)
                {
                    crawled.Push(link);
                    count++;

                    if (count >= 100)
                    {
                        break;
                    }
                }
            }

            return crawled;
        }
    }
}
