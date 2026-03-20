using System;
using System.Collections.Generic;
using Fund.Crawler.Interfaces;

namespace Fund.Crawler.Models
{
    /// <summary>
    /// 指数信息集合
    /// </summary>
    public class IndexInfoList : ICrawlerInfo
    {
        /// <summary>
        /// 指数信息集合
        /// </summary>
        public List<IndexInfo> IndexInfos { get; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 爬取信息
        /// </summary>
        public CrawlerInfo CrawlerInfo { get; set; }

        /// <summary>
        /// 空值
        /// </summary>
        public static IndexInfoList EmptyValue => new IndexInfoList(0);

        public IndexInfoList()
        {
            IndexInfos = new List<IndexInfo>();
        }

        public IndexInfoList(int capacity)
        {
            IndexInfos = new List<IndexInfo>(capacity);
        }

        public IndexInfoList(List<IndexInfo> list, DateTime updateTime) : this(list.Count)
        {
            IndexInfos.AddRange(list);
            UpdateTime = updateTime;
        }
    }
}
