using System.Collections.Generic;
using Prism.Events;

namespace FundSearcher.PubSubEvents
{
    /// <summary>
    /// 基金黑名单有更新事件
    /// </summary>
    class FundBlackRefreshEvent : PubSubEvent<List<string>>
    {
    }
}
