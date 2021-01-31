using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Crawler.DataHandler
{
    /// <summary>
    /// 正则辅助类
    /// </summary>
    public static class RegexHelper
    {
        #region Html正则表达式
        private static string htmlTagContentPattern = @"(?<=>)[\s\S]*(?=<)";
        /// <summary>
        /// 获取Html标签内容的正则表达式
        /// </summary>
        /// <returns></returns>
        public static string GetHtmlTagContentPattern()
        {
            return htmlTagContentPattern;
        }

        private static string htmlTagPattern = @"<(?<HtmlTag>{0})[^>]*>(?><\k<HtmlTag>[^>]*>(?<groupName>)|</\k<HtmlTag>>(?<-groupName>)|(?:(?!</?\k<HtmlTag>\b)[\s\S])*)*(?(groupName)(?!))</\k<HtmlTag>>";
        private static Dictionary<string, string> htmlTagPatternDict = new Dictionary<string, string>();
        /// <summary>
        /// 获取指定Html标签的正则表达式
        /// </summary>
        /// <param name="htmlTag"></param>
        /// <returns></returns>
        public static string GetHtmlTagPattern(string htmlTag)
        {
            if (htmlTagPatternDict.ContainsKey(htmlTag)) return htmlTagPatternDict[htmlTag];
            var value = string.Format(htmlTagPattern, htmlTag);
            htmlTagPatternDict[htmlTag] = value;
            return value;
        }

        private static string htmlTagByAttriPattern = @"<(?<HtmlTag>{0})[^>]*\s{1}=(?<Quote>['""]?){2}\k<Quote>[^>]*>(?><\k<HtmlTag>[^>]*>(?<groupName>)|</\k<HtmlTag>>(?<-groupName>)|(?:(?!</?\k<HtmlTag>\b)[\s\S])*)*(?(groupName)(?!))</\k<HtmlTag>>";
        /// <summary>
        /// 获取指定Html标签具有指定属性的正则表达式
        /// </summary>
        /// <param name="htmlTag"></param>
        /// <param name="attriName"></param>
        /// <param name="attriValue"></param>
        /// <returns></returns>
        public static string GetHtmlTagPattern(string htmlTag, string attriName, string attriValue)
        {
            return string.Format(htmlTagByAttriPattern, htmlTag, attriName, attriValue);
        }
        #endregion

        #region 正则匹配
        /// <summary>
        /// 判断是否匹配
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool IsMatch(string input, string pattern)
        {
            return Regex.IsMatch(input, pattern);
        }

        /// <summary>
        /// 获取第一个匹配项
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static Match Match(string input, string pattern)
        {
            return Regex.Match(input, pattern);
        }

        /// <summary>
        /// 获取所有匹配项
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static MatchCollection Matches(string input, string pattern)
        {
            return Regex.Matches(input, pattern);
        }
        #endregion
    }
}
