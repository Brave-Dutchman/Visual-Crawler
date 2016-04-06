using Newtonsoft.Json;

namespace Core.Data
{
    /// <summary>
    /// The usable link for the visual application
    /// </summary>
    public class Link
    {
        //Fields
        public string Host { get; set; }
        public string From { get; set; }
        public string   To { get; set; }
        public int TimesOnPage { get; set; }

        /// <summary>
        /// Constructor of Link
        /// </summary>
        [JsonConstructor]
        public Link() { }

        /// <summary>
        /// Constructor of Link
        /// </summary>
        /// <param name="host">Link (URL)</param>
        /// <param name="from">Source of Link</param>
        /// <param name="to">Destination of Link</param>
        public Link(string host, string @from, string to)
        {
            Host = host;
            From = @from;
            To = to;
            TimesOnPage = 1;
        }

        /// <summary>
        /// Return Link as string
        /// </summary>
        /// <returns>Link (URL) string</returns>
        public override string ToString()
        {
            return string.Format("{0} : {1} : {2} : {3}", Host, From, To, TimesOnPage);
        }
    }
}
