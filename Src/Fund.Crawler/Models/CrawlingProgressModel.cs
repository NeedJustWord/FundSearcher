using System;

namespace Fund.Crawler.Models
{
    public class CrawlingProgressModel
    {
        /// <summary>
        /// 总数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 当前值
        /// </summary>
        public int Current { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 是否已完成
        /// </summary>
        public bool Finish { get; set; }

        /// <summary>
        /// 异常
        /// </summary>
        public Exception Exception { get; set; }
    }
}
