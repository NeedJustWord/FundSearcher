using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Crawler.DataHandler.Extensions;

namespace Crawler.DataHandler
{
    /// <summary>
    /// Html解码
    /// </summary>
    public static class HtmlDecode
    {
        #region 获取指定标签
        /// <summary>
        /// 获取指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <returns></returns>
        public static MatchCollection GetHtmlTag(string input, string htmlTag)
        {
            return Regex.Matches(input, RegexHelper.GetHtmlTagPattern(htmlTag));
        }

        /// <summary>
        /// 获取第<paramref name="index"/>个指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Match GetHtmlTag(string input, string htmlTag, int index)
        {
            if (index < 0) return null;

            var matches = GetHtmlTag(input, htmlTag);
            return index < matches.Count ? matches[index] : null;
        }

        /// <summary>
        /// 获取指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetHtmlTagValue(string input, string htmlTag)
        {
            return GetHtmlTag(input, htmlTag).Select(t => t.Value);
        }

        /// <summary>
        /// 获取第<paramref name="index"/>个指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetHtmlTagValue(string input, string htmlTag, int index)
        {
            var match = GetHtmlTag(input, htmlTag, index);
            return match != null && match.Success ? match.Value : string.Empty;
        }

        /// <summary>
        /// 获取第一个指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <returns></returns>
        public static Match GetFirstHtmlTag(string input, string htmlTag)
        {
            return Regex.Match(input, RegexHelper.GetHtmlTagPattern(htmlTag));
        }

        /// <summary>
        /// 获取第一个指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <returns></returns>
        public static string GetFirstHtmlTagValue(string input, string htmlTag)
        {
            var match = GetFirstHtmlTag(input, htmlTag);
            return match != null && match.Success ? match.Value : string.Empty;
        }
        #endregion

        #region 获取具有指定属性键值对的指定标签
        /// <summary>
        /// 获取具有指定属性键值对的指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <param name="attriName"></param>
        /// <param name="attriValue"></param>
        /// <returns></returns>
        public static MatchCollection GetHtmlTagByAttri(string input, string htmlTag, string attriName, string attriValue)
        {
            return Regex.Matches(input, RegexHelper.GetHtmlTagPattern(htmlTag, attriName, attriValue));
        }

        /// <summary>
        /// 获取第<paramref name="index"/>个具有指定属性键值对的指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <param name="attriName"></param>
        /// <param name="attriValue"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static Match GetHtmlTagByAttri(string input, string htmlTag, string attriName, string attriValue, int index)
        {
            if (index < 0) return null;

            var matches = GetHtmlTagByAttri(input, htmlTag, attriName, attriValue);
            return index < matches.Count ? matches[index] : null;
        }

        /// <summary>
        /// 获取具有指定属性键值对的指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <param name="attriName"></param>
        /// <param name="attriValue"></param>
        /// <returns></returns>
        public static IEnumerable<string> GetHtmlTagValueByAttri(string input, string htmlTag, string attriName, string attriValue)
        {
            return GetHtmlTagByAttri(input, htmlTag, attriName, attriValue).Select(t => t.Value);
        }

        /// <summary>
        /// 获取第<paramref name="index"/>个具有指定属性键值对的指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <param name="attriName"></param>
        /// <param name="attriValue"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetHtmlTagValueByAttri(string input, string htmlTag, string attriName, string attriValue, int index)
        {
            var match = GetHtmlTagByAttri(input, htmlTag, attriName, attriValue, index);
            return match != null && match.Success ? match.Value : string.Empty;
        }

        /// <summary>
        /// 获取第一个具有指定属性键值对的指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <param name="attriName"></param>
        /// <param name="attriValue"></param>
        /// <returns></returns>
        public static Match GetFirstHtmlTagByAttri(string input, string htmlTag, string attriName, string attriValue)
        {
            return Regex.Match(input, RegexHelper.GetHtmlTagPattern(htmlTag, attriName, attriValue));
        }

        /// <summary>
        /// 获取第一个具有指定属性键值对的指定标签
        /// </summary>
        /// <param name="input"></param>
        /// <param name="htmlTag"></param>
        /// <param name="attriName"></param>
        /// <param name="attriValue"></param>
        /// <returns></returns>
        public static string GetFirstHtmlTagValueByAttri(string input, string htmlTag, string attriName, string attriValue)
        {
            var match = GetFirstHtmlTagByAttri(input, htmlTag, attriName, attriValue);
            return match != null && match.Success ? match.Value : string.Empty;
        }
        #endregion

        /// <summary>
        /// 获取标签内容
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetHtmlTagContent(string input)
        {
            var match = Regex.Match(input, RegexHelper.GetHtmlTagContentPattern());
            return match != null && match.Success ? match.Value : string.Empty;
        }
    }
}
