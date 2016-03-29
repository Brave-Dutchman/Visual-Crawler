namespace Core.Objects
{
    public class Link
    {
        public string Host { get; set; }
        public string From { get; set; }
        public string   To { get; set; }
        public int TimesOnPage { get; set; }

        private Link()
        {
            TimesOnPage = 1;
        }

        public Link(string host, string @from, string to) : this()
        {
            Host = host;
            From = @from;
            To = to;
        }

        public override string ToString()
        {
            return string.Format("{0} : {1} : {2} : {3}", Host, From, To, TimesOnPage);
        }
    }
}
