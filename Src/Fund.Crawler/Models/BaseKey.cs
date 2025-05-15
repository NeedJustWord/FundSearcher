namespace Fund.Crawler.Models
{
    /// <summary>
    /// 基础Key
    /// </summary>
    public class BaseKey
    {
        /// <summary>
        /// 索引
        /// </summary>
        public long Index { get; set; }

        public BaseKey(long index)
        {
            Index = index;
        }

        public virtual string GetKey(string url)
        {
            return $"[{Index},{url}]";
        }
    }
}
