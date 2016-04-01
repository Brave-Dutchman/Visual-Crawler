using System;
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
        private static Timer _timer;

        private readonly PerformanceCounter _cpuCounter;
        private WebCrawler _crawler;
        private bool _isWaiting;

        public CrawlerController()
        {
            _cpuCounter = new PerformanceCounter
            {
                CategoryName = "Processor",
                CounterName = "% Processor Time",
                InstanceName = "_Total"
            };
        }

        private void TimerCallback(object state)
        {
            float cpu = GetCurrentCpuUsage();

            if (!_isWaiting && cpu > 60)
            {
                _isWaiting = true;

                Console.WriteLine("\nStopped Crawling\n");

                _crawler.Stop = true;
            }
            else if (_isWaiting && cpu < 20)
            {
                _isWaiting = false;

                Console.WriteLine("\nStarted Crawling again\n");

                _crawler.Stop = false;
                StartCrawling(_crawler);
            }
        }

        private void StartUp()
        {
            bool exists = Storage.Enable();

            if (!exists)
            {
                const string url = "http://www.insidegamer.nl";
                ToDbStorage.Add(new List<CrawledLink> { new CrawledLink(url) }, url);
                ToDbStorage.Add(new List<Link> { new Link("www.insidegamer.nl", "http://www.insidegamer.nl", "http://www.insidegamer.nl") });

                new WebCrawler().Crawler(url);

                Thread.Sleep(1000);
            }

            _crawler = new WebCrawler();
            StartCrawling(_crawler);
        }

        public float GetCurrentCpuUsage()
        {
            dynamic firstValue = _cpuCounter.NextValue();

            Thread.Sleep(1000);

            dynamic secondValue = _cpuCounter.NextValue();

            return secondValue;
        }

        public void Start()
        {
            StartUp();

            _timer = new Timer(TimerCallback, null, 0, 2000);
        }

        private static void StartCrawling(WebCrawler webCrawler)
        {
            Thread thread = new Thread(webCrawler.Run);
            thread.Start();
        }

        public void Stop()
        {
            _timer.Dispose();
            _crawler.Stop = true;

            ToDbStorage.Write();
            Storage.Disconnect();
        }
    }
}
