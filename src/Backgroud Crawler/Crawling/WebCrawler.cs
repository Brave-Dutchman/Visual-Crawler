using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using Core.Objects;

namespace Backgroud_Crawler.Crawling
{
    public class WebCrawler : Threaded
    {
        private const int MAX_THREADS = 5;
        private List<Thread> _threads;

        public WebCrawler()
        {
            _threads = new List<Thread>();
        }

        public void Run()
        {
            while (!Stop)
            {
                if (_threads.Count < 5)
                {
                    string url = CrawlingStorage.GetCrawledLink().Link;
                    Crawler(url);
                }
                else
                {
                    for (int i = _threads.Count -1; i >= 0 ; i--)
                    {
                        if (_threads[i].IsAlive) continue;
 
                        _threads.RemoveAt(i);
                    }
                }

                Thread.Sleep(1000);
            }
        }

        private void Crawler(string webUrl)
        {
            try
            {
                WebRequest myWebRequest = WebRequest.Create(webUrl);
                using (WebResponse myWebResponse = myWebRequest.GetResponse())
                {
                    using (Stream streamResponse = myWebResponse.GetResponseStream())
                    {
                        using (StreamReader sreader = new StreamReader(streamResponse))
                        {
                            Console.WriteLine("Crawled: {0}\n", myWebResponse.ResponseUri); //Reads it to the end

                            FormatCrawler crawler = new FormatCrawler();
                            crawler.Set(new CrawledContent(myWebResponse.ResponseUri.ToString(), sreader.ReadToEnd(), myWebResponse));

                            _threads.Add(new Thread(crawler.Format));
                            _threads[_threads.Count -1].Start();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\t{0}:{1}", webUrl, e.Message);
            }
        }
    }
}