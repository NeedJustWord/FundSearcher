using Fund.Crawler.Models;
using Prism.Events;

namespace Fund.Crawler.PubSubEvents
{
    public class CrawlingProgressEvent : PubSubEvent<CrawlingProgressModel>
    {
    }
}
