using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace Backgroud_Crawler
{
    public class WebCrawler
    {
        private readonly int _number;
        private readonly List<string> _foundLinks;

        public WebCrawler(int number)
        {
            _number = number;
            _foundLinks = new List<string>();
        }

        public void Run()
        {
            Crawler("https://en.wikipedia.org/wiki/Special:Random");
        }

        private void Crawler(string url)
        {
            WebRequest myWebRequest = WebRequest.Create(url);
            using (WebResponse myWebResponse = myWebRequest.GetResponse())
            {
                using (Stream streamResponse = myWebResponse.GetResponseStream())
                {
                    using (StreamReader sreader = new StreamReader(streamResponse))
                    {
                        string rstring = sreader.ReadToEnd(); //reads it to the end

                        string header = myWebResponse.ResponseUri.Scheme + "://" + myWebResponse.ResponseUri.Host;

                        GetNewLinks(rstring, header);
                    }
                }
            }
        }

        private void GetNewLinks(string content, string header)
        {
            Regex regexLink = new Regex("(?<=<a\\s*?href=(?:'|\"))[^'\"]*?(?=(?:'|\"))");

            foreach (object match in regexLink.Matches(content))
            {
                string url = match.ToString();
                if (url.StartsWith("#") || url.Contains(":") || url.Contains(".")) continue;

                if (!url.StartsWith("http"))
                {
                    url = header + url;
                }

                if (!_foundLinks.Contains(url))
                {
                    _foundLinks.Add(url);
                    Console.WriteLine($"{_number}:{_foundLinks.Count}:{url}\n");
                    Crawler(url);
                }
            }
        }
    }
}
