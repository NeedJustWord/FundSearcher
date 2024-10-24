namespace Fund.Crawler.Models
{
    /// <summary>
    /// 指数Key
    /// </summary>
    public class IndexKey : BaseKey
    {
        /// <summary>
        /// 指数代码
        /// </summary>
        public string IndexCode { get; set; }

        public IndexKey(long index, string indexCode) : base(index)
        {
            IndexCode = indexCode;
        }

        public override string GetKey(string url)
        {
            return $"[{IndexCode},{Index},{url}]";
        }
    }
}
