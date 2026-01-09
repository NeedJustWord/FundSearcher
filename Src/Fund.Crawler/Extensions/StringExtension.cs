namespace Fund.Crawler.Extensions
{
    static class StringExtension
    {
        /// <summary>
        /// 获取基金类别
        /// </summary>
        /// <param name="fundName"></param>
        /// <returns></returns>
        public static string GetFundClass(this string fundName)
        {
            if (fundName == null) return "A类";

            fundName = fundName.Replace("(人民币)", "").Replace("(美元现汇)", "").Replace("人民币", "").Replace("美元现汇", "");

            var last = fundName[fundName.Length - 1];
            if (last >= 'A' && last <= 'Z') return $"{last}类";
            if (last >= 'a' && last <= 'z') return $"{char.ToUpper(last)}类";

            return "A类";
        }
    }
}
