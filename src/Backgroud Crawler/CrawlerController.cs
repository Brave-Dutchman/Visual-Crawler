using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Backgroud_Crawler.Crawling;
using Core;
using Core.Objects;

namespace Backgroud_Crawler
{
    public class CrawlerController
    {
        private const int MAX_CRAWLERS = 5;
        private readonly PerformanceCounter _cpuCounter;
        private readonly WebCrawler _crawler;

        private List<FormatCrawler> _formatCrawlers;
        private bool _stop;

        public CrawlerController()
        {
            _crawler = new WebCrawler();
            _formatCrawlers = new List<FormatCrawler> { new FormatCrawler() };

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
            StartTask(_crawler);
            foreach (FormatCrawler crawler in _formatCrawlers)
            {
                StartTask(crawler);
            }

            while (!_stop)
            {
                float cpu = GetCurrentCpuUsage();

                if (cpu > 80)
                {
                    if (_formatCrawlers.Count > 0)
                    {
                        int index = _formatCrawlers.Count - 1;

                        _formatCrawlers[index].Stop = true;
                        _formatCrawlers.RemoveAt(index);
                        Console.WriteLine("Removed a FormatCrawler, {0}", _formatCrawlers.Count);
                    }
                }
                else if (cpu < 50)
                {
                    if (_formatCrawlers.Count < MAX_CRAWLERS)
                    {
                        FormatCrawler crawler = new FormatCrawler();
                        _formatCrawlers.Add(crawler);
                        StartTask(crawler);
                        Console.WriteLine("Created new FormatCrawler, {0}", _formatCrawlers.Count);
                    }
                }

                Thread.Sleep(1000);
            }
        }

        private static void StartTask(Threaded threaded)
        {
            Task.Run(() => { threaded.Run(); });
        }

        public void Stop()
        {
            _stop = true;
            _crawler.Stop = true;

            foreach (FormatCrawler crawler in _formatCrawlers)
            {
                crawler.Stop = true;
            }
        }
    }
}
