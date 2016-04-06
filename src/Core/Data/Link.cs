using Newtonsoft.Json;

namespace Core.Data
{
    public class Link
    {
        public string Host { get; set; }
        public string From { get; set; }
        public string   To { get; set; }
        public int TimesOnPage { get; set; }

        [JsonConstructor]
        public Link()
        {
            
        }

        public Link(string host, string @from, string to)
        {
            Host = host;
            From = @from;
            To = to;
            TimesOnPage = 1;
        }

        public override string ToString()
        {
            return string.Format("{0} : {1} : {2} : {3}", Host, From, To, TimesOnPage);
        }
    }
}
