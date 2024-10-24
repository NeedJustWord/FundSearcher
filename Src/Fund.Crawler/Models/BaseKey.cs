namespace Fund.Crawler.Models
{
    /// <summary>
    /// 基础Key
    /// </summary>
    public abstract class BaseKey
    {
        /// <summary>
        /// 索引
        /// </summary>
        public long Index { get; set; }

        public BaseKey(long index)
        {
            Index = index;
        }

        public abstract string GetKey(string url);
    }
}
