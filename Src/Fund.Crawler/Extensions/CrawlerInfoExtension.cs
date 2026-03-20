using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fund.Crawler.Models;

namespace Fund.Crawler.Extensions
{
    public static class CrawlerInfoExtension
    {
        public static string PrintCrawlerInfo(this IEnumerable<CrawlerInfo> source)
        {
            var list = source.Where(t => t != null).OrderBy(t => t.Name).ToList();
            var sb = new StringBuilder();
            var sumTicks = list.Sum(t => t.HandleTime.Ticks);
            sb.AppendLine($"总数：{list.Count}，总处理耗时：{sumTicks}，平均处理耗时：{(list.Count == 0 ? 0 : sumTicks / list.Count)}");
            sb.AppendLine(list.ToJson());
            return sb.ToString();
        }
    }
}
