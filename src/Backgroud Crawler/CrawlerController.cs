using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Backgroud_Crawler.Crawling;
using Core;
using Core.Objects;

namespace Backgroud_Crawler
{
    public class CrawlerController
    {
        private readonly PerformanceCounter _cpuCounter;
        private WebCrawler _crawler;

        private bool _stop;

        public CrawlerController()
        {
            _cpuCounter = new PerformanceCounter
            {
                CategoryName = "Processor",
                CounterName = "% Processor Time",
                InstanceName = "_Total"
            };
        }

        private void StartUp()
        {
            bool exists = Storage.Enable();

            if (!exists)
            {
                const string url = "http://www.insidegamer.nl";
                Storage.WriteLinks(new List<CrawledLink> { new CrawledLink(url) });
                Storage.WriteLinks(new List<Link> { new Link("www.insidegamer.nl", "http://www.insidegamer.nl", "http://www.insidegamer.nl") });
            }

            _crawler = new WebCrawler();
            StartCrawling(_crawler);
        }

        public float GetCurrentCpuUsage()
        {
            return _cpuCounter.NextValue();
        }

        public void Start()
        {
            StartUp();
            bool isWaiting = false;

            while (!_stop)
            {
                float cpu = GetCurrentCpuUsage();

                if (cpu > 60)
                {
                    isWaiting = true;
                    _crawler.Stop = true;
                }
                else if (isWaiting && cpu < 20)
                {
                    isWaiting = false;
                    _crawler.Stop = false;

                    StartCrawling(_crawler);
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private static void StartCrawling(WebCrawler webCrawler)
        {
            Thread thread = new Thread(webCrawler.Run);
            thread.Start();
        }

        public void Stop()
        {
            _stop = true;
            _crawler.Stop = true;
            Storage.Disconnect();
        }
    }
}
