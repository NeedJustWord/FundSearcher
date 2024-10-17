using System;
using Prism.Events;

namespace FundSearcher.Extensions
{
    static class IEventAggregatorExtension
    {
        #region 订阅消息
        public static void Subscribe<TEventType>(this IEventAggregator aggregator, Action action) where TEventType : PubSubEvent, new()
        {
            aggregator.GetEvent<TEventType>().Subscribe(action);
        }

        public static void Subscribe<TEventType, TMessage>(this IEventAggregator aggregator, Action<TMessage> action) where TEventType : PubSubEvent<TMessage>, new()
        {
            aggregator.GetEvent<TEventType>().Subscribe(action);
        }
        #endregion

        #region 发布消息
        public static void Publish<TEventType>(this IEventAggregator aggregator) where TEventType : PubSubEvent, new()
        {
            aggregator.GetEvent<TEventType>().Publish();
        }

        public static void Publish<TEventType, TMessage>(this IEventAggregator aggregator, TMessage msg) where TEventType : PubSubEvent<TMessage>, new()
        {
            aggregator.GetEvent<TEventType>().Publish(msg);
        }
        #endregion
    }
}
