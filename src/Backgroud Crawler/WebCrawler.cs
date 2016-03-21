﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Backgroud_Crawler
{
    public class WebCrawler
    {
        private const string StartUrl = "http://www.insidegamer.nl/";
        
        private readonly int _number;

        private readonly List<string> _links;
        private readonly List<ALink> _foundText;

        public WebCrawler(int number)
        {
            _number = number;

            _links = new List<string>();
            _foundText = new List<ALink>();
        }

        public void Run()
        {
            int level = 0;
            Crawler(StartUrl, level);

            while (true)
            {
                level++;

                ALink[] linsk = GetByLevels(level);

                foreach (ALink link in linsk)
                {
                    Crawler(link.Link, level);
                }
            }
        }

        private void Crawler(string webUrl, int level)
        {
            try
            {
                WebRequest myWebRequest = WebRequest.Create(webUrl);
                using (WebResponse myWebResponse = myWebRequest.GetResponse())
                {
                    using (Stream streamResponse = myWebResponse.GetResponseStream())
                    {
                        string header = myWebResponse.ResponseUri.Scheme + "://" + myWebResponse.ResponseUri.Host;
                        Console.WriteLine($"{_number}:{level}:{myWebResponse.ResponseUri}\n");

                        using (StreamReader sreader = new StreamReader(streamResponse))
                        {
                            string content = sreader.ReadToEnd(); //reads it to the end
                            Regex regexLink = new Regex("(?<=<a\\s*?href=(?:'|\"))[^'\"]*?(?=(?:'|\"))");

                            foreach (object match in regexLink.Matches(content))
                            {
                                string url = match.ToString();
                                if (url.StartsWith("#") || Path.HasExtension(url) || url.Equals("/")) continue;

                                if (!url.StartsWith("http"))
                                {
                                    url = header + url;
                                }

                                if (_links.Contains(url)) continue;

                                _foundText.Add(new ALink(level + 1, url));
                                _links.Add(url);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"\t{webUrl}:{e.Message}");
            }
        }

        private ALink[] GetByLevels(int level)
        {
            return _foundText.FindAll(x => x.Level == level).ToArray();
        }
    }

    public class ALink
    {
        public ALink(int level, string link)
        {
            Level = level;
            Link = link;
        }

        public int Level { get; set; }
        public string Link { get; set; }

        public override string ToString()
        {
            return Link;
        }
    }
}