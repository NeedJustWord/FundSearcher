namespace Fund.Crawler.Models
{
    /// <summary>
    /// 基础Key
    /// </summary>
    public class BaseKey
    {
        /// <summary>
        /// Key名字
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 索引
        /// </summary>
        public long Index { get; set; }

        public BaseKey(long index, string name)
        {
            Name = name;
            Index = index;
        }

        public virtual string GetKey(string url)
        {
            return $"[{Index},{url}]";
        }

        public string GetKey(string url, int threadId)
        {
            return $"{threadId} {GetKey(url)}";
        }
    }
}
