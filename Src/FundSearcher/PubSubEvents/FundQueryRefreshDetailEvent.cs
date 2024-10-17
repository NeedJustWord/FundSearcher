using Fund.Crawler.Models;
using Prism.Events;

namespace FundSearcher.PubSubEvents
{
    class FundQueryRefreshDetailEvent : PubSubEvent<FundInfo>
    {
    }
}
