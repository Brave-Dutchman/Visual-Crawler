namespace Backgroud_Crawler.Crawling
{
    public abstract class Threaded
    {
        public bool Stop { get; set; }

        protected Threaded()
        {
            Stop = false;
        }

        public abstract void Run();
    }
}
