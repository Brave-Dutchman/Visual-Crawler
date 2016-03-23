using System.Threading.Tasks;

namespace Backgroud_Crawler
{
    public class CrawlerController
    {
        private static WebCrawler[] WebCrawlers { get; set; }

        public CrawlerController()
        {
            WebCrawlers = new[]
            {
                new WebCrawler(1) , new WebCrawler(2) , new WebCrawler(3) , new WebCrawler(4)
            };
        }

        public void Start()
        {
            foreach (var item in WebCrawlers)
            {
                 Task.Run(() => { item.Run(); });
            }
            /*
            for (int i = 0; i < 1; i++)
            {
                int index = i;
                Task.Run(() => { WebCrawlers[index].Run(); });
            }
            */
        }

        public void Stop()
        {
            
        }
    }
}
