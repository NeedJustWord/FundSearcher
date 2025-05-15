using System.Collections.Generic;

namespace Fund.Crawler.Models
{
    /// <summary>
    /// 指数信息
    /// </summary>
    public class IndexInfo : BaseInfo
    {
        private string indexCode;
        /// <summary>
        /// 指数代码
        /// </summary>
        public string IndexCode
        {
            get { return indexCode; }
            set { SetProperty(ref indexCode, value); }
        }

        private string indexName;
        /// <summary>
        /// 指数名称
        /// </summary>
        public string IndexName
        {
            get { return indexName; }
            set { SetProperty(ref indexName, value); }
        }

        private int trackingCount;
        /// <summary>
        /// 跟踪此指数的基金数量
        /// </summary>
        public int TrackingCount
        {
            get { return trackingCount; }
            set { SetProperty(ref trackingCount, value); }
        }

        private List<FundBaseInfo> fundBaseInfos;
        /// <summary>
        /// 指数相关基金
        /// </summary>
        public List<FundBaseInfo> FundBaseInfos
        {
            get { return fundBaseInfos; }
            set { SetProperty(ref fundBaseInfos, value); }
        }
    }
}
