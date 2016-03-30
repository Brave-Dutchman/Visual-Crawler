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
        private readonly WebCrawler _crawler;

        private bool _stop;

        public CrawlerController()
        {
            _crawler = new WebCrawler();

            //TODO remove this or make a check

            const string url = "https://en.wikipedia.org/wiki/Frank_Matson";
            Storage.WriteLinks(new List<CrawledLink> { new CrawledLink(url) });

            _cpuCounter = new PerformanceCounter
            {
                CategoryName = "Processor",
                CounterName = "% Processor Time",
                InstanceName = "_Total"
            };
        }

        public float GetCurrentCpuUsage()
        {
            return _cpuCounter.NextValue();
        }

        public void Start()
        {
            Start(_crawler);

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

                    Start(_crawler);
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private static void Start(WebCrawler webCrawler)
        {
            Thread thread = new Thread(webCrawler.Run);
            thread.Start();
        }

        public void Stop()
        {
            _stop = true;
            _crawler.Stop = true;
        }
    }
}
